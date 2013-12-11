namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Error codes when determining versions of <see cref="IXmlMappingEngine" />.
    /// </summary>
    [Flags]
    public enum XmlEngineResolutionErrorCode
    {
        /// <summary>
        /// Error reason is unknown.
        /// </summary>
        /// <remarks>
        /// Could be a parsing error, e.g. the requested schema value is invalid
        /// or that the schema is registered but no specific versions are present.
        /// </remarks>
        Undetermined = 0x0000,

        /// <summary>
        /// Cannot resolve an engine for the schema at all.
        /// </summary>
        UnexpectedSchema = 0x0001,

        /// <summary>
        /// Schema version of the message is too low.
        /// </summary>
        /// <remarks>
        /// For example, we have registered V11 of the schema and we
        /// are asked to resolve a V10.2 engine and no V10 engines are registered.
        /// </remarks>
        MessageVersionTooLow = 0x0002,

        /// <summary>
        /// Schema version of the message is too high.
        /// </summary>
        /// <remarks>
        /// For example, we have registered V10 of the schema and we
        /// are asked to resolve a V11.1 engine and no V11 engines are registered.
        /// </remarks>
        MessageVersionTooHigh = 0x0004
    }
}
