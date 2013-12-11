namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;

    public class TestFactoryRegistrar : IContainerRegistrar
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<ITestFactory, TestLocatorFactory>();
        }
    }
}