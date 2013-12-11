namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Manages XML namespaces.
    /// </summary>
    public interface INamespaceManager
    {
        /// <summary>
        /// Add a namespace to the cache.
        /// </summary>
        /// <param name="prefix">Prefix for the namespace.</param>
        /// <param name="uri">XML namespace to register.</param>
        void RegisterNamespace(string prefix, string uri);

        /// <summary>
        /// Lookup a prefix from a namespace.
        /// </summary>
        /// <param name="prefix">Prefix to lookup</param>
        /// <param name="xname">Whether to return the namespace qualified as a namespace e.g. {http://tempuri.org/} </param>
        /// <returns>Namespace if found, null otherwise.</returns>
        string LookupNamespace(string prefix, bool xname = false);

        /// <summary>
        /// Lookup a prefix namespace from a namespace.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Prefix if found, null otherwise.</returns>
        string LookupPrefix(string uri);

        /// <summary>
        /// Gets whether the namespace is registered.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        bool NamespaceExists(string uri);

        /// <summary>
        /// Gets whether the prefix is registered.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        bool PrefixExists(string prefix);
    }
}