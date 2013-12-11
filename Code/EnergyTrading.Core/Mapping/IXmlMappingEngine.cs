namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>
    /// Extends a <see cref="IMappingEngine" /> with methods to assist when mapping to and from XML.
    /// </summary>
    public interface IXmlMappingEngine : IMappingEngine
    {
        /// <summary>
        /// Creates a document from the source.
        /// <para>
        /// Similar to Map, but allows us to know where the root of the document is so we can centrally create
        /// XML namespace references.
        /// </para>
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <param name="source">Source object to serialize.</param>
        /// <returns>A new <see cref="XElement"/> containing the serialized object.</returns>
        XElement CreateDocument<TSource>(TSource source);

        /// <summary>
        /// Lookup a prefix namespace from a namespace.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Prefix if found, null otherwise.</returns>
        string LookupPrefix(string uri);

        /// <summary>
        /// Construct an XElement from an entity
        /// </summary>
        /// <typeparam name="TSource">Type of the source</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="nodeName">Name to emit.</param>
        /// <param name="xmlNamespace">Namespace to use for the nodeName.</param>
        /// <param name="outputDefault">Whether to output, even if the source has its default value.</param>
        /// <param name="useDynamicResolution">
        /// If <c>true</c> and <paramref name="source"/> instance is subtype of <typeparamref name="TSource"/>
        /// will attempt to resolve the concrete mapper instead of using <typeparamref name="TSource"/> mapper.
        /// </param>
        /// <returns>A new <see cref="XElement" /> containing the serialized object.</returns>
        XElement Map<TSource>(TSource source, string nodeName, string xmlNamespace = null, bool outputDefault = false, bool useDynamicResolution = false);

        /// <summary>
        /// Map the source to a created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="nodeName">Name to emit.</param>
        /// <param name="xmlNamespace">Namespace to use if provided, otherwise use the destination's namespace</param>        
        /// <param name="xmlType">XML type to emit</param>
        /// <param name="xmlPrefix">Prefix to use for the XML namespace</param>
        /// <param name="index">Index to use if the node is a collection element.</param>
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination Map<TSource, TDestination>(TSource source, string nodeName, string xmlNamespace = null, string xmlType = "", string xmlPrefix = "", int index = -1);

        /// <summary>
        /// Map the source to a list of created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="collectionNode">Name of the collection node.</param>
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>     
        /// <returns>Generated list of objects with values mapped from the source</returns>
        List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, bool outputDefault = false);

        /// <summary>
        /// Map the source to a list of created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="nodeName">Name to emit.</param>
        /// <param name="collectionNode">Name of the collection node.</param> 
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>      
        /// <returns>Generated list of objects with values mapped from the source</returns>
        List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, bool outputDefault = false);

        /// <summary>
        /// Map the source to a list of created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="nodeName">Name to emit.</param>
        /// <param name="collectionNode">Name of the collection node.</param>
        /// <param name="collectionNodeNamespacePrefix">Collection NamespacePrefix to use if provided, otherwise use the destination's NamespacePrefix</param>
        /// <param name="collectionItemNodeNamespacePrefix">Collection item Namespace to use if provided, otherwise use the destination's namespacePrefix</param>
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>
        /// <returns>Generated list of objects with values mapped from the source</returns>
        List<TDestination> MapList<TSource, TDestination>(TSource source, string collectionNode, string nodeName, string collectionNodeNamespacePrefix = "", string collectionItemNodeNamespacePrefix = "", bool outputDefault = false);

        /// <summary>
        /// Map a list of source to a created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="collectionNode">Name of the collection node.</param>
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, bool outputDefault = false);

        /// <summary>
        /// Map a list of source to a created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="nodeName">Name to emit.</param>
        /// <param name="collectionNode">Name of the collection node.</param>   
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>  
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string nodeName, bool outputDefault = false);

        /// <summary>
        /// Map a list of source to a created destination
        /// </summary>
        /// <typeparam name="TSource">Type of the source.</typeparam>
        /// <typeparam name="TDestination">Type of the destination.</typeparam>
        /// <param name="source">Source object to use.</param>
        /// <param name="collectionNode">Name of the collection node.</param>
        /// <param name="collectionItemNodeName">Name to emit for items in the collection.</param>
        /// <param name="collectionNodeNamespace">Collection Namespace to use if provided, otherwise use the destination's namespace</param>
        /// <param name="collectionItemNodeNamespace">Collection item Namespace to use if provided, otherwise use the destination's namespace</param>
        /// <param name="outputDefault">Do we return an empty element if the collection is empty</param>
        /// <returns>Generated object with values mapped from the source</returns>
        TDestination MapList<TSource, TDestination>(IList<TSource> source, string collectionNode, string collectionItemNodeName, string collectionNodeNamespace = "", string collectionItemNodeNamespace = "", bool outputDefault = false);

        /// <summary>
        /// Register an XML Type and it's associated namespace and target class.
        /// </summary>
        /// <param name="xmlNamespace">XML namespace for the type.</param>
        /// <param name="xmlType">XML type to register.</param>
        /// <param name="type">CLR type to register.</param>
        void RegisterXmlType(string xmlNamespace, string xmlType, Type type);

        /// <summary>
        /// Register a namespace and prefix to use.
        /// </summary>
        /// <param name="xmlPrefix">Prefix to register for the namespace.</param>
        /// <param name="xmlNamespace">XML namespace to register.</param>
        void RegisterNamespace(string xmlPrefix, string xmlNamespace);
    }
}