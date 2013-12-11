namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Extension of <see cref="IXmlVersionDetector" /> to handle detection based on the metadata.
    /// </summary>
    /// <typeparam name="TMetaData">Type of the metadata.</typeparam>
    public interface IMetadataXmlVersionDetector<in TMetaData> : IXmlVersionDetector
    {
        /// <summary>
        /// Determine the schema version of a value located via metadata.
        /// </summary>
        /// <param name="metadata">Metadata to use.</param>
        /// <param name="contentKey">Key to locate the content via the metadata.</param>
        /// <returns>Schema version of the supplied XML, typically {Schema}.{Version} e.g. Css.2_1 or <see cref="string.Empty"/> if not recognised</returns>
        string DetermineContentVersion(TMetaData metadata, string contentKey);
    }
}
