using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnergyTrading.Container.Unity;
using EnergyTrading.FileProcessing.Configuration;
using EnergyTrading.FileProcessing.Registrars;
using EnergyTrading.Services;

namespace EnergyTrading.UnitTest.FileProcessing.Registrars
{
    [TestClass]
    public class FileProcessorHostConfiguratorFixture
    {
        [TestMethod]
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