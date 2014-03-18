namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.Registrars;

    [TestFixture]
    public class XmlSchemaRegistryRegistrarFixture : Fixture
    {
        [Test]
        public void CanResolve()
        {
            new XmlSchemaRegistryRegistrar().Register(this.Container);

            var candidate = this.Container.TryResolve<IXmlSchemaRegistry>();
            Assert.IsNotNull(candidate);
        }
    }
}