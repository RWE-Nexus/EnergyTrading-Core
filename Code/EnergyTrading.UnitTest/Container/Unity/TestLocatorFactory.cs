namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.ServiceLocation;

    public class TestLocatorFactory : ITestFactory
    {
        private readonly IServiceLocator locator;

        public TestLocatorFactory(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public ITest Get()
        {
            return locator.GetInstance<ITest>();
        }
    }
}