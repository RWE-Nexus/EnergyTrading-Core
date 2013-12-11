namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;

    public class ChildTestClassRegistrar : IContainerRegistrar
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<ITest, ChildTestClass>();
        }
    }
}