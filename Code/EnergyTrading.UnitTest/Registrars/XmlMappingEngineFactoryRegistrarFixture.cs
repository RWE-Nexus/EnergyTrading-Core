namespace EnergyTrading.UnitTest.Registrars
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;

    [TestClass]
    public class XmlMappingEngineFactoryRegistrarFixture : Fixture
    {
        [TestMethod]
        public void CanResolve()
        {
            new XmlSchemaRegistryRegistrar().Register(Container);
            new XmlMappingEngineFactoryRegistrar().Register(Container);

            var candidate = Container.TryResolve<IXmlMappingEngineFactory>();
            Assert.IsNotNull(candidate);
        }
    }
}