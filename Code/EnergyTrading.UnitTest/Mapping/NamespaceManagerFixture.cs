namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Mapping;

    [TestClass]
    public class NamespaceManagerFixture : BaseNamespaceManagerFixture
    {
        protected override INamespaceManager CreateManager()
        {
            return new NamespaceManager();
        }
    }
}