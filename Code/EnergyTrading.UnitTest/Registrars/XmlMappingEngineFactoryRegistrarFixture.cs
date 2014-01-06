namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Mapping;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Registrars;

    [TestClass]
    public class XmlMappingEngineFactoryRegistrarFixture : Fixture
    {
        [TestMethod]
        public void CanResolve()
        {
            new XmlSchemaRegistryRegistrar().Register(this.Container);
            new XmlMappingEngineFactoryRegistrar().Register(this.Container);

            var candidate = this.Container.TryResolve<IXmlMappingEngineFactory>();
            Assert.IsNotNull(candidate);
        }
    }
}