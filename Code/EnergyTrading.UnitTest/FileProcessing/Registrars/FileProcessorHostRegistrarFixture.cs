namespace EnergyTrading.UnitTest.FileProcessing.Registrars
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.FileProcessing;
    using EnergyTrading.FileProcessing.Registrars;
    using EnergyTrading.Services;

    [TestClass]
    public class FileProcessorHostRegistrarFixture
    {
        [TestMethod]
        public void CanResolve()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ITypeResolver>(new TypeResolver());
            new FileProcessorHostRegistrar().Register(container);

            container.Resolve<IFileProcessorHost>();
        }

        [TestMethod]
        public void ShouldResolveSambaStyleProcessor()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ITypeResolver>(new TypeResolver());
            var configurator = new FileProcessorHostRegistrar { SectionName = "sambaFileProcessorHost" };
            configurator.Register(container);

            container.Resolve<IFileProcessorHost>();
        }
    }
}