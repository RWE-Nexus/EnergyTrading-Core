namespace EnergyTrading.UnitTest.FileProcessing.Registrars
{
    using EnergyTrading.FileProcessing;
    using EnergyTrading.Services;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.FileProcessing.Registrars;

    [TestFixture]
    public class FileProcessorHostRegistrarFixture
    {
        [Test]
        public void CanResolve()
        {
            var container = new UnityContainer();
            container.RegisterInstance<ITypeResolver>(new TypeResolver());
            new FileProcessorHostRegistrar().Register(container);

            container.Resolve<IFileProcessorHost>();
        }

        [Test]
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