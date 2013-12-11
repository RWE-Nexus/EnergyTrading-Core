namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;

    [TestClass]
    public class ContainerFixture
    {
        [TestMethod]
        public void ContainerResolvesSelf()
        {
            var container = new UnityContainer();
            var candidate = container.Resolve<IUnityContainer>();

            Assert.AreSame(container, candidate);
        }

        [TestMethod]
        public void ContainerResolveSelfAfterStandardConfiguration()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            var candidate = container.Resolve<IUnityContainer>();

            Assert.AreSame(container, candidate); 
        }

        [TestMethod]
        public void ContainerResolvesServiceLocator()
        {
            var container = new UnityContainer();
            // Deliberately discard result - should still exist in container
            new EnergyTradingUnityServiceLocator(container);

            var candidate = container.Resolve<IServiceLocator>();
        }

        [TestMethod]
        public void ContainerResolvesServiceLocatorAfterStandardConfiguration()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            var candidate = container.Resolve<IServiceLocator>();
        }

        [TestMethod]
        public void ContainerResolvesServiceLocatorAfterStandardConfigurationAndInstallCoreExtensions()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            container.InstallCoreExtensions();

            var candidate = container.Resolve<IServiceLocator>();
        }
    }
}