namespace EnergyTrading.Mapping
{
    using System.Xml.Linq;
    using System.Xml.Schema;

    /// <summary>
    /// Validate XML against a schema.
    /// </summary>
    public interface IXmlSchemaValidator
    {
        /// <summary>
        /// Gets the schema set to use.
        /// </summary>
        /// <param name="schema">Name/version of schema to use.</param>
        /// <returns>Returns the XmlSchemaSet or null if not found</returns>
        XmlSchemaSet GetSchemaSet(string schema);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        /// <exception cref="MappingException"></exception>
        ValidationEventArgs Validate(XElement element, string schema = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        /// <exception cref="MappingException"></exception>        
        ValidationEventArgs Validate(string xml, string schema = "");
    }
}