namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Hold schema and version
    /// </summary>
    public class SchemaVersion
    {
        /// <summary>
        /// Get or set the schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Get or set the version.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Displayable value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToAsmVersion();
        }
    }
}