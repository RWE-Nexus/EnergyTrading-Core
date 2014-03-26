namespace EnergyTrading.UnitTest.FileProcessing.Registrars
{
    using EnergyTrading.Services;

    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;
    using EnergyTrading.FileProcessing.Configuration;
    using EnergyTrading.FileProcessing.Registrars;

    [TestFixture]
    public class FileProcessorHostConfiguratorFixture
    {
        [Test]
        public void ShouldStartFileProcessorHostThatIsPollingBased()
        {
            var configurator = new FileProcessorConfigurator()
                .Register<TestFileHostRegistrar>()
                .ConfigureRegistrars()
                .RunConfigurationTasks();
            var host = configurator.ToProcessorHost();

            host.Stop();
        }

        private class TestFileHostRegistrar : IContainerRegistrar
        {
            public void Register(IUnityContainer container)
            {
                container.RegisterInstance<ITypeResolver>(new TypeResolver());
                var configurator = new FileProcessorHostRegistrar { SectionName = "sambaFileProcessorHost" };
                configurator.Register(container);
            }
        }
    }
}