namespace EnergyTrading.Test
{
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public abstract class SpecBaseAutoMocking<T>
    {
        protected T Sut { get; set; }

        private IUnityContainer container;

        public void AddConcrete<TInterface, TInstance>(TInstance concrete) where TInstance : TInterface
        {
            this.container.RegisterInstance<TInterface>(concrete);
        }

        public TInterface Concrete<TInterface>()
        {
            return this.container.Resolve<TInterface>();
        }

        public Mock<TInterface> Mock<TInterface>() where TInterface : class
        {
            return this.container.ConfigureMockFor<TInterface>();
        }

        public Mock<TInterface> RegisterMock<TInterface>() where TInterface : class
        {
            return this.container.RegisterMock<TInterface>();
        }

        [SetUp]
        public void Setup()
        {
            this.container = new UnityContainer().AddNewExtension<AutoMockingContainerExtension>();
            this.Establish_context();
            this.Because_of();
        }

        protected virtual void Because_of()
        {
        }

        protected virtual void Establish_context()
        {
            this.Sut = this.container.Resolve<T>();
        }
    }
}