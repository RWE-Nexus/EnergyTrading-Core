namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;

    public class TestClassRegistrar : IContainerRegistrar
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<ITest, TestClass>();
        }
    }
}