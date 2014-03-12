namespace EnergyTrading.Xml
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// A hierarchy of XPaths to validate.
    /// </summary>
    public interface IXPathValidator
    {
        /// <summary>
        /// Gets or sets the XPath to validate.
        /// </summary>
        string XPath { get; set; }

        /// <summary>
        /// Gets or sets the child validation items to check if the <see cref="XPath"/> is present.
        /// </summary>
        IList<IXPathValidator> Children { get; set; }

        /// <summary>
        /// Validate a document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        IList<string> Validate(XDocument document);

        /// <summary>
        /// Validate a document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="xnm"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        IEnumerable<string> Validate(XDocument document, XmlNamespaceManager xnm, string prefix);
    }
}