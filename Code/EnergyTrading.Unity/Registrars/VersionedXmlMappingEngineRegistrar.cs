namespace EnergyTrading.Registrars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Schema;

    using EnergyTrading.Logging;
    using EnergyTrading.Mapping;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Base implementation of a mapper registrar for creating versioned <see cref="IXmlMappingEngine" />s.
    /// </summary>
    public abstract class VersionedXmlMappingEngineRegistrar : MappingEngineRegistrar<IXmlMappingEngine>
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or set the schema name for the versions.
        /// </summary>
        protected string SchemaName { get; set; }

        /// <summary>
        /// Gets the base type of the mapper, <see cref="IXmlMapper{T, D}" />
        /// </summary>
        protected override Type MapperType
        {
            get { return typeof(IXmlMapper<,>); }
        }

        /// <inheritdoc />
        protected override IXmlMappingEngine CreateEngine(IUnityContainer container)
        {
            // Get the service locator.
            var locator = container.Resolve<IServiceLocator>();

            IXmlMapperFactory mapperFactory = new LocatorXmlMapperFactory(locator);
            if (CacheMappers)
            {
                // Wrap in the cache provider.
                var lf = mapperFactory;
                mapperFactory = new CachingXmlMapperFactory(lf);
            }
            var engine = new XmlMappingEngine(mapperFactory);

            return engine;
        }

        /// <summary>
        /// Gets the names of the schemas that are in the schema version.
        /// </summary>
        /// <param name="schemaVersion">Schema version to use</param>
        /// <returns>Enumeration of resource names with the specified schema version</returns>
        /// <remarks>We added a trailing '.' to ensure an exact match otherwise {Schema}.V2 would match against {Schema}.V2_2 schemas</remarks>
        protected virtual IEnumerable<string> GetSchemaNames(string schemaVersion)
        {
            return SchemaResourceAssembly.GetManifestResourceNames()
                                         .Where(resourceName => resourceName.Contains(schemaVersion + ".")
                                                             && resourceName.EndsWith(".xsd"));
        }

        /// <inheritdoc />
        protected override IUnityContainer RegisterVersionedEngine(IUnityContainer container, Version version, IEnumerable<MapperArea> areas)
        {
            var versioned = base.RegisterVersionedEngine(container, version, areas);

            // And try to put an associated schema set in as well.
            RegisterSchemaSetVersion(container, version);

            return versioned;
        }

        /// <summary>
        /// Register all the XML schemas associated with a version in a <see cref="XmlSchemaSet" />.
        /// </summary>
        /// <param name="container">Container to use</param>
        /// <param name="version">Version to use</param>
        protected virtual void RegisterSchemaSetVersion(IUnityContainer container, Version version)
        {
            var schemaSet = new XmlSchemaSet();
            var found = false;
            var schemaVersion = ToVersionString(version);

            foreach (var schemaResource in GetSchemaNames(schemaVersion))
            {
                Logger.Debug(schemaVersion + ": Loading " + schemaResource);
                using (var stream = SchemaResourceAssembly.GetManifestResourceStream(schemaResource))
                {
                    var schema = XmlSchema.Read(stream, (sender, args) => { });
                    schemaSet.Add(schema);
                }

                found = true;
            }

            if (!found)
            {
                // Quit if we didn't find anything for this version
                return;
            }

            // Compile it first, checks we have a coherent set of schemas
            try
            {
                schemaSet.Compile();

                // And register it
                container.RegisterInstance(schemaVersion, schemaSet);
            }
            catch (Exception ex)
            {
                Logger.Warn(schemaVersion + ": Schema error", ex);
            }
        }

        /// <inheritdoc />
        protected override void ParentRegister(IUnityContainer container, Version version, IXmlMappingEngine engine)
        {
            container.RegisterInstance(ToVersionString(version), engine);

            // Ok, record the schema as one we are interested in
            if (!string.IsNullOrEmpty(SchemaName))
            {
                SchemaRegistry(container).RegisterSchema(SchemaName);
            }
        }

        private static IXmlSchemaRegistry SchemaRegistry(IUnityContainer container)
        {
            try
            {
                return container.Resolve<IXmlSchemaRegistry>();
            }
            catch (Exception)
            {
                throw new MappingException("Must register IXmlSchemaRegistry");
            }
        }

        /// <inheritdoc />
        protected override string ToVersionString(Version version)
        {
            return version.ToAsmVersion(SchemaName);
        }
    }
}