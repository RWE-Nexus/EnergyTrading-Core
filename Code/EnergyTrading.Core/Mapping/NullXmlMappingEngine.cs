namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;

    public class NullXmlMappingEngine : IXmlMappingEngine
    {
        private const string ExceptionMessage = "Supply real XmlMappingEngine to get functionality.";

        public Context Context
        {
            get { throw new MappingException(ExceptionMessage); }
            set { throw new MappingException(ExceptionMessage); }
        }

        public XElement CreateDocument<TSource>(TSource source)
        {
            throw new MappingException(ExceptionMessage);
        }

        public string LookupPrefix(string uri)
        {
            throw new MappingException(ExceptionMessage);
        }

        public XElement Map<TSource>(TSource source, string nodeName, string xmlNamespace = null, bool outputDefault = false, bool useDynamicResolution = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public TDestination Map<TSource, TDestination>(TSource source, string nodeName)
        {
            throw new MappingException(ExceptionMessage);
        }

        public TDestination Map<TSource, TDestination>(TSource source, string nodeName, string xmlNamespace = null, string xmlType = "", string xmlPrefix = "", int index = -1)
        {
            throw new MappingException(ExceptionMessage);
        }

        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string nodeName, bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false)
        {
            throw new MappingException(ExceptionMessage);
        }

        public void RegisterXmlType(string xmlNamespace, string xmlType, Type type)
        {
            throw new MappingException(ExceptionMessage);
        }

        public void RegisterNamespace(string xmlPrefix, string xmlNamespace)
        {
            // NB: Don't throw exception on using this, allows us to use null object pattern from XmlMapper.

            // Do some validation - catches bad behaviour under some testing scenarios
            if (xmlPrefix.Contains(":"))
            {
                throw new XmlException("The ':' character, hexadecimal value 0x3A, cannot be included in a name - " + xmlPrefix);
            }
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            throw new MappingException(ExceptionMessage);
        }

        public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        {
            throw new MappingException(ExceptionMessage);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new MappingException(ExceptionMessage);
        }

        public void RegisterMap<TSource, TDestination>(IMapper<TSource, TDestination> mapper, string name = null)
        {
            throw new MappingException(ExceptionMessage);
        }
    }
}