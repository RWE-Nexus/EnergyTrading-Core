namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;

    [TestFixture]
    public class ContainerFixture
    {
        [Test]
        public void ContainerResolvesSelf()
        {
            var container = new UnityContainer();
            var candidate = container.Resolve<IUnityContainer>();

            Assert.AreSame(container, candidate);
        }

        [Test]
        public void ContainerResolveSelfAfterStandardConfiguration()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            var candidate = container.Resolve<IUnityContainer>();

            Assert.AreSame(container, candidate); 
        }

        [Test]
        public void ContainerResolvesServiceLocator()
        {
            var container = new UnityContainer();
            // Deliberately discard result - should still exist in container
            new EnergyTradingUnityServiceLocator(container);

            var candidate = container.Resolve<IServiceLocator>();
        }

        [Test]
        public void ContainerResolvesServiceLocatorAfterStandardConfiguration()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            var candidate = container.Resolve<IServiceLocator>();
        }

        [Test]
        public void ContainerResolvesServiceLocatorAfterStandardConfigurationAndInstallCoreExtensions()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();

            container.InstallCoreExtensions();

            var candidate = container.Resolve<IServiceLocator>();
        }
    }
}