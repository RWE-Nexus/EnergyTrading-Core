namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Locates the most compatible XML mapping engine factory.
    /// </summary>
    public class CompatibleXmlMappingEngineFactory : IXmlMappingEngineFactory
    {
        private readonly IXmlMappingEngineFactory factory;
        private readonly IXmlSchemaRegistry registry;

        public CompatibleXmlMappingEngineFactory(IXmlMappingEngineFactory factory, IXmlSchemaRegistry registry)
        {
            this.factory = factory;
            this.registry = registry;
        }

        /// <summary>
        /// Finds a specified version of an IXmlMappingEngine.
        /// <para>
        /// Will try to locate an earlier minor version if the exact version is not found e.g. can return Css.V2_0 if Css.V2_1 was requested.
        /// </para>
        /// </summary>
        /// <param name="version">Version of mapping engine to find, typically {Schema}.{Version} e.g. Css.V2_1</param>
        /// <returns></returns>
        /// <exception cref="XmlEngineResolutionException">thrown if the versioned engine is not found/configured incorrectly.</exception>
        public IXmlMappingEngine Find(string version)
        {
            try
            {
                var engine = ObtainExactVersion(version) ?? ObtainLowerMinorVersion(version);
                if (engine != null)
                {
                    return engine;
                }
            }
            catch (XmlEngineResolutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ThrowXmlEngineResolutionError(version, ex);
            }

            throw ThrowXmlEngineResolutionError(version);
        }

        /// <summary>
        /// Finds an IXmlMappingEngine for the specified version.
        /// <para>
        /// Will try to locate an earlier minor version if the exact version is not found e.g. can return Css.V2_0 if Css.V2_1 was requested.
        /// </para>        
        /// </summary>
        /// <param name="version">Version of mapping engine to find, typically {Schema}.{Version} e.g. Css.V2_1</param>
        /// <param name="engine">Engine instance if found, null otherwise</param>
        /// <returns>true if found, false otherwise.</returns>
        public bool TryFind(string version, out IXmlMappingEngine engine)
        {
            try
            {
                engine = Find(version);
                return engine != null;
            }
            catch
            {
                engine = null;
                return false;
            }
        }

        private IXmlMappingEngine ObtainExactVersion(string admVersion)
        {
            IXmlMappingEngine engine;
            return this.factory.TryFind(admVersion, out engine) ? engine : null;
        }

        private IXmlMappingEngine ObtainLowerMinorVersion(string asmVersion)
        {
            IXmlMappingEngine engine = null;

            var schemaVersion = asmVersion.ToSchemaVersion();
            while (schemaVersion.Version.Minor > 0 && engine == null)
            {
                schemaVersion.Version = new Version(schemaVersion.Version.Major, schemaVersion.Version.Minor - 1);
                engine = ObtainExactVersion(schemaVersion.ToAsmVersion());
            }
            return engine;
        }

        private Exception ThrowXmlEngineResolutionError(string asmVersion, Exception exception = null)
        {
            var code = XmlEngineResolutionErrorCode.Undetermined;
            if (exception != null)
            {
                return new XmlEngineResolutionException(code, asmVersion, exception);
            }

            var schemaVersion = asmVersion.ToSchemaVersion();

            if (!registry.SchemaExists(schemaVersion.Schema))
            {
                code = XmlEngineResolutionErrorCode.UnexpectedSchema;
            }
            else 
            {
                // Only check the version flags if we have an expected schema
                if (ObtainExactVersion(new Version(schemaVersion.Version.Major - 1, 0).ToAsmVersion(schemaVersion.Schema)) != null)
                {
                    code |= XmlEngineResolutionErrorCode.MessageVersionTooHigh;
                }

                if (ObtainExactVersion(new Version(schemaVersion.Version.Major + 1, 0).ToAsmVersion(schemaVersion.Schema)) != null)
                {
                    code |= XmlEngineResolutionErrorCode.MessageVersionTooLow;
                }
            }

            return new XmlEngineResolutionException(code, asmVersion);
        }
    }
}