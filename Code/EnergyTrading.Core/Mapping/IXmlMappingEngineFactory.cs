namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Locates versioned <see cref="IXmlMappingEngine" />s
    /// </summary>
    public interface IXmlMappingEngineFactory
    {
        /// <summary>
        /// Finds an <see cref="IXmlMappingEngine"/> for the specified version.
        /// </summary>
        /// <param name="version">Version of mapping engine to find, typically {Schema}.{Version} e.g. Css.V2_1</param>
        /// <returns></returns>
        /// <exception cref="MappingException">thrown if the versioned engine is not found/configured incorrectly.</exception>
        IXmlMappingEngine Find(string version);

        /// <summary>
        /// Finds an <see cref="IXmlMappingEngine"/> for the specified version.
        /// </summary>
        /// <param name="version">Version of mapping engine to find, typically {Schema}.{Version} e.g. Css.V2_1</param>
        /// <param name="engine">Engine instance if found, null otherwise</param>
        /// <returns>true if found, false otherwise.</returns>
        bool TryFind(string version, out IXmlMappingEngine engine);
    }
}