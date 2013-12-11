namespace EnergyTrading.Mapping
{
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Determine the schema version for some XML.
    /// </summary>
    public interface IXmlVersionDetector
    {
        /// <summary>
        /// Determine the schema version of a XML string.
        /// </summary>
        /// <param name="xml">XML string to check</param>
        /// <returns>Schema version of the supplied XML, typically {Schema}.{Version} e.g. Css.2_1 or <see cref="string.Empty"/> if not recognised</returns>
        /// <exception cref="NotSupportedException">If the supplied string is not valid XML</exception>
        string DetectSchemaVersion(string xml);

        /// <summary>
        /// Determine the schema version of a XML element.
        /// </summary>
        /// <param name="element">XElement to check.</param>
        /// <returns>Schema version of the supplied XML, typically {Schema}.{Version} e.g. Css.2_1 or <see cref="string.Empty"/> if not recognised</returns>
        string DetectSchemaVersion(XElement element);
    }
}