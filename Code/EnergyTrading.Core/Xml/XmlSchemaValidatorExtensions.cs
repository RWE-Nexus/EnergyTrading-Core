namespace EnergyTrading.Xml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.Schema;

    /// <summary>
    /// Extensions for IXmlSchemaValidator
    /// </summary>
    public static class XmlSchemaValidatorExtensions
    {
        /// <copydocfrom cref="IXmlSchemaValidator.Validate(string, string)" />
        public static IList<ValidationEventArgs> Validate(this IXmlSchemaValidator validator, XElement element, string schema = "")
        {
            return validator.Validate(element.ToString(), schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.Validate(string, string)" />
        public static IList<ValidationEventArgs> Validate(this IXmlSchemaValidator validator, string xml, XmlSchemaSet schemaSet)
        {
            var doc = Load(xml);

            return validator.Validate(doc, schemaSet);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.ValidateXPaths(string, string)" />
        public static IList<string> ValidatePaths(this IXmlSchemaValidator validator, XElement element, string schema = "")
        {
            return validator.ValidateXPaths(element.ToString(), schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.ValidateXPaths(string, string)" />
        public static IList<string> ValidatePaths(this IXmlSchemaValidator validator, string xml, IXPathValidator xpathValidator)
        {
            var doc = Load(xml);

            return validator.ValidateXPaths(doc, xpathValidator);
        }

        private static XDocument Load(string xml)
        {
            // TODO: Use Settings here
            return XDocument.Load(new StringReader(xml));
        }
    }
}
