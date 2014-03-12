namespace EnergyTrading.Xml
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using EnergyTrading.Mapping;

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
        /// Gets the validation item to use.
        /// </summary>
        /// <param name="schema">Name/version of schema to use.</param>
        /// <returns>Returns the ValidationItem or null if not found</returns>
        IXPathValidator GetXPathValidator(string schema);

        /// <summary>
        /// Validate against a schema.
        /// </summary>
        /// <param name="xml">Xml to use</param>
        /// <param name="schema">Name/version of schema to use.</param>
        /// <returns></returns>
        /// <exception cref="MappingException">Thrown if SchemaSet schema does not exist</exception>
        IList<ValidationEventArgs> Validate(string xml, string schema = "");

        /// <summary>
        /// Validate against a schema.
        /// </summary>
        /// <param name="document">Document to use</param>
        /// <param name="schemaSet">XmlSchemaSet to use.</param>
        /// <returns></returns>
        IList<ValidationEventArgs> Validate(XDocument document, XmlSchemaSet schemaSet);

        /// <summary>
        /// Validate against a schema.
        /// </summary>
        /// <param name="xml">Xml to use</param>
        /// <param name="schema">Name/version of schema to use.</param>
        /// <returns></returns>
        /// <exception cref="MappingException">Thrown if ValidationItem for the schema does not exist</exception>
        IList<string> ValidateXPaths(string xml, string schema = "");

        /// <summary>
        /// Validate against a schema.
        /// </summary>
        /// <param name="document">Document to use</param>
        /// <param name="validator">XPath validator to use.</param>
        /// <returns></returns>
        IList<string> ValidateXPaths(XDocument document, IXPathValidator validator);
    }
}