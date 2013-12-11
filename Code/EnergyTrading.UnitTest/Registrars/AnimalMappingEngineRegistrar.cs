namespace EnergyTrading.UnitTest.Registrars
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;

    public class AnimalMappingEngineRegistrar : VersionedMappingEngineRegistrar
    {
        protected override Type MapperType
        {
            get { return typeof(IMapper<,>); }
        }

        public static readonly List<MapperArea> V1 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 1 }
        };

        public static readonly List<MapperArea> V2 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 2 }
        };

        public static readonly List<MapperArea> V2R1 = new List<MapperArea>
        {
            new MapperArea(),
            new MapperArea { Area = "Common", Version = 2, AllowedMinorVersions = { 2.1 } }
        };

        public void Register(IUnityContainer container)
        {
            RegisterVersionedEngine(container, new Version(1, 0), V1);
            RegisterVersionedEngine(container, new Version(2, 0), V2);
            RegisterVersionedEngine(container, new Version(2, 1), V2R1);
        }

        protected override bool IsVersionedMapper(Type type, string area, double version)
        {
            var ns = "EnergyTrading.UnitTest.Registrars.Maps" + ToVersionString(area, version);

            return type.Namespace == ns;
        }
    }
}