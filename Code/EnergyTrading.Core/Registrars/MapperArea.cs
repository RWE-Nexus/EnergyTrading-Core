namespace EnergyTrading.Registrars
{
    using System.Collections.Generic;

    /// <summary>
    /// An area and version of mappers used to control registration of mappers     
    /// </summary>
    public class MapperArea
    {
        public MapperArea()
        {
            AllowedMinorVersions = new List<double>();
        }

        /// <summary>
        /// Gets or sets the Area name property.
        /// <para>
        /// This corresponds to the partial namespace that will be adopted.
        /// </para>
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Gets or sets the major version number that is supported.
        /// <para>
        /// Version numbers are inclusive, i.e. version 2 includes version 1
        /// so that only changed mappers need to be implemented at each new version.
        /// </para>
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the allowed minor versions.
        /// <para>
        /// Normally you would include all versions e.g. if we are on 2.3, the allowed minor
        /// versions would be 2.3, 2.2 and 2.1.
        /// </para>
        /// <para>
        /// However, we sometimes include breaking changes in minor releases, this allows
        /// us to control whether these minor releases are included in a versioned engine.
        /// </para>
        /// </summary>
        public List<double> AllowedMinorVersions { get; set; }
    }
}