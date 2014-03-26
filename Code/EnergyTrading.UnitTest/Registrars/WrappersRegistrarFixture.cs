namespace EnergyTrading.UnitTest.Registrars
{
    using EnergyTrading.Wrappers;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Registrars;

    [TestFixture]
    public class WrappersRegistrarFixture
    {
        [Test]
        public void CanResolve()
        {
            var container = new UnityContainer();
            new WrappersRegistrar().Register(container);
            Assert.IsInstanceOf<DirectoryWrapper>(container.Resolve<IDirectory>());
            Assert.IsInstanceOf<FileWrapper>(container.Resolve<IFile>());
            Assert.IsInstanceOf<DateTimeWrapper>(container.Resolve<IDateTime>());
        }         
    }
}