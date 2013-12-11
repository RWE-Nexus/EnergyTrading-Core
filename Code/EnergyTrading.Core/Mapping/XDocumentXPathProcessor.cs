namespace EnergyTrading.Mapping
{
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// An XPathProcessor that uses an <see cref="XDocument"/> rather than a <see cref="XPathDocument" />.
    /// </summary>
    public class XDocumentXPathProcessor : XPathProcessor
    {
        /// <summary>
        /// Gets or sets the root element we use.
        /// </summary>
        protected XContainer RootElement { get; set; }

        /// <inheritdoc />
        public override void Initialize(string xml)
        {
            var document = XDocument.Parse(xml);
            Initialize(document);
        }

        /// <inheritdoc />
        public override void Initialize(XDocument document)
        {
            base.Initialize(document);

            RootElement = document;
        }

        /// <inheritdoc />
        public override void Initialize(XElement element)
        {
            base.Initialize(element);

            RootElement = element;
        }
    }
}