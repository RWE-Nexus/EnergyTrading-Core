namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using NUnit.Framework;

    using EnergyTrading.Container.Unity;

    [TestFixture]
    public class ChildContainerFixture : Fixture
    {
        [Test]
        public void ChildContainerResolvesSelf()
        {
            var container = new UnityContainer();

            var child = container.CreateChildContainer();

            var candidate = child.Resolve<IUnityContainer>();

            Assert.AreSame(child, candidate);
        }

        [Test]
        public void ChildContainerResolvesInParentDefault()
        {
            var container = new UnityContainer();

            var child = container.CreateChildContainer();

            container.RegisterInstance("test", child);

            var candidate = container.Resolve<IUnityContainer>("test");

            Assert.AreSame(child, candidate);
        }

        [Test]
        public void ChildContainerResolvesInParent()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var expected = container.ConfigureChildContainer("test");

            var candidate = container.Resolve<IUnityContainer>("test");

            Assert.AreSame(expected, candidate);
        }

        [Test]
        public void ChildResolveParentServiceDirectly()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var child = container.ConfigureChildContainer("test");

            var expected = typeof(TestClass);
            var candidate = child.Resolve<ITest>("direct");

            Assert.AreSame(expected, candidate.GetType());
        }

        [Test]
        public void ChildResolveChildServiceDirectly()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var child = container.ConfigureChildContainer("test");

            var expected = typeof(ChildTestClass);
            var candidate = child.Resolve<ITest>("direct2");

            Assert.AreSame(expected, candidate.GetType());  
        }

        [Test]
        public void ChildResolveChildServiceViaRegistrar()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var child = container.ConfigureChildContainer("test");

            var expected = typeof(ChildTestClass);
            var candidate = child.Resolve<ITest>();

            Assert.AreSame(expected, candidate.GetType());
        }

        [Test]
        public void FactoryCalllInParent()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var factory = container.Resolve<ITestFactory>();

            var expected = typeof(TestClass);
            var candidate = factory.Get();

            Assert.AreSame(expected, candidate.GetType());
        }

        [Test]
        public void FactoryCallInChild()
        {
            var container = new UnityContainer();
            container.StandardConfiguration();
            container.LoadConfiguration();
            container.ConfigureRegistrars();

            var child = container.ConfigureChildContainer("test");

            var factory = child.Resolve<ITestFactory>();

            var expected = typeof(ChildTestClass);
            var candidate = factory.Get();

            Assert.AreSame(expected, candidate.GetType());
        }
    }
}