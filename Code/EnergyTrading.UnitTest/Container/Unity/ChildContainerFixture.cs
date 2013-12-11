namespace EnergyTrading.UnitTest.Container.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.Container.Unity;

    [TestClass]
    public class ChildContainerFixture : Fixture
    {
        [TestMethod]
        public void ChildContainerResolvesSelf()
        {
            var container = new UnityContainer();

            var child = container.CreateChildContainer();

            var candidate = child.Resolve<IUnityContainer>();

            Assert.AreSame(child, candidate);
        }

        [TestMethod]
        public void ChildContainerResolvesInParentDefault()
        {
            var container = new UnityContainer();

            var child = container.CreateChildContainer();

            container.RegisterInstance("test", child);

            var candidate = container.Resolve<IUnityContainer>("test");

            Assert.AreSame(child, candidate);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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