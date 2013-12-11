namespace EnergyTrading.Mapping
{
    using System;
    using System.Xml.Linq;

    using EnergyTrading.Xml.Serialization;

    /// <summary>
    /// XML converter to convert objects to and from XML using version detection and our XML mapping engine.
    /// </summary>
    public class XmlConverter : IXmlConverter
    {
        /// <summary>
        /// Create a new version of the XmlConverter.
        /// </summary>
        /// <param name="versionDetector">Version detector to use.</param>
        /// <param name="mappingEngineFactory">Mapping engine factory to use.</param>
        public XmlConverter(IXmlVersionDetector versionDetector, IXmlMappingEngineFactory mappingEngineFactory)
        {
            if (versionDetector == null)
            {
                throw new ArgumentNullException("versionDetector");
            }
            if (mappingEngineFactory == null)
            {
                throw new ArgumentNullException("mappingEngineFactory");
            }

            VersionDetector = versionDetector;
            Factory = mappingEngineFactory;

            Clear();
        }

        /// <summary>
        /// Gets the mapping engine factory.
        /// </summary>
        protected IXmlMappingEngineFactory Factory { get; private set; }

        /// <summary>
        /// Gets the version detector.
        /// </summary>
        protected IXmlVersionDetector VersionDetector { get; private set; }

        /// <summary>
        /// Gets or sets the namespace manager.
        /// <para>
        /// This controls the registration/resolution of XML namespaces.
        /// </para>
        /// </summary>
        protected INamespaceManager NamespaceManager { get; set; }

        /// <summary>
        /// Gets or sets the XPathManager.
        /// <para>
        /// </para>
        /// </summary>
        protected IXPathManager XPathManager { get; set; }

        /// <copydocfrom cref="IXmlConverter.FromXml{T}(string)" />
        /// <exception cref="MappingException">Thrown if we can't parse the XML, detect version or the deserialize fails</exception>
        public T FromXml<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            XDocument document;
            try
            {
                document = XDocument.Parse(xml);
            }
            catch (Exception ex)
            {
                throw new MappingException("Could not parse xml", ex);
            }

            var processor = CreateProcessor();
            processor.Initialize(document);

            return FromXml<T>(processor, document.Root);
        }

        /// <copydocfrom cref="IXmlConverter.FromXml{T}(XElement)" />
        /// <exception cref="MappingException">Thrown if we can't detect version or the deserialize fails</exception>
        public T FromXml<T>(XElement element)
        {
            if (element == null)
            {
                return default(T);
            }

            var processor = CreateProcessor();
            processor.Initialize(element);

            return FromXml<T>(processor, element);
        }

        /// <summary>
        /// Convert an entity to XML
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="version">The schema version of the XML to produce</param>
        /// <param name="localNamespaces">Declare namespaces locally versus at the top of the element</param>
        /// <returns>The XML string containing the order details</returns>
        public XElement ToXElement<T>(T entity, string version, bool localNamespaces = false)
        {
            try
            {
                // Quick sanity check.
                if (Equals(entity, default(T)))
                {
                    return null;
                }

                var engine = Engine(version);

                // Map will declare namespaces locally whereas CreateDocument declares at the root.
                var element = localNamespaces 
                                ? engine.Map<T, XElement>(entity) 
                                : engine.CreateDocument(entity);

                return element;
            }
            catch (MappingException)
            {
                // Lower level returned mapping exception, don't add anything.
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException(string.Format("Could not convert entity to XML, version {0}: {1}", version, typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Convert an entity to XML
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="version">The schema version of the XML to produce</param>
        /// <param name="localNamespaces">Declare namespaces locally versus at the top of the element</param>
        /// <returns>The XML string containing the order details</returns>
        public string ToXml<T>(T entity, string version, bool localNamespaces = false)
        {
            try
            {                
                var orderElement = ToXElement(entity, version, localNamespaces);
                var xml = orderElement == null ? null : orderElement.ToXmlString();

                return xml;
            }
            catch (MappingException)
            {
                // Lower level returned mapping exception, don't add anything.
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException(string.Format("Could not convert entity to XML, version {0}: {1}", version, typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Clear the internal state of the XmlConverter.
        /// </summary>
        public void Clear()
        {
            NamespaceManager = new NamespaceManager();
            XPathManager = new LinqXPathManager(NamespaceManager);
        }

        /// <summary>
        /// Create an XPathProcessor to handle the conversion.
        /// </summary>
        /// <returns></returns>
        protected virtual XPathProcessor CreateProcessor()
        {
            var processor = new LinqXPathProcessor
            {
                NamespaceManager = NamespaceManager,
                XPathManager = XPathManager,
            };

            return processor;
        }

        /// <summary>
        /// Convert XML to an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="processor"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected T FromXml<T>(XPathProcessor processor, XElement element)
        {
            try
            {
                var version = VersionDetector.DetectSchemaVersion(element);
                if (version == string.Empty)
                {
                    throw new MappingException("Could not detect schema version");
                }

                var entity = Engine(version).Map<XPathProcessor, T>(processor);

                return entity;
            }
            catch (MappingException)
            {
                // Lower level returned mapping exception, don't add anything.
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException("Could not convert document to entity: " + typeof(T).Name, ex);
            }
        }

        private IXmlMappingEngine Engine(string version)
        {
            try
            {
                var engine = Factory.Find(version);
                if (engine != null)
                {
                    return engine;
                }

                throw new MappingException("IXmlMappingEngine not found: " + version);
            }
            catch (MappingException)
            {
                // Lower level returned mapping exception, don't add anything.
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException("IXmlMappingEngine not found: " + version, ex);
            }
        }
    }
}