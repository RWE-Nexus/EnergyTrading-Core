namespace EnergyTrading.Mapping
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using EnergyTrading.Container;

    using Microsoft.Practices.ServiceLocation;

    /// <copydocfrom cref="IXmlSchemaValidator" />
    public class XmlSchemaValidator : IXmlSchemaValidator
    {
        private readonly IServiceLocator locator;
        private readonly IXmlVersionDetector detector;

        /// <summary>
        /// Creates a new instance of the <see cref="IXmlSchemaValidator" /> class.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="detector"></param>
        public XmlSchemaValidator(IServiceLocator locator, IXmlVersionDetector detector)
        {
            this.locator = locator;
            this.detector = detector;
        }

        /// <copydocfrom cref="IXmlSchemaValidator.GetSchemaSet" />
        public XmlSchemaSet GetSchemaSet(string schema)
        {
            if (string.IsNullOrEmpty(schema))
            {
                throw new ArgumentException("No schema version specified", schema);
            }

            return this.locator.TryGetInstance<XmlSchemaSet>(schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.Validate(XElement, string)" />
        public ValidationEventArgs Validate(XElement element, string schema = "")
        {
            return this.Validate(element.ToString(), schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.Validate(string, string)" />
        public ValidationEventArgs Validate(string xml, string schema = "")
        {
            if (string.IsNullOrEmpty(schema))
            {
                schema = this.detector.DetectSchemaVersion(xml);
            }

            var schemaSet = this.GetSchemaSet(schema);
            if (schemaSet == null)
            {
                throw new MappingException("No SchemaSet available for " + schema);
            }

            ValidationEventArgs validation = null;
            XDocument.Load(new StringReader(xml)).Validate(
                schemaSet,
                (sender, e) => { validation = e; },
                true);

            return validation;
        }        
    }
}