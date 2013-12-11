namespace EnergyTrading.Mapping.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// Extensions for mapping, need to be in a different namespace to avoid clashes with core classes.
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Append some content to a node.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elements"></param>
        [Obsolete("Use LinqExtensions.Add")]
        public static void Append(this XElement element, IEnumerable<object> elements)
        {
            foreach (var x in elements)
            {
                element.Add(x);
            }
        }

        public static XElement Map<TSource>(this IXmlMappingEngine engine, TSource source)
        {
            return engine.Map<TSource, XElement>(source);
        }

        public static TDestination Map<TDestination>(this IXmlMappingEngine engine, XPathProcessor xpathProcessor, string nodeName, string xmlPrefix = "")
        {
            return engine.Map<XPathProcessor, TDestination>(xpathProcessor, nodeName, xmlPrefix: xmlPrefix);
        }

        public static TDestination Map<TDestination>(this IXmlMappingEngine engine, XPathProcessor xpathProcessor)
        {
            return engine.Map<XPathProcessor, TDestination>(xpathProcessor);
        }

        public static XElement MapList<TSource>(this IXmlMappingEngine engine, IList<TSource> source, string collectionNode, bool outputDefault = false, string xmlNamespace = null)
        {
            var element = engine
                .MapList<TSource, XElement>(source, collectionNode, outputDefault)
                .InNamespace(xmlNamespace);
            if (!outputDefault && (element == null || element.IsEmpty))
            {
                return null;
            }

            return element;
        }

        public static List<TDestination> MapList<TDestination>(this IXmlMappingEngine engine, XPathProcessor xpathProcessor, string collectionNode, string xmlPrefix = "", bool outputDefault = false)
        {
            return engine.MapList<XPathProcessor, TDestination>(xpathProcessor, collectionNode, string.Empty, collectionNodeNamespacePrefix: xmlPrefix, outputDefault: outputDefault);
        }

        public static XElement InNamespace(this XElement xelement, string nameSpace)
        {
            if (xelement == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(nameSpace))
            {
                return xelement;
            }

            XNamespace xns = nameSpace;
            xelement.Name = xns + xelement.Name.LocalName;

            return xelement;
        }
    }
}