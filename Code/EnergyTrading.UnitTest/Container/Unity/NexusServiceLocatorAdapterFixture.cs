namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;

    /// <summary>
    /// Summary description for UnityServiceLocatorAdapterFixture
    /// </summary>
    [TestFixture]
    public class EnergyTradingServiceLocatorAdapterFixture : EnergyTradingServiceLocatorFixture
    {
        protected override Microsoft.Practices.ServiceLocation.IServiceLocator CreateServiceLocator()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<ITest, TestClass>()
                .RegisterType<ITest, AnotherTestClass>(typeof(AnotherTestClass).FullName)
                .RegisterType<ITest, TestClass>(typeof(TestClass).FullName);

            return new EnergyTradingUnityServiceLocator(container);
        }

        [SetUp]
        public void Setup()
        {
            this.Locator = this.CreateServiceLocator();
        }

        [Test]
        public new void GetInstance()
        {
            base.GetInstance();
        }

        [Test]
        public new void AskingForInvalidComponentShouldRaiseActivationException()
        {
            base.AskingForInvalidComponentShouldRaiseActivationException();
        }

        [Test]
        public new void GetNamedInstance()
        {
            base.GetNamedInstance();
        }

        [Test]
        public new void GetNamedInstance2()
        {
            base.GetNamedInstance2();
        }

        [Test]
        public new void GetUnknownInstance2()
        {
            base.GetUnknownInstance2();
        }

        [Test]
        public new void GetAllInstances()
        {
            base.GetAllInstances();
        }

        [Test]
        public new void GetAllInstance_ForUnknownType_ReturnEmptyEnumerable()
        {
            base.GetAllInstance_ForUnknownType_ReturnEmptyEnumerable();
        }

        [Test]
        public new void GenericOverload_GetInstance()
        {
            base.GenericOverload_GetInstance();
        }

        [Test]
        public new void GenericOverload_GetInstance_WithName()
        {
            base.GenericOverload_GetInstance_WithName();
        }

        [Test]
        public new void Overload_GetInstance_NoName_And_NullName()
        {
            base.Overload_GetInstance_NoName_And_NullName();
        }

        [Test]
        public new void GenericOverload_GetAllInstances()
        {
            base.GenericOverload_GetAllInstances();
        }

        [Test]
        public void Get_WithZeroLenName_ReturnsDefaultInstance()
        {
            Assert.AreSame(
                this.Locator.GetInstance<ITest>().GetType(),
                this.Locator.GetInstance<ITest>(string.Empty).GetType());
        }
    }
}