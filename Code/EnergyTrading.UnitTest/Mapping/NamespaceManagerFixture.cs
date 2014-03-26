namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class NamespaceManagerFixture : BaseNamespaceManagerFixture
    {
        protected override INamespaceManager CreateManager()
        {
            return new NamespaceManager();
        }
    }
}