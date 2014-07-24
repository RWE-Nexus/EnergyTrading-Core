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
        /// If the item can be mapped (i.e. source is not null) then the destination is retrieved and Engine.Map is called
        /// allows for the source == null check before retrieving the destination (in case you don't want to retrieve it if the source is not available)
        /// </summary>
        /// <typeparam name="TSource">mapping source type</typeparam>
        /// <typeparam name="TDestination">mapping destination type</typeparam>
        /// <param name="engine">instance of IMappingEngine</param>
        /// <param name="source">instance of source</param>
        /// <param name="retriever">func to retrieve instance of destination</param>
        /// <returns>the retrieved instance of TDestination after mapping process</returns>
        public static TDestination RetrieveAndMap<TSource, TDestination>(this IMappingEngine engine, TSource source, Func<TDestination> retriever)
        {
            if (engine == null || (!(source is ValueType) && source == null) || retriever == null)
            {
                return default(TDestination);
            }

            var destination = retriever();
            engine.Map(source, destination);
            return destination;
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
            return engine.MapList<XPathProcessor, TDestination>(xpathProcessor, collectionNode, string.Empty, xmlPrefix, outputDefault: outputDefault);
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