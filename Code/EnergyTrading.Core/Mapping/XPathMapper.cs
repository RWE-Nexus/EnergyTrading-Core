namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Extends <see cref="Mapper{XPathProcesoor, T}"/> with <see cref="IXmlMapper{XPathProcessor, T}"/> features.
    /// </summary>
    /// <typeparam name="TDestination">Type of entity to map.</typeparam>
    public abstract class XPathMapper<TDestination> : Mapper<XPathProcessor, TDestination>, IXmlMapper<XPathProcessor, TDestination>, IXmlMapper<XPathProcessor>
        where TDestination : class, new()
    {
        protected const string XsiPrefix = "xsi";
        protected const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        private readonly List<Tuple<string, string>> namespaces;
        private IXmlMappingEngine engine;

        /// <summary>
        /// Create a new instance of the <see cref="XPathMapper{T}" /> class.
        /// </summary>
        protected XPathMapper() : this(string.Empty)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XPathMapper{T}" /> class.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        protected XPathMapper(string nodeName) : this(nodeName, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XPathMapper{T}" /> class.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="engine">Mapping engine to use.</param>        
        protected XPathMapper(string nodeName, IXmlMappingEngine engine)
        {
            namespaces = new List<Tuple<string, string>>();

            Engine = engine;

            NodeName = nodeName;
            Namespace = string.Empty;
            NamespacePrefix = string.Empty;
            XmlType = string.Empty;
            Mappings = new List<XmlPropertyMap>();
        }

        /// <summary>
        /// Gets the underlying XmlType, used to emit into an xsi:Type attribute
        /// </summary>
        public string XmlType { get; private set; }

        /// <summary>
        /// Gets the namespace of the XML type.
        /// </summary>
        public string XmlTypeNamespace { get; private set; }

        /// <summary>
        /// Gets the namespace prefix of the XML type.
        /// </summary>
        public string XmlTypeNamespacePrefix { get; private set; }

        /// <summary>
        /// Gets the XML mapping engine.
        /// </summary>
        protected new IXmlMappingEngine Engine
        {
            get { return engine ?? (engine = new NullXmlMappingEngine()); }
            private set { engine = value; }
        }

        /// <summary>
        /// Gets or sets the XML namespace.
        /// </summary>
        protected virtual string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the XML namespace prefix.
        /// </summary>
        protected virtual string NamespacePrefix { get; set; }

        /// <summary>
        /// Gets or sets the XML node name.
        /// </summary>
        protected virtual string NodeName { get; set; }

        protected List<XmlPropertyMap> Mappings { get; set; }

        /// <inheritdoc />
        public override TDestination Map(XPathProcessor source)
        {
            return Map(source, NodeName);
        }

        /// <copydocfrom cref="IXmlMapper{T, D}.Map(T, string, string, bool)" />
        public TDestination Map(XPathProcessor source, string nodeName, string xmlNamespace = "", bool outputDefault = false)
        {
            RegisterNamespace(source, NamespacePrefix, Namespace);

            // Push both the node and the namespace - presumption that the namespace applies to the node
            source.Push(nodeName, Namespace);
            try
            {
                if (!source.CurrentNode())
                {
                    return null;
                }

                RegisterNamespace(source, XsiPrefix, XsiNamespace);
                var xmlType = source.ToString("type", XsiPrefix, isAttribute: true);

                TDestination destination;

                // This mapper can handle it if there's no type specified or it's our type
                if (string.IsNullOrEmpty(xmlType) || string.IsNullOrEmpty(XmlType))
                {
                    destination = CreateAndMap(source);
                }
                else
                {
                    var t = XmlTypeInfo(source, xmlType);
                    if (XmlTypeNamespace == t.Item1 && XmlType == t.Item2)
                    {
                        destination = CreateAndMap(source);
                    }
                    else
                    {
                        // Otherwise, pass off to the engine
                        // NB Use the current namespace for the node name, not the target mapper's
                        if (!nodeName.Contains(":"))
                        {
                            nodeName = string.Format("{0}:{1}", NamespacePrefix, nodeName);
                        }

                        destination = Engine.Map<XPathProcessor, TDestination>(source, nodeName, t.Item1, t.Item2);
                    }
                }

                return destination;
            }
            finally
            {
                // Make sure we unwind the stack in all cases
                source.Pop();   
            }
        }

        /// NOTE: No defaults as we can't enforce implementation - caller determines passed values - C# standard
        object IXmlMapper<XPathProcessor>.Map(XPathProcessor source, string nodeName, string xmlNamespace, string xmlPrefix, int index)
        {
            return Map(source, nodeName, xmlNamespace, xmlPrefix, index);
        }

        public List<TDestination> MapList(XPathProcessor source, string collectionNode, bool outputDefault = false)
        {
            return MapList(source, collectionNode, NodeName, outputDefault);
        }

        public virtual List<TDestination> MapList(XPathProcessor source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            return MapList(source, collectionNode, nodeName, NamespacePrefix, outputDefault: outputDefault);
        }

        public virtual List<TDestination> MapList(XPathProcessor source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false)
        {
            // Default namespace
            RegisterNamespace(source, NamespacePrefix, Namespace);

            string collNamespace;
            var list = new List<TDestination>();

            if (string.IsNullOrWhiteSpace(collectionNodeNamespacePrefix))
            {
                // It's the same as normal
                collectionNodeNamespacePrefix = NamespacePrefix;
                collNamespace = Namespace;
            }
            else
            {
                // Look it up.
                collNamespace = source.LookupNamespace(collectionNodeNamespacePrefix);
                if (string.IsNullOrWhiteSpace(collNamespace))
                {
                    throw new MappingException(string.Format("No namespace registered for {0}", collectionNodeNamespacePrefix));
                }
            }

            if (!string.IsNullOrEmpty(collectionNode))
            {
                source.Push(collectionNode, collNamespace, collectionNodeNamespacePrefix);
            }

            if (string.IsNullOrEmpty(nodeName))
            {
                nodeName = NodeName;
            }

            var index = 1;
            TDestination item;
            do
            {
                item = Map(source, nodeName, string.Empty, collectionItemNodeNamespacePrefix, index++);
                if (item != null)
                {
                    list.Add(item);
                }
            }
            while (item != null);

            if (!string.IsNullOrEmpty(collectionNode))
            {
                source.Pop();
            }

            return list;
        }

        public virtual TDestination MapList(IEnumerable<XPathProcessor> source, string collectionNode, bool outputDefault = false)
        {
            throw new NotSupportedException();
        }

        public virtual TDestination MapList(IEnumerable<XPathProcessor> source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            throw new NotSupportedException();
        }

        public virtual TDestination MapList(IEnumerable<XPathProcessor> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false)
        {
            throw new NotSupportedException();
        }

        protected virtual void InitializeXmlType(string xmlPrefix, string xmlNamespace, string xmlType)
        {
            XmlType = xmlType;
            XmlTypeNamespace = xmlNamespace;
            XmlTypeNamespacePrefix = xmlPrefix;

            Engine.RegisterNamespace(xmlPrefix, xmlNamespace);
            Engine.RegisterXmlType(xmlNamespace, xmlType, typeof(TDestination));
        }

        /// <summary>
        /// Initialize a XML mapping
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <param name="xpath"></param>
        /// <param name="xmlNamespace"></param>
        /// <returns></returns>
        protected XmlPropertyMapExpression InitializeMap(Expression<Func<TDestination, object>> propertyExpression, string xpath = "", string xmlNamespace = "")
        {
            return InitializeMap(ReflectionExtension.GetPropertyInfo(propertyExpression), xpath);
        }

        protected XmlPropertyMapExpression InitializeMap(PropertyInfo propertyInfo, string xpath = "")
        {
            return InitializeMap(propertyInfo, DeterminMapTarget(propertyInfo.PropertyType), xpath);
        }

        protected XmlPropertyMapExpression InitializeMap(PropertyInfo propertyInfo, XmlMapTarget target, string xpath = "")
        {
            var map = new XmlPropertyMap(propertyInfo, target);

            // Default the name if we don't have one for value
            if (string.IsNullOrEmpty(xpath) && target == XmlMapTarget.Value)
            {
                xpath = propertyInfo.Name;
            }

            map.XPath = xpath;

            Mappings.Add(map);

            return new XmlPropertyMapExpression(map);
        }

        protected TDestination Map(XPathProcessor source, string nodeName, string xmlNamespace, string xmlPrefix, int index)
        {
            RegisterNamespace(source, NamespacePrefix, Namespace);

            // Namespace and node provide context, however the prefix if present overrides the node's actual namespace
            source.Push(nodeName, Namespace, xmlPrefix, index);
            try
            {
                if (!source.CurrentNode())
                {
                    return null;
                }

                RegisterNamespace(source, XsiPrefix, XsiNamespace);
                var xmlType = source.ToString("type", XsiPrefix, isAttribute: true);

                TDestination destination;

                // This mapper can handle it if there's no type specified or it's our type
                if (string.IsNullOrEmpty(xmlType) || string.IsNullOrEmpty(XmlType))
                {
                    destination = CreateAndMap(source);
                }
                else
                {
                    var t = XmlTypeInfo(source, xmlType);
                    if (t.Item1 == XmlTypeNamespace && t.Item2 == XmlType)
                    {
                        // We're responsible
                        destination = CreateAndMap(source);
                    }
                    else
                    {
                        // Need a mapper for this xsi:type
                        destination = XsiMapper(source, nodeName, t.Item1, t.Item2, index);
                    }
                }

                return destination;
            }
            finally
            {
                // Make sure we unwind the stack in all cases
                source.Pop();
            }
        }

        /// <summary>
        /// Registers a namespace for later processing.
        /// </summary>
        /// <param name="xmlPrefix">Prefix to use as an alias for the XML namespace</param>
        /// <param name="xmlNamespace">XML namespace to register</param>
        protected void RegisterNamespace(string xmlPrefix, string xmlNamespace)
        {
            namespaces.Add(new Tuple<string, string>(xmlPrefix, xmlNamespace));
        }
        
        /// <summary>
        /// Registers the namespaces against the processor if it doesn't exist
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Empty string if no namespace, existing prefix if already defined, namespacePrefix otherwise</returns>
        protected void RegisterNamespaces(XPathProcessor source)
        {
            foreach (var nst in namespaces)
            {
                RegisterNamespace(source, nst.Item1, nst.Item2);
            }
        }

        /// <summary>
        /// Registers the namespace against the processor if it doesn't exist
        /// </summary>
        /// <param name="source"></param>
        /// <param name="xmlPrefix">Prefix to use as an alias for the XML namespace</param>
        /// <param name="xmlNamespace">XML namespace to register</param>
        /// <returns>Empty string if no namespace, existing prefix if already defined, namespacePrefix otherwise</returns>
        protected void RegisterNamespace(XPathProcessor source, string xmlPrefix, string xmlNamespace)
        {
            source.RegisterNamespace(xmlPrefix, xmlNamespace);
        }

        /// <summary>
        /// Map an entity for a particular xsi:type.
        /// </summary>
        /// <param name="source">Processor to use</param>
        /// <param name="nodeName">Node name to use.</param>
        /// <param name="xmlNamespace">XML namespace to use.</param>
        /// <param name="xmlType">XML type to use</param>
        /// <param name="index">Index to use.</param>
        /// <returns>A new instance of the TDestination class mapped from the source.</returns>
        private TDestination XsiMapper(XPathProcessor source, string nodeName, string xmlNamespace, string xmlType, int index)
        {
            // Unwind the current node position, we're about to do the same again.
            source.Pop();

            try
            {
                // NB Use the current namespace for the node name, not the target mapper's
                var destination = Engine.Map<XPathProcessor, TDestination>(
                    source, nodeName, xmlNamespace, xmlType, NamespacePrefix, index);

                return destination;
            }
            finally
            {
                // Put it back where it was
                source.Push(nodeName, xmlNamespace, NamespacePrefix, index);
            }
        }

        private Tuple<string, string> XmlTypeInfo(XPathProcessor source, string xmlType)
        {
            var parts = xmlType.Split(':');
            if (parts.GetUpperBound(0) == 0)
            {
                return new Tuple<string, string>(source.CurrentNamespace, parts[0]);
            }

            // Need the namespace not the registered prefix
            var ns = source.LookupNamespace(parts[0]);
            return new Tuple<string, string>(ns, parts[1]);
        }

        private XmlMapTarget DeterminMapTarget(Type type)
        {
            var target = XmlMapTarget.Value;

            if (type.IsEnum)
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(Guid))
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(string))
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(decimal))
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(DateTime))
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(DateTimeOffset))
            {
                target = XmlMapTarget.Value;
            }
            else if (type == typeof(TimeSpan))
            {
                target = XmlMapTarget.Value;
            }
            else if (type.IsValueType)
            {
                // Trying to make sure we process structs as entities, but exclude nullables
                if (!type.IsPrimitive && !type.FullName.StartsWith("System.Nullable"))
                {
                    target = XmlMapTarget.Entity;
                }
            }
            else
            {
                target = typeof(IEnumerable).IsAssignableFrom(type) ? XmlMapTarget.Collection : XmlMapTarget.Entity;
            }

            return target;
        }

        private TDestination CreateAndMap(XPathProcessor source)
        {
            var destination = CreateDestination();
            var pb = destination as INullableProperties;
            if (pb != null)
            {
                pb.NullProperties.Loading = true;
            }

            Map(source, destination);

            if (pb != null)
            {
                pb.NullProperties.Loading = false;
            }

            return destination;
        }
    }
}