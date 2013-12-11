namespace EnergyTrading.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using EnergyTrading.Extensions;

    /// <summary>
    /// Implements the <see cref="INamespaceManager" /> using internal dictionaries.
    /// </summary>
    public class NamespaceManager : INamespaceManager
    {
        private readonly Dictionary<string, string> prefixDictionary;
        private readonly Dictionary<string, string> namespaceDictionary;
        private readonly Func<string, string> qualifyNs;

        private readonly object syncLock;

        public NamespaceManager() : this(false)
        {
        }

        public NamespaceManager(bool memoize)
        {
            prefixDictionary = new Dictionary<string, string>();
            namespaceDictionary = new Dictionary<string, string>();

            // Memoize the namespace qualification function.
            Func<string, string> f = this.QualifyNs;
            qualifyNs = memoize ? f.Memoize() : f;

            syncLock = new object();
        }

        /// <contentfrom cref="INamespaceManager.RegisterNamespace" />
        public void RegisterNamespace(string prefix, string xmlNamespace)
        {
            prefix = prefix ?? string.Empty;
            xmlNamespace = xmlNamespace ?? string.Empty;

            if (string.IsNullOrEmpty(xmlNamespace))
            {
                return;
            }

            var pf2 = LookupNamespace(xmlNamespace);
            if (!string.IsNullOrEmpty(pf2))
            {
                return;
            }

            if (prefix.Contains(":"))
            {
                throw new XmlException("The ':' character, hexadecimal value 0x3A, cannot be included in a name - " + prefix);
            }

            lock (syncLock)
            {
                prefixDictionary[xmlNamespace] = prefix;
                namespaceDictionary[prefix] = xmlNamespace;
            }
        }

        /// <contentfrom cref="INamespaceManager.LookupNamespace" />
        public string LookupNamespace(string prefix, bool xname = false)
        {
            prefix = prefix ?? string.Empty;
            string value;
            if (this.namespaceDictionary.TryGetValue(prefix, out value))
            {
                return xname ? qualifyNs(value) : value;
            }
            return null;
        }

        /// <contentfrom cref="INamespaceManager.LookupPrefix" />
        public string LookupPrefix(string uri)
        {
            uri = uri ?? string.Empty;
            string prefix;
            return this.prefixDictionary.TryGetValue(uri, out prefix) ? prefix : null;
        }

        /// <contentfrom cref="INamespaceManager.NamespaceExists" />
        public bool NamespaceExists(string uri)
        {
            uri = uri ?? string.Empty;
            string prefix;
            return this.prefixDictionary.TryGetValue(uri, out prefix);
        }

        /// <contentfrom cref="INamespaceManager.PrefixExists" />
        public bool PrefixExists(string prefix)
        {
            prefix = prefix ?? string.Empty;
            string value;
            return this.namespaceDictionary.TryGetValue(prefix, out value);
        }

        private string QualifyNs(string uri)
        {
            return uri == string.Empty ? string.Empty : "{" + uri + "}";
        }
    }
}