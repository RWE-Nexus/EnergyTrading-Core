namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NamespaceManagerFixture : BaseNamespaceManagerFixture
    {
        protected override INamespaceManager CreateManager()
        {
            return new NamespaceManager();
        }
    }
}