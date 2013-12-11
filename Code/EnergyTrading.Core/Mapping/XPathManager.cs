namespace EnergyTrading.Mapping
{
    using System;
    using System.Globalization;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Computes the qualified XPath for use in XPath queries.
    /// </summary>
    public class XPathManager : IXPathManager
    {
        private readonly Func<string, string, string> prefixFunc;
        private readonly Func<string, string, int, bool, string> xpathFunc;
        private readonly Func<int, string> indexFunc;

        /// <summary>
        /// Creates a new instance of the XPathManager class.
        /// </summary>
        /// <param name="namespaceManager">Namespace manager to use.</param>
        public XPathManager(INamespaceManager namespaceManager)
        {
            this.NamespaceManager = namespaceManager;

            Func<string, string, string> f = this.DeterminePrefix;
            prefixFunc = f.Memoize();

            Func<string, string, int, bool, string> g = this.DetermineXPath;
            xpathFunc = g.Memoize();

            Func<int, string> h = this.Index;
            indexFunc = h.Memoize();
        }

        private INamespaceManager NamespaceManager { get; set; }

        /// <copydocfrom cref="IXPathManager.QualifyXPath" />
        public string QualifyXPath(string xpath, string prefix, string uri = null, int index = -1, bool isAttribute = false)
        {
            var qprefix = prefixFunc(prefix, uri);
            var qxpath = xpathFunc(xpath, qprefix, index, isAttribute);

            return qxpath;
        }

        private string DetermineXPath(string xpath, string prefix, int index, bool isAttribute)
        {
            // Fully qualify but don't want to normalize as we will query from this value.
            // Basic path;
            xpath = prefix + xpath;

            if (isAttribute)
            {
                // Attributes can't have an index and don't need the trailing slash
                xpath = "@" + xpath;
            }
            else if (index > -1)
            {
                // Add the index on
                xpath += indexFunc(index);
            }

            return xpath;
        }

        private string DeterminePrefix(string prefix, string uri)
        {
            // Prefix takes precedence over supplied namespace due to xsi:type handling concerns
            if (string.IsNullOrEmpty(prefix))
            {
                if (!string.IsNullOrEmpty(uri))
                {
                    prefix = NamespaceManager.LookupPrefix(uri);

                    if (string.IsNullOrEmpty(prefix))
                    {
                        // By this stage we should have a prefix
                        throw new MappingException("No prefix for namespace: " + uri);
                    }
                }
            }

            // Always add the colon on if we have a prefix
            if (!string.IsNullOrEmpty(prefix))
            {
                prefix = prefix + ":";
            }

            return prefix;
        }
        
        private string Index(int index)
        {
            return index == -1 ? string.Empty : "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}