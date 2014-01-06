namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    using EnergyTrading.Logging;
    using EnergyTrading.Xml.Linq;

    /// <summary>
    /// Parses an XML document.
    /// </summary>
    /// <remarks>
    /// This implementation uses stacks to maintain the current XML namespace and XPath, adjusting them
    /// by means of Push/Pop methods.
    /// </remarks>
    [DebuggerDisplay("{CurrentPath}")]
    public class XPathProcessor
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Stack<string> nodeStack;
        private readonly Stack<string> namespaceStack;
        private readonly CultureInfo culture;
        private string currentPath;

        /// <summary>
        /// Constructs a new instance of the XPathProcessor class.
        /// </summary>
        public XPathProcessor()
        {
            this.nodeStack = new Stack<string>();
            this.namespaceStack = new Stack<string>();

            this.culture = CultureInfo.InvariantCulture;
            this.currentPath = "/";
        }

        /// <summary>
        /// Gets the current namespace we are processing for.
        /// </summary>
        public string CurrentNamespace { get; private set; }

        /// <summary>
        /// Gets the current XPath we are processing for.
        /// </summary>
        public string CurrentPath
        {
            get { return this.DetermineCurrentPath(); }
        }

        /// <summary>
        /// Gets the namespace manager.
        /// </summary>
        public INamespaceManager NamespaceManager { get; set; }

        /// <summary>
        /// Gets the XPath manager.
        /// </summary>
        public IXPathManager XPathManager { get; set; }

        /// <summary>
        /// Gets the internal namespace manager.
        /// </summary>
        private XmlNamespaceManager XmlNamespaceManager { get; set; }

        /// <summary>
        /// Gets or sets the XPathNavigator we use to process XPaths.
        /// </summary>
        private XPathNavigator Navigator { get; set; }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        /// <remarks>Uses an XPathDocument internally</remarks>
        /// <param name="xml">XML to use.</param>
        public virtual void Initialize(string xml)
        {
            var stream = new StringReader(xml);
            var document = new XPathDocument(stream);
            this.Initialize(document);
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        /// <param name="document">XPathDocument to use</param>
        public void Initialize(XPathDocument document)
        {
            this.InitializeManagers(document.CreateNavigator());
           
            foreach (var ns in document.Namespaces())
            {
                this.RegisterNamespace(ns.Item1, ns.Item2);
            }
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        /// <param name="document">XDocument to use.</param>
        public virtual void Initialize(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            this.InitializeManagers(document.CreateNavigator());          

            foreach (var ns in document.Namespaces())
            {
                this.RegisterNamespace(ns.Item1, ns.Item2);
            }
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        /// <param name="element">XElement to use.</param>
        public virtual void Initialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.InitializeManagers(element.CreateNavigator());           

            foreach (var ns in element.Namespaces())
            {
                this.RegisterNamespace(ns.Item1, ns.Item2);
            }
        }

        /// <summary>
        /// Push the XPath provided onto the current path.
        /// </summary>
        /// <param name="xpath">XPath to push, may contain an index and a prefix, which takes precedence over the namespace</param>
        /// <param name="xmlNamespace">Namespace for the xpath.</param>
        /// <param name="prefix">Prefix used to alias the namespace.</param>
        /// <param name="index">Index if the path is collection element.</param>
        public virtual void Push(string xpath, string xmlNamespace = null, string prefix = "", int index = -1)
        {
            this.PushNamespace(xmlNamespace, prefix);

            var qualifiedPath = Normalize(this.QualifyXPath(xpath, prefix, xmlNamespace, index));
            this.PushPath(qualifiedPath);
        }

        /// <summary>
        /// Pop the top context from the path.
        /// </summary>
        public virtual void Pop()
        {
            this.PopNamespace();
            this.PopPath();
        }

        [Obsolete("Use ToString and specify isAttribute=true")]
        public virtual string AttributeToString(string xpath, string prefix = "", string defaultValue = null)
        {
            return this.ToString(xpath, prefix, true, defaultValue);
        }

        /// <summary>
        /// Gets whether there is a current node.
        /// </summary>
        /// <returns></returns>
        public virtual bool CurrentNode()
        {
            var node = this.Navigator.Select(this.CurrentPath.Substring(0, this.CurrentPath.Length - 1), this.XmlNamespaceManager);
            return node.MoveNext();
        }

        /// <summary>
        /// Determine whether a node is null or not
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="prefix"></param>
        /// <returns>true if the node is not present or is empty, false otherwise</returns>
        public virtual bool IsNull(string xpath, string prefix = "")
        {
            var qualifiedPath = this.CurrentPath + this.QualifyXPath(xpath, prefix);
            var node = this.Navigator.Select(qualifiedPath, this.XmlNamespaceManager);
            var exists = node.MoveNext();
            if (exists)
            {
                return node.Current == null || node.Current.IsEmptyElement;
            }

            return true;
        }

        /// <summary>
        /// Determine if the current node has the named element.
        /// </summary>
        /// <param name="xpath">Relative xpath</param>
        /// <param name="prefix">Prefix for the xpath</param>
        /// <param name="index">Index if xpath is an array</param>
        /// <returns>true if the element is present, otherwise false</returns>
        public virtual bool HasElement(
            string xpath,
            string prefix = "",
            int index = -1)
        {
            var qualifiedPath = this.CurrentPath + this.QualifyXPath(xpath, prefix, isAttribute: false, index: index);
            var node = this.Navigator.Select(qualifiedPath, this.XmlNamespaceManager);

            return node.MoveNext();
        }

        /// <summary>
        /// Determine if the current node has the named attribute.
        /// </summary>
        /// <param name="xpath">Relative xpath</param>
        /// <param name="prefix">Prefix for the xpath</param>
        /// <returns>true if the element is present, otherwise false</returns>
        public virtual bool HasAttribute(
            string xpath,
            string prefix = "")
        {
            var qualifiedPath = this.CurrentPath + this.QualifyXPath(xpath, prefix, isAttribute: true);
            var node = this.Navigator.Select(qualifiedPath, this.XmlNamespaceManager);

            return node.MoveNext();
        }

        [Obsolete("Use HasElement or HasAttribute")]
        public virtual bool HasElementOrAttribute(
            string xpath,
            string prefix = "",
            bool isAttribute = false)
        {
            return isAttribute ? this.HasElement(xpath, prefix) : this.HasAttribute(xpath, prefix);
        }

        /// <summary>
        /// Lookup a namespace based on the prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual string LookupNamespace(string prefix)
        {
            return this.NamespaceManager.LookupNamespace(prefix);
        }

        /// <summary>
        /// Lookup a prefix namespace based on the namespace.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual string LookupPrefix(string uri)
        {
            return this.NamespaceManager.LookupPrefix(uri);
        }

        /// <summary>
        /// Register a namespace and prefix to use
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="prefix"></param>
        public virtual void RegisterNamespace(string prefix, string uri)
        {
            this.NamespaceManager.RegisterNamespace(prefix, uri);
        }

        /// <summary>
        /// Evaluate a node as an integer.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual int ToInt(string xpath, string prefix = "", bool isAttribute = false, int defaultValue = 0)
        {
            return this.ToValue(xpath, x => x.Current.ValueAsInt, prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as a boolean.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual bool ToBool(string xpath, string prefix = "", bool isAttribute = false, bool defaultValue = false)
        {
            return this.ToValue(xpath, x => x.Current.Value.ParseXmlBool(), prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as an decimal.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual decimal ToDecimal(string xpath, string prefix = "", bool isAttribute = false, decimal defaultValue = 0)
        {
            return this.ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(decimal));
                                    return decimal.Parse(x.Current.Value, NumberStyles.Number | NumberStyles.AllowExponent, this.culture);
                                },
                                prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as a float.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual float ToFloat(string xpath, string prefix = "", bool isAttribute = false, float defaultValue = 0)
        {
            return this.ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(float));
                                    return float.Parse(x.Current.Value, this.culture);
                                },
                                prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as an double.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual double ToDouble(string xpath, string prefix = "", bool isAttribute = false, float defaultValue = 0)
        {
            return this.ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(double));
                                    return double.Parse(x.Current.Value, this.culture);
                                },
                                prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as a long.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual long ToLong(string xpath, string prefix = "", bool isAttribute = false, long defaultValue = 0)
        {
            return this.ToValue(xpath, x => x.Current.ValueAsLong, prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as a string.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <param name="defaultValue">Value to return if node not present.</param>
        /// <returns>Value of the node or defaultValue if not present.</returns>
        public virtual string ToString(string xpath, string prefix = "", bool isAttribute = false, string defaultValue = null)
        {
            return this.ToValue(xpath, x => x.Current.Value, prefix, isAttribute, defaultValue);
        }

        /// <summary>
        /// Evaluate a node as an enum.
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <returns>Value of the node.</returns>
        public virtual T ToEnum<T>(string xpath, string prefix = "", bool isAttribute = false)
        {
            return this.ToValue(xpath, x => (T)Enum.Parse(typeof(T), x.Current.Value), prefix, isAttribute);
        }

        /// <summary>
        /// Evaluate a node as a DateTime.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <returns>Value of the node.</returns>
        public virtual DateTime ToDateTime(string xpath, string prefix = "", bool isAttribute = false)
        {
            return this.ToValue(xpath, x => x.Current.ValueAsDateTime, prefix, isAttribute);
        }

        /// <summary>
        /// Evaluate a node as a <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <returns>Value of the node.</returns>
        public virtual DateTimeOffset ToDateTimeOffset(string xpath, string prefix = "", bool isAttribute = false)
        {
            return this.ToValue(xpath, x => DateTimeOffset.Parse(x.Current.Value), prefix, isAttribute);
        }

        /// <summary>
        /// Evaluate a node as a <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="xpath">XPath relative to CurrentNode</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="isAttribute">Whether the node is an attribute or an element.</param>        
        /// <returns>Value of the node.</returns>
        public virtual TimeSpan ToTimeSpan(string xpath, string prefix = "", bool isAttribute = false)
        {
            return this.ToValue(xpath, x => TimeSpan.Parse(x.Current.Value), prefix, isAttribute); 
        }

        /// <summary>
        /// Initialize the internal namespace/xpath managers.
        /// </summary>
        /// <param name="navigator"></param>
        protected virtual void InitializeManagers(XPathNavigator navigator)
        {
            this.Navigator = navigator;
            this.XmlNamespaceManager = new XmlNamespaceManager(this.Navigator.NameTable);

            // TODO: Any easy way of inject this one??
            this.NamespaceManager = new BaseNamespaceManager(this.XmlNamespaceManager);
            if (this.XPathManager == null)
            {
                this.XPathManager = new XPathManager(this.NamespaceManager);
            }
        }

        /// <summary>
        /// Push a namespace onto the stack, used for local namespace prefix evaluation.
        /// </summary>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="prefix">Prefix to use.</param>
        protected void PushNamespace(string xmlNamespace, string prefix)
        {
            // TODO: Make this responsible for registering/resolving prefix -> namespace if xmlNamespace not present?
            this.namespaceStack.Push(this.CurrentNamespace);
            this.CurrentNamespace = xmlNamespace ?? this.CurrentNamespace;
        }

        /// <summary>
        /// Push an XPath onto the stack, relative to the current path.
        /// </summary>
        /// <param name="xpath">XPath to use.</param>
        protected void PushPath(string xpath)
        {
            this.nodeStack.Push(this.CurrentPath);
            this.currentPath += xpath;
        }

        /// <summary>
        /// Determine the current XPath.
        /// </summary>
        /// <returns>The current XPath.</returns>
        protected virtual string DetermineCurrentPath()
        {
            return this.currentPath;
        }

        /// <summary>
        /// Pop the current XML namespace from stack.
        /// </summary>
        protected void PopNamespace()
        {
            this.CurrentNamespace = this.namespaceStack.Pop();
        }

        /// <summary>
        /// Pop the current XPath from the stack.
        /// </summary>
        protected void PopPath()
        {
            this.currentPath = this.nodeStack.Pop();
        }

        /// <summary>
        /// Construct the qualified XPath for the supplied path.
        /// </summary>
        /// <param name="xpath">Base XPath to qualify.</param>
        /// <param name="prefix">Prefix to use, must provide to evaluate qualified attributes</param>
        /// <param name="xmlNamespace">XML namespace to apply.</param>
        /// <param name="index">Index if part of a node collection.</param>
        /// <param name="isAttribute">Whether we are accessing a element or attribute.</param>
        /// <returns>The qualified path.</returns>
        protected virtual string QualifyXPath(string xpath, string prefix, string xmlNamespace = null, int index = -1, bool isAttribute = false)
        {
            // Determine namespace if not provided.
            var ns = xmlNamespace ?? this.CurrentNamespace;
            if (isAttribute && string.IsNullOrEmpty(prefix))
            {
                // Don't qualify the default namespace for attributes
                // TODO: How to inject this, might need schema-qualfied for some apps
                ns = string.Empty;
            }

            return this.XPathManager.QualifyXPath(xpath, prefix, ns, index, isAttribute);
        }

        private static string Normalize(string xpath)
        {
            if (!xpath.EndsWith(@"/"))
            {
                xpath = xpath + @"/";
            }

            return xpath;
        }

        private T ToValue<T>(
            string xpath,
            Func<XPathNodeIterator, T> valueSelector,
            string prefix = "",
            bool isAttribute = false,
            T defaultValue = default(T))
        {
            var qualifiedPath = string.IsNullOrEmpty(xpath) ? this.CurrentPath.Substring(0, this.CurrentPath.Length - 1) : this.CurrentPath + this.QualifyXPath(xpath, prefix, isAttribute: isAttribute);

            return this.Navigator.ToValue(this.XmlNamespaceManager, qualifiedPath, valueSelector, defaultValue);
        }
    }
}