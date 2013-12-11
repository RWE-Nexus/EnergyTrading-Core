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
            nodeStack = new Stack<string>();
            namespaceStack = new Stack<string>();

            culture = CultureInfo.InvariantCulture;
            currentPath = "/";
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
            get { return DetermineCurrentPath(); }
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
            Initialize(document);
        }

        /// <summary>
        /// Initialize the processor.
        /// </summary>
        /// <param name="document">XPathDocument to use</param>
        public void Initialize(XPathDocument document)
        {
            InitializeManagers(document.CreateNavigator());
           
            foreach (var ns in document.Namespaces())
            {
                RegisterNamespace(ns.Item1, ns.Item2);
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

            InitializeManagers(document.CreateNavigator());          

            foreach (var ns in document.Namespaces())
            {
                RegisterNamespace(ns.Item1, ns.Item2);
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

            InitializeManagers(element.CreateNavigator());           

            foreach (var ns in element.Namespaces())
            {
                RegisterNamespace(ns.Item1, ns.Item2);
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
            PushNamespace(xmlNamespace, prefix);

            var qualifiedPath = Normalize(QualifyXPath(xpath, prefix, xmlNamespace, index));
            PushPath(qualifiedPath);
        }

        /// <summary>
        /// Pop the top context from the path.
        /// </summary>
        public virtual void Pop()
        {
            PopNamespace();
            PopPath();
        }

        [Obsolete("Use ToString and specify isAttribute=true")]
        public virtual string AttributeToString(string xpath, string prefix = "", string defaultValue = null)
        {
            return ToString(xpath, prefix, true, defaultValue);
        }

        /// <summary>
        /// Gets whether there is a current node.
        /// </summary>
        /// <returns></returns>
        public virtual bool CurrentNode()
        {
            var node = Navigator.Select(CurrentPath.Substring(0, CurrentPath.Length - 1), XmlNamespaceManager);
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
            var qualifiedPath = CurrentPath + QualifyXPath(xpath, prefix);
            var node = Navigator.Select(qualifiedPath, XmlNamespaceManager);
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
            var qualifiedPath = CurrentPath + QualifyXPath(xpath, prefix, isAttribute: false, index: index);
            var node = Navigator.Select(qualifiedPath, XmlNamespaceManager);

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
            var qualifiedPath = CurrentPath + QualifyXPath(xpath, prefix, isAttribute: true);
            var node = Navigator.Select(qualifiedPath, XmlNamespaceManager);

            return node.MoveNext();
        }

        [Obsolete("Use HasElement or HasAttribute")]
        public virtual bool HasElementOrAttribute(
            string xpath,
            string prefix = "",
            bool isAttribute = false)
        {
            return isAttribute ? HasElement(xpath, prefix) : HasAttribute(xpath, prefix);
        }

        /// <summary>
        /// Lookup a namespace based on the prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual string LookupNamespace(string prefix)
        {
            return NamespaceManager.LookupNamespace(prefix);
        }

        /// <summary>
        /// Lookup a prefix namespace based on the namespace.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public virtual string LookupPrefix(string uri)
        {
            return NamespaceManager.LookupPrefix(uri);
        }

        /// <summary>
        /// Register a namespace and prefix to use
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="prefix"></param>
        public virtual void RegisterNamespace(string prefix, string uri)
        {
            NamespaceManager.RegisterNamespace(prefix, uri);
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
            return ToValue(xpath, x => x.Current.ValueAsInt, prefix, isAttribute, defaultValue);
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
            return ToValue(xpath, x => x.Current.Value.ParseXmlBool(), prefix, isAttribute, defaultValue);
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
            return ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(decimal));
                                    return decimal.Parse(x.Current.Value, NumberStyles.Number | NumberStyles.AllowExponent, culture);
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
            return ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(float));
                                    return float.Parse(x.Current.Value, culture);
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
            return ToValue(xpath,
                                x =>
                                {
                                    Logger.CheckNumericFormat(x, typeof(double));
                                    return double.Parse(x.Current.Value, culture);
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
            return ToValue(xpath, x => x.Current.ValueAsLong, prefix, isAttribute, defaultValue);
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
            return ToValue(xpath, x => x.Current.Value, prefix, isAttribute, defaultValue);
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
            return ToValue(xpath, x => (T)Enum.Parse(typeof(T), x.Current.Value), prefix, isAttribute);
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
            return ToValue(xpath, x => x.Current.ValueAsDateTime, prefix, isAttribute);
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
            return ToValue(xpath, x => DateTimeOffset.Parse(x.Current.Value), prefix, isAttribute);
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
            return ToValue(xpath, x => TimeSpan.Parse(x.Current.Value), prefix, isAttribute); 
        }

        /// <summary>
        /// Initialize the internal namespace/xpath managers.
        /// </summary>
        /// <param name="navigator"></param>
        protected virtual void InitializeManagers(XPathNavigator navigator)
        {
            Navigator = navigator;
            XmlNamespaceManager = new XmlNamespaceManager(Navigator.NameTable);

            // TODO: Any easy way of inject this one??
            NamespaceManager = new BaseNamespaceManager(XmlNamespaceManager);
            if (XPathManager == null)
            {
                XPathManager = new XPathManager(NamespaceManager);
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
            namespaceStack.Push(CurrentNamespace);
            CurrentNamespace = xmlNamespace ?? CurrentNamespace;
        }

        /// <summary>
        /// Push an XPath onto the stack, relative to the current path.
        /// </summary>
        /// <param name="xpath">XPath to use.</param>
        protected void PushPath(string xpath)
        {
            nodeStack.Push(CurrentPath);
            currentPath += xpath;
        }

        /// <summary>
        /// Determine the current XPath.
        /// </summary>
        /// <returns>The current XPath.</returns>
        protected virtual string DetermineCurrentPath()
        {
            return currentPath;
        }

        /// <summary>
        /// Pop the current XML namespace from stack.
        /// </summary>
        protected void PopNamespace()
        {
            CurrentNamespace = namespaceStack.Pop();
        }

        /// <summary>
        /// Pop the current XPath from the stack.
        /// </summary>
        protected void PopPath()
        {
            currentPath = nodeStack.Pop();
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
            var ns = xmlNamespace ?? CurrentNamespace;
            if (isAttribute && string.IsNullOrEmpty(prefix))
            {
                // Don't qualify the default namespace for attributes
                // TODO: How to inject this, might need schema-qualfied for some apps
                ns = string.Empty;
            }

            return XPathManager.QualifyXPath(xpath, prefix, ns, index, isAttribute);
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
            var qualifiedPath = string.IsNullOrEmpty(xpath) ? CurrentPath.Substring(0, CurrentPath.Length - 1) : CurrentPath + QualifyXPath(xpath, prefix, isAttribute: isAttribute);

            return Navigator.ToValue(XmlNamespaceManager, qualifiedPath, valueSelector, defaultValue);
        }
    }
}