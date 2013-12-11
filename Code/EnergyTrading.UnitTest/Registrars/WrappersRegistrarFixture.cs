namespace EnergyTrading.UnitTest.Registrars
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Registrars;
    using EnergyTrading.Wrappers;

    [TestClass]
    public class WrappersRegistrarFixture
    {
        [TestMethod]
        public void CanResolve()
        {
            var container = new UnityContainer();
            new WrappersRegistrar().Register(container);
            Assert.IsInstanceOfType(container.Resolve<IDirectory>(), typeof(DirectoryWrapper));
            Assert.IsInstanceOfType(container.Resolve<IFile>(), typeof(FileWrapper));
            Assert.IsInstanceOfType(container.Resolve<IDateTime>(), typeof(DateTimeWrapper));
        }         
    }
}