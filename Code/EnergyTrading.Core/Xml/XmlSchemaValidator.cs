namespace EnergyTrading.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using EnergyTrading.Container;
    using EnergyTrading.Mapping;

    using Microsoft.Practices.ServiceLocation;

    /// <copydocfrom cref="EnergyTrading.Xml.IXmlSchemaValidator" />
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

            return locator.TryGetInstance<XmlSchemaSet>(schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.GetXPathValidator" />
        public IXPathValidator GetXPathValidator(string schema)
        {
            if (string.IsNullOrEmpty(schema))
            {
                throw new ArgumentException("No schema version specified", schema);
            }

            return locator.TryGetInstance<IXPathValidator>(schema);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.Validate(string, string)" />
        public IList<ValidationEventArgs> Validate(string xml, string schema = "")
        {
            if (string.IsNullOrEmpty(schema))
            {
                schema = detector.DetectSchemaVersion(xml);
                if (string.IsNullOrEmpty(schema))
                {
                    throw new NotSupportedException("Cannot determine XML schema");
                }
            }

            var schemaSet = GetSchemaSet(schema);
            if (schemaSet == null)
            {
                throw new NotSupportedException("No SchemaSet available for " + schema);
            }

            return this.Validate(xml, schemaSet);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.Validate(XDocument, XmlSchemaSet)" />
        public IList<ValidationEventArgs> Validate(XDocument document, XmlSchemaSet schemaSet)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (schemaSet == null)
            {
                throw new ArgumentNullException("schemaSet");
            }

            var validation = new List<ValidationEventArgs>();

            // Validate against the schema
            document.Validate(schemaSet, (sender, e) => validation.Add(e), true);
            return validation;
        }

        /// <copydocfrom cref="IXmlSchemaValidator.ValidateXPaths(string, string)" />
        public IList<string> ValidateXPaths(string xml, string schema = "")
        {
            if (string.IsNullOrEmpty(schema))
            {
                schema = detector.DetectSchemaVersion(xml);
                if (string.IsNullOrEmpty(schema))
                {
                    throw new NotSupportedException("Cannot determine XML schema");
                }
            }

            var validator = GetXPathValidator(schema);
            if (validator == null)
            {
                throw new NotSupportedException("No XPathValidator available for " + schema);
            }

            return this.ValidatePaths(xml, validator);
        }

        /// <copydocfrom cref="IXmlSchemaValidator.ValidateXPaths(XDocument, IXPathValidator)" />
        public IList<string> ValidateXPaths(XDocument document, IXPathValidator validator)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }

            return validator.Validate(document);
        }
    }
}