namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Extension methods for ASM version strings, handling conversions for namespace values and <see cref="Version" /> objects.
    /// </summary>
    public static class SchemaVersionExtensions
    {
        /// <summary>
        /// Convert an ASM version string into a <see cref="SchemaVersion" />
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static SchemaVersion ToSchemaVersion(this string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return null;
            }

            var parts = version.Split('.');
            var schemaVersion = new SchemaVersion();
            switch (parts.GetUpperBound(0))
            {
                case 0:
                    schemaVersion.Schema = null;
                    schemaVersion.Version = parts[0].ToVersion();
                    break;

                case 1:
                    schemaVersion.Schema = parts[0];
                    schemaVersion.Version = parts[1].ToVersion();
                    break;

                default:
                    throw new MappingException("Cannot parse schema version: " + version);
            }

            return schemaVersion;
        }

        /// <summary>
        /// Locate the schema from a fully qualified ASM schema string.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string ToAsmSchema(this string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return null;
            }

            var parts = version.Split('.');
            var candidate = parts[0];

            // Assume well-formed first part, otherwise, we can't analyse well enough
            return parts.GetLength(0) > 1 ? candidate : null;
        }

        /// <summary>
        /// Convert a version to a schema qualified version string.
        /// </summary>
        /// <param name="version">Version to use.</param>
        /// <param name="schema">Schema to use</param>
        /// <returns>Schema-qualified version string if valid, otherwise null.</returns>
        public static string ToAsmVersion(this string version, string schema)
        {
            if (string.IsNullOrEmpty(version))
            {
                return null;
            }

            return !string.IsNullOrEmpty(schema) && version.StartsWith(schema)
                ? version
                : version.ToVersion().ToAsmVersion(schema);
        }

        /// <summary>
        /// Convert a version to a schema qualified version string.
        /// </summary>
        /// <param name="version">Version to use.</param>
        /// <param name="schema">Schema to use</param>
        /// <returns>Schema-qualified version string if valid, otherwise null.</returns>
        public static string ToAsmVersion(this Version version, string schema = null)
        {
            if (version == null)
            {
                return null;
            }

            if (schema != null && !schema.EndsWith("."))
            {
                schema += ".";
            }

            return schema + "V" + version.Major + (version.Minor > 0 ? ("_" + version.Minor) : string.Empty);
        }

        /// <summary>
        /// Convert a version to a schema qualified version string.
        /// </summary>
        /// <param name="version">Version to use.</param>
        public static string ToAsmVersion(this SchemaVersion version)
        {
            return version == null ? null : version.Version.ToAsmVersion(version.Schema);
        }

        /// <summary>
        /// Convert a version object into a ASM version.
        /// </summary>
        /// <param name="version">Version to convert</param>
        /// <param name="schema">Optional schema prefix for version</param>
        /// <returns></returns>
        [Obsolete("Use ToAsmVersion")]
        public static string ToAsmVersionString(this Version version, string schema = null)
        {
            return version.ToAsmVersion(schema);
        }

        /// <summary>
        /// Convert a version string to a <see cref="Version" /> object.
        /// </summary>
        /// <param name="value">String to use</param>
        /// <returns>Version object if valid, otherwise null</returns>
        public static Version ToVersion(this string value)
        {
            Version version;
            return Version.TryParse(value.ToVersionString(), out version) ? version : null;
        }

        /// <summary>
        /// Convert an ASM version string into a string parsable by <see cref="Version" />
        /// </summary>
        /// <param name="value">String to use</param>
        /// <returns>Converted string.</returns>
        public static string ToVersionString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var index = char.IsDigit(value[0]) ? 0 : 1;
            var ret = value.Substring(index).Replace("_", ".");
            if (!ret.Contains("."))
            {
                ret += ".0";
            }
            return ret;
        }
    }
}