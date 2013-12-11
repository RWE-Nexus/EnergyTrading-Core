namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Used to map to and from XML
    /// </summary>
    public class XmlMappingEngine : IXmlMappingEngine
    {
        protected const string XsiPrefix = "xsi";
        protected const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

        private readonly IXmlMapperFactory factory;
        private readonly IDictionary<Tuple<string, string>, Type> xmlTypes;
        private Context context;

        /// <summary>
        /// Create a new instance of the <see cref="XmlMappingEngine" /> class.
        /// </summary>
        /// <param name="factory">Factory to use.s</param>
        public XmlMappingEngine(IXmlMapperFactory factory)
        {
            this.factory = factory;
            xmlTypes = new ConcurrentDictionary<Tuple<string, string>, Type>();

            XmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            NamespaceManager = new BaseNamespaceManager(XmlNamespaceManager);

            // Always register value that allows us to declare XML types.
            RegisterNamespace(XsiPrefix, XsiNamespace);
        }

        /// <copydocfrom cref="IMappingEngine.Context" />
        public Context Context
        {
            get { return context ?? (context = new Context()); }
            set { context = value; }
        }
      
        /// <summary>
        /// Gets the namespace manager used for handling namespaces and their prefixes
        /// </summary>
        public INamespaceManager NamespaceManager { get; private set; }

        /// <summary>
        /// Gets the internal namespace manager.
        /// </summary>
        public XmlNamespaceManager XmlNamespaceManager { get; private set; }

        /// <copydocfrom cref="IXmlMappingEngine.CreateDocument{T}" />
        public XElement CreateDocument<TSource>(TSource source)
        {
            var result = Map<TSource, XElement>(source);

            // Ok, now emit XML namespace attributes for all registered namespaces
            var dictionary = new Dictionary<string, string>();
            foreach (var ns in XmlNamespaceManager.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml))
            {
                dictionary.Add(ns.Key, ns.Value);
            }

            foreach (var pair in dictionary)
            {
                var attrib = new XAttribute(XNamespace.Xmlns + pair.Key, pair.Value);
                try
                {
                    result.Add(attrib);
                }
                catch (Exception)
                {                    
                }
            }

            return result;
        }

        /// <copydocfrom cref="IXmlMappingEngine.LookupPrefix" />
        public string LookupPrefix(string xmlNamespace)
        {
            return NamespaceManager.LookupPrefix(xmlNamespace);
        }

        /// <copydocfrom cref="IMappingEngine.Map{T,S}(T)" />
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper<TSource, TDestination>().Map(source);
        }

        /// <copydocfrom cref="IMappingEngine.Map{T,S}(IEnumerable{T})" />
        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return source.Select(Map<TSource, TDestination>);
        }

        /// <copydocfrom cref="IXmlMappingEngine.Map{T}(T, string, string, bool, bool)" />
        public XElement Map<TSource>(TSource source, string nodeName, string xmlNamespace = null, bool outputDefault = false, bool useDynamicResolution = false)
        {
            var typeOfTypeParameter = typeof(TSource);
            if (!useDynamicResolution
                || typeOfTypeParameter.IsValueType
                || source == null
                || typeOfTypeParameter == source.GetType())
            {
                return Mapper<TSource, XElement>().Map(source, nodeName, xmlNamespace, outputDefault);
            }

            return Map((dynamic)source, nodeName, xmlNamespace, outputDefault, false);
        }

        /// <copydocfrom cref="IXmlMappingEngine.Map{T, D}(T, D)" />
        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper<TSource, TDestination>().Map(source, destination);
        }

        /// <copydocfrom cref="IXmlMappingEngine.Map{T, D}(T, string, string, string, string, int)" />
        public TDestination Map<TSource, TDestination>(TSource source, string nodeName, string xmlNamespace = null, string xmlType = "", string xmlPrefix = "", int index = -1)
        {
            return (TDestination) Mapper<TSource, TDestination>(xmlNamespace, xmlType).Map(source, nodeName, xmlNamespace, xmlPrefix, index);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(T, string, string, bool)" />
        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, outputDefault);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(T, string, string, bool)" />
        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, nodeName, outputDefault);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(T, string, string, string, string, bool)" />
        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, nodeName, collectionNodeNamespacePrefix, collectionItemNodeNamespacePrefix, outputDefault);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(IList{T}, string, string, string, string, bool)" />
        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, outputDefault);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(IList{T}, string, string, bool)" />
        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, nodeName, outputDefault);
        }

        /// <copydocfrom cref="IXmlMappingEngine.MapList{T, D}(IList{T}, string, string, string, string, bool)" />
        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false)
        {
            return Mapper<TSource, TDestination>().MapList(source, collectionNode, collectionItemNodeName, collectionNodeNamespace, collectionItemNodeNamespace, outputDefault);
        }

        /// <summary>
        /// Register a <see cref="IXmlMapper{T, D}"/> to use.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="mapper">The mapper to register</param>
        /// <param name="name">Name of the mapper</param>
        public void RegisterMap<TSource, TDestination>(IMapper<TSource, TDestination> mapper, string name = null)
        {
            var xmlMapper = (IXmlMapper<TSource, TDestination>)mapper;
            if (xmlMapper == null)
            {
                throw new ArgumentException("Mapper is not an IXmlMapper", "mapper");
            }

            factory.Register(xmlMapper, name);
        }

        /// <copydocfrom cref="IXmlMappingEngine.RegisterNamespace" />
        public void RegisterNamespace(string xmlPrefix, string xmlNamespace)
        {
            NamespaceManager.RegisterNamespace(xmlPrefix, xmlNamespace);
        }

        /// <copydocfrom cref="IXmlMappingEngine.RegisterXmlType" />        
        public void RegisterXmlType(string xmlNamespace, string xmlType, Type type)
        {
            var key = new Tuple<string, string>(xmlNamespace, xmlType);
            xmlTypes[key] = type;
        }

        /// <summary>
        /// Gets a <see cref="IXmlMapper{T, D}" />
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="name">Name of the mapper</param>
        /// <returns>A <see cref="IXmlMapper{T, D}" /></returns>
        public IXmlMapper<TSource, TDestination> Mapper<TSource, TDestination>(string name = null)
        {
            var mapper = Mapper(typeof(TSource), typeof(TDestination), name);
            return (IXmlMapper<TSource, TDestination>)mapper;
        }

        /// <summary>
        /// Gets a <see cref="IXmlMapper{T, D}" /> for an XML type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="xmlNamespace">Namespace of the XML type</param>
        /// <param name="xmlType">XML type of the <typeparamref name="TSource"/></param>
        /// <returns>A <see cref="IXmlMapper{T, D}" /></returns>
        protected IXmlMapper<TSource> Mapper<TSource, TDestination>(string xmlNamespace, string xmlType)
        {
            if (string.IsNullOrEmpty(xmlType))
            {
                var mapper = Mapper<TSource, TDestination>();
                return (IXmlMapper<TSource>)mapper;
            }

            var key = new Tuple<string, string>(xmlNamespace, xmlType);
            Type type;
            if (xmlTypes.TryGetValue(key, out type))
            {
                return (IXmlMapper<TSource>)Mapper(typeof(TSource), type);
            }

            throw new MappingException(string.Format("Cannot handle xml type: '{0}/{1}'", xmlNamespace, xmlType));
        }

        /// <summary>
        /// Gets a <see cref="IXmlMapper{T, D}" />
        /// </summary>
        /// <param name="source">Type of the source.</param>
        /// <param name="destination">Type of the destination.</param>
        /// <param name="name">Name of the mapper</param>
        /// <returns>A <see cref="IXmlMapper{T, D}" /></returns>
        protected object Mapper(Type source, Type destination, string name = null)
        {
            return factory.Mapper(source, destination, name);
        }
    }
}