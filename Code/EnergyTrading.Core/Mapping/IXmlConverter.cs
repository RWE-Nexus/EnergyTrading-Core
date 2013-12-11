namespace EnergyTrading.Mapping
{
    using System.Xml.Linq;

    /// <summary>
    /// Converts entities to and from XML.
    /// </summary>
    public interface IXmlConverter
    {
        /// <summary>
        /// Convert XML into an entity.
        /// </summary>
        /// <typeparam name="T">Entity type to create</typeparam>
        /// <param name="xml">XML to convert</param>
        /// <returns>Deserialized entity or null if xml not provided</returns>
        /// <exception cref="MappingException">Thrown on errors</exception>        
        T FromXml<T>(string xml);

        /// <summary>
        /// Convert XElement into an entity.
        /// </summary>
        /// <typeparam name="T">Entity type to create</typeparam>
        /// <param name="document">XML document to convert</param>
        /// <returns>Deserialized entity or null if XElement not provided.</returns>
        /// <exception cref="MappingException">Thrown on errors</exception>
        T FromXml<T>(XElement document);

        /// <summary>
        /// Convert an entity to XElement.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="version">The schema version of the XML to produce</param>
        /// <param name="localNamespaces">Declare namespaces locally to nodes or at the root</param>
        /// <returns>The XML string containing the order details</returns>
        XElement ToXElement<T>(T entity, string version, bool localNamespaces = false);

        /// <summary>
        /// Convert an entity to XML.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="version">The schema version of the XML to produce</param>
        /// <param name="localNamespaces">Declare namespaces locally to nodes or at the root</param>
        /// <returns>The XML string containing the order details</returns>
        string ToXml<T>(T entity, string version, bool localNamespaces = false);
    }
}