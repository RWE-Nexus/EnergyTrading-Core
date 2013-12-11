namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;

    /// <summary>
    /// Extends <see cref="IMapper{T,U}" /> to assist when mapping to and from XML.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IXmlMapper<in TSource, TDestination> : IMapper<TSource, TDestination>
    {
        /// <summary>
        /// Map the source to a created destination
        /// </summary>
        /// <param name="source">Object to map from</param>
        /// <param name="nodeName">Name for the node</param>
        /// <param name="xmlNamespace">Namespace to override the default</param>
        /// <param name="outputDefault"></param>
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination Map(TSource source, string nodeName, string xmlNamespace = null, bool outputDefault = false);

        List<TDestination> MapList(TSource source, string collectionNode, bool outputDefault = false);

        List<TDestination> MapList(TSource source, string collectionNode, string nodeName, bool outputDefault = false);

        List<TDestination> MapList(TSource source, string collectionNode, string collectionItemNode, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false);

        TDestination MapList(IEnumerable<TSource> source, string collectionNode, bool outputDefault = false);

        TDestination MapList(IEnumerable<TSource> source, string collectionNode, string nodeName, bool outputDefault = false);

        TDestination MapList(IEnumerable<TSource> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false);
    }
}