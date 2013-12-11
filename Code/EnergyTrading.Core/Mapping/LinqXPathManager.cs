namespace EnergyTrading.Mapping
{
    using System;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Computes the qualified XPath for use in LINQ to XML queries.
    /// </summary>
    public class LinqXPathManager : IXPathManager
    {
        private readonly Func<string, string, string> calculateNamespace;
        private readonly Func<string, string, string> calculateXPath;
        private readonly Func<string, string> nsQualFunc;

        /// <summary>
        /// Create a new instance of the LinqXPathManager class.
        /// </summary>
        /// <param name="namespaceManager">Namespace manager to use.</param>
        public LinqXPathManager(INamespaceManager namespaceManager) : this(namespaceManager, false)
        {
        }

        /// <summary>
        /// Create a new instance of the LinqXPathManager class.
        /// </summary>
        /// <param name="namespaceManager">Namespace manager to use.</param>
        /// <param name="memoize">Whether to memoize (cache) the results.</param>
        public LinqXPathManager(INamespaceManager namespaceManager, bool memoize)
        {
            NamespaceManager = namespaceManager;

            Func<string, string, string> f = DetermineNamespace;
            calculateNamespace = memoize ? f.Memoize() : f;

            Func<string, string, string> g = DetermineXPath;
            calculateXPath = memoize ? g.Memoize() : g;

            Func<string, string> h = QualifyNamespace;
            nsQualFunc = memoize ? h.Memoize() : h;
        }

        private INamespaceManager NamespaceManager { get; set; }

        /// <copydocfrom cref="IXPathManager.QualifyXPath" />
        public string QualifyXPath(string xpath, string prefix, string uri = null, int index = -1, bool isAttribute = false)
        {
            var ns = calculateNamespace(prefix, uri);
            var qxp = calculateXPath(ns, xpath);

            return qxp;
        }

        private string DetermineXPath(string ns, string xpath)
        {
            return ns + xpath;
        }

        private string DetermineNamespace(string prefix, string uri)
        {
            var ns = string.Empty;
            if (!string.IsNullOrEmpty(prefix))
            {
                ns = NamespaceManager.LookupNamespace(prefix, true);
                if (ns == null)
                {
                    throw new MappingException("No namespace for prefix: " + prefix);
                }
            }
            else if (!string.IsNullOrEmpty(uri))
            {
                ns = nsQualFunc(uri);
            }
            return ns;
        }

        private static string QualifyNamespace(string uri)
        {
            return "{" + uri + "}";
        }
    }
}