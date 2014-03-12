namespace EnergyTrading.Xml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <copydocfrom cref="IXPathValidator" />
    public class XPathValidator : IXPathValidator
    {
        private IList<IXPathValidator> children;

        /// <summary>
        /// Gets or sets the XPath to validate.
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// Gets or sets the child validation items to check if the <see cref="XPath"/> is present.
        /// </summary>
        public IList<IXPathValidator> Children
        {
            get { return children ?? (children = new List<IXPathValidator>()); }
            set { children = value; }
        }

        /// <copydocfrom cref="IXPathValidator.Validate(XDocument)" />
        public IList<string> Validate(XDocument document)
        {
            var xnm = DefineNamespaceManager(document, "x");

            return Validate(document, xnm, "x").Where(result => !string.IsNullOrEmpty(result)).ToList();
        }

        /// <copydocfrom cref="IXPathValidator.Validate(XDocument, XmlNamespaceManager, string)" />
        public IEnumerable<string> Validate(XDocument document, XmlNamespaceManager xnm, string prefix)
        {
            if (Children.Count == 0)
            {
                yield return ValidatePath(document, xnm, prefix);
            }
            else if (string.IsNullOrEmpty(ValidatePath(document, xnm, prefix)))
            {
                // Valid means it exists, so we need to process the children
                foreach (var result in Children.SelectMany(child => child.Validate(document, xnm, prefix)))
                {
                    yield return result;
                }
            }
            else
            {
                yield return string.Empty;
            }
        }

        private XmlNamespaceManager DefineNamespaceManager(XDocument document, string prefix)
        {
            var ns = document.Root.Name.Namespace;
            var xnm = new XmlNamespaceManager(new NameTable());
            xnm.AddNamespace(prefix, ns.ToString());

            return xnm;
        }

        private string ValidatePath(XDocument document, XmlNamespaceManager xnm, string prefix)
        {
            try
            {
                var nsxpath = XPath.QualifyXPath(prefix);
                var check = ((IEnumerable)document.XPathEvaluate(nsxpath, xnm)).Cast<XObject>().FirstOrDefault();
                return check != null ? string.Empty : XPath;
            }
            catch (Exception ex)
            {
                return XPath + ": " + ex.Message;
            }
        }
    }
}