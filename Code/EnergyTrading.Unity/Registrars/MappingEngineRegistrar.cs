namespace EnergyTrading.Registrars
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Container.Unity.AutoRegistration;
    using EnergyTrading.Mapping;
    using EnergyTrading.Math;

    /// <summary>
    /// Base registrar for versioned mappings.
    /// </summary>
    public abstract class MappingEngineRegistrar<T>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MappingEngineRegistrar{T}" /> class.
        /// </summary>
        protected MappingEngineRegistrar()
        {
            CacheMappers = true;
        }

        /// <summary>
        /// Gets the assembly to check for schema resources.
        /// </summary>
        /// <remarks>By default the assembly containing the registrar</remarks>
        protected virtual Assembly SchemaResourceAssembly
        {
            get { return Assembly.GetAssembly(GetType()); }
        }

        /// <summary>
        /// Gets the type of mapper we are registering, typically an open type.
        /// </summary>
        protected abstract Type MapperType { get; }

        /// <summary>
        /// Gets or sets whether the mapping engines cache mappers.
        /// </summary>
        /// <remarks>
        /// For bulk operations, e.g. serializing 1K trades, the cost of resolving the 
        /// mappers becomes appreciable so we provide this as a way of caching them in
        /// the mapping engine.
        /// </remarks>
        protected bool CacheMappers { get; set; }

        /// <summary>
        /// Gets a function that determines whether a mapper is the specified version.
        /// <para>
        /// Typically this will verify that the type is in the appropriate namespace.
        /// </para>
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="area">Mapping area to check, typically a partial namespace.</param>
        /// <param name="version">Version of the mapper, can be 0 (unversioned) or a major/minor version e.g. 2.1</param>
        protected abstract bool IsVersionedMapper(Type type, string area, double version);

        /// <summary>
        /// Create the mapping engine but don't register it anywhere.
        /// </summary>
        /// <param name="container">Container for any dependencies.</param>
        /// <returns>A new mapping engine</returns>
        protected abstract T CreateEngine(IUnityContainer container);

        /// <summary>
        /// Uses the areas to register mappers into a self-contained container and creating a new 
        /// instance of a mapping engine which is registered into the parent container.         
        /// </summary>
        /// <param name="container">Parent container to register engine into</param>
        /// <param name="version">Version name to register the mapping engine against</param>
        /// <param name="areas">Mapping areas to register.</param>
        /// <returns>Child container used to register each of the mappers</returns>
        protected virtual IUnityContainer RegisterVersionedEngine(IUnityContainer container, Version version, IEnumerable<MapperArea> areas)
        {
            var versioned = RegisterMappers(areas);

            // Get an mapping factory and engine.
            var engine = CreateEngine(versioned);

            // Self register so the mappers get this engine instance
            versioned.RegisterInstance(engine);

            // Register in the parent container under a version name
            ParentRegister(container, version, engine);

            return versioned;
        }

        /// <summary>
        /// Uses the areas to register mappers into a self-contained container.         
        /// </summary>
        /// <param name="areas">Mapping areas to register.</param>
        protected virtual IUnityContainer RegisterMappers(IEnumerable<MapperArea> areas)
        {
            var versioned = new UnityContainer();
            versioned.StandardConfiguration();

            foreach (var x in areas)
            {
                AutoRegisterMappers(versioned, x.Area, x.Version);
                AutoRegisterMinorVersionMappers(versioned, x.Area, x.AllowedMinorVersions);
            }

            return versioned;
        }

        /// <summary>
        /// Register the engine in a container under a version number.
        /// </summary>
        /// <param name="container">Container to register in.</param>
        /// <param name="version">Version to register under</param>
        /// <param name="engine">Engine to register</param>
        protected abstract void ParentRegister(IUnityContainer container, Version version, T engine);
        
        /// <summary>
        /// Convert an area and version into a version string.
        /// </summary>
        /// <param name="area">Area to register against</param>
        /// <param name="version">Version number e.g. 2, 2.1</param>
        /// <returns>Empty string if the area is empty, a formatted version otherwise e.g. {Area}.V1</returns>
        protected virtual string ToVersionString(string area, double version)
        {
            if (string.IsNullOrEmpty(area))
            {
                return string.Empty;
            }

            var format = version == 0 ? ".{0}" : ".{0}.V{1}";
            return string.Format(format, area, version.ToVersionString());
        }
        
        /// <summary>
        /// Converts a version into a version string.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        protected virtual string ToVersionString(Version version)
        {
            return version.ToAsmVersion();
        }

        private void AutoRegisterMinorVersionMappers(IUnityContainer versioned, string area, IEnumerable<double> allowedMinorVersions)
        {
            foreach (var version in allowedMinorVersions)
            {
                AutoAreaRegisterMappers(versioned, area, version);
            }
        }

        private void AutoRegisterMappers(IUnityContainer versioned, string area, int version)
        {
            // Allow for version 0, i.e. non-versioned areas
            for (var i = 0; i <= version; i++)
            {
                AutoAreaRegisterMappers(versioned, area, i);
            }
        }

        private void AutoAreaRegisterMappers(IUnityContainer versioned, string area, double version)
        {
            versioned.ConfigureAutoRegistration()
                     .Include(
                         x => x.ImplementsOpenGeneric(MapperType) && IsVersionedMapper(x, area, version),
                         Then.Register().AsAllInterfacesOfType())
                     .ApplyAutoRegistration(GetType().Assembly);
        }
    }
}