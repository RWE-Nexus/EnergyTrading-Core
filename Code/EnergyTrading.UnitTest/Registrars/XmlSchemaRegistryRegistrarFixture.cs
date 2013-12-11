namespace EnergyTrading.UnitTest.Registrars
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Mapping;
    using EnergyTrading.Registrars;

    [TestClass]
    public class XmlSchemaRegistryRegistrarFixture : Fixture
    {
        [TestMethod]
        public void CanResolve()
        {
            new XmlSchemaRegistryRegistrar().Register(Container);

            var candidate = Container.TryResolve<IXmlSchemaRegistry>();
            Assert.IsNotNull(candidate);
        }
    }
}