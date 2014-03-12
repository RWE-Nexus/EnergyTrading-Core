namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Extensions for classes in EnergyTrading.Mapping
    /// </summary>
    public static class MappingExtensions
    {
        private const string CurrentXPath = "";

        /// <summary>
        /// Maps a list of DateTime values into a collection
        /// </summary>
        /// <param name="source">XPathProcessor to use</param>
        /// <param name="collectionNode">Collection node name (may be null)</param>
        /// <param name="nodeName">Node name to use</param>
        /// <returns>List of DateTime values</returns>
        public static List<DateTime> MapDateTimeList(this XPathProcessor source, string collectionNode, string nodeName)
        {
            return source.MapList(collectionNode, nodeName, x => x.ToDateTime(CurrentXPath));
        }

        /// <summary>
        /// Maps a list of string values into a collection
        /// </summary>
        /// <param name="source">XPathProcessor to use</param>
        /// <param name="collectionNode">Collection node name (may be null)</param>
        /// <param name="nodeName">Node name to use</param>
        /// <returns>List of strings values</returns>
        public static List<string> MapStringList(this XPathProcessor source, string collectionNode, string nodeName)
        {
            return source.MapList(collectionNode, nodeName, x => x.ToString(CurrentXPath));
        }

        /// <summary>
        /// Maps a list of string values into a collection
        /// </summary>
        /// <param name="source">XPathProcessor to use</param>
        /// <param name="collectionNode">Collection node name (may be null)</param>
        /// <param name="nodeName">Node name to use</param>
        /// <returns>List of strings values</returns>
        public static List<string> MapList(this XPathProcessor source, string collectionNode, string nodeName)
        {
            return source.MapList(collectionNode, nodeName, x => x.ToString(CurrentXPath));
        }

        /// <summary>
        /// Maps a list of simple values into a collection
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="source">XPathProcessor to use</param>
        /// <param name="collectionNode">Collection node name (may be null)</param>
        /// <param name="nodeName">Node name to use</param>
        /// <param name="func">Function to access the data from the XPathProcessor, e.g. x => x.ToInt(CurrentXPath)</param>
        /// <returns>List of values</returns>
        public static List<T> MapList<T>(this XPathProcessor source, string collectionNode, string nodeName, Func<XPathProcessor, T> func)
        {
            if (!string.IsNullOrEmpty(collectionNode))
            {
                source.Push(collectionNode);
            }

            var list = new List<T>();
            var index = 1;
            while (source.HasElement(nodeName, index: index))
            {
                source.Push(nodeName, index: index);
                try
                {
                    list.Add(func(source));
                    index++;
                }
                finally
                {
                    source.Pop();
                }
            }

            if (!string.IsNullOrEmpty(collectionNode))
            {
                source.Pop();
            }

            return list;
        }

        /// <summary>
        /// Maps a list of values into a collection of XElement
        /// </summary>
        /// <typeparam name="T">Type of value to return</typeparam>
        /// <param name="source">XPathProcessor to use</param>
        /// <param name="func">Function to access the data from the XPathProcessor</param>
        /// <returns>List of values</returns>
        public static List<XElement> MapList<T>(this IEnumerable<T> source, Func<T, XElement> func)
        {
            return source.Select(func).ToList();
        }

        /// <summary>
        /// Registers a <see cref="XmlMapper{T}" /> against a <see cref="IXmlMappingEngine" />.
        /// </summary>
        /// <typeparam name="TEntity">Entity we are registering for</typeparam>
        /// <param name="engine">Engine to use</param>
        /// <param name="mapper">Mapper to use</param>
        public static void RegisterMapper<TEntity>(this IXmlMappingEngine engine, XmlMapper<TEntity> mapper)
            where TEntity : class, new()
        {
            engine.RegisterMap<XPathProcessor, TEntity>(mapper);
            engine.RegisterMap<TEntity, XElement>(mapper);
        }
    }
}