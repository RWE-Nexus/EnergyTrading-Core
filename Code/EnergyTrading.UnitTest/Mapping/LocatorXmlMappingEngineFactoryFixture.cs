namespace EnergyTrading.UnitTest.Mapping
{
    using System;

    using EnergyTrading.Mapping;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class LocatorXmlMappingEngineFactoryFixture : Fixture
    {
        [Test]
        public void FindSuccessful()
        {
            var expected = new Mock<IXmlMappingEngine>();
            this.Container.RegisterInstance(typeof(IXmlMappingEngine), "Css.V1", expected.Object, new ContainerControlledLifetimeManager());

            var factory = new LocatorXmlMappingEngineFactory(this.Container.Resolve<IServiceLocator>());
            var candidate = factory.Find("Css.V1");

            Assert.AreSame(expected.Object, candidate);
        }

        [Test]
        public void FindUnsuccessful()
        {
            var factory = new LocatorXmlMappingEngineFactory(this.Container.Resolve<IServiceLocator>());

            try
            {
                factory.Find("Css.V1");

                throw new Exception("Factory did not throw");
            }
            catch (MappingException)
            {                
            }
        }

        [Test]
        public void TryFindSuccessful()
        {
            var expected = new Mock<IXmlMappingEngine>();
            this.Container.RegisterInstance(typeof(IXmlMappingEngine), "Css.V1", expected.Object, new ContainerControlledLifetimeManager());

            var factory = new LocatorXmlMappingEngineFactory(this.Container.Resolve<IServiceLocator>());
            IXmlMappingEngine candidate;
            var result = factory.TryFind("Css.V1", out candidate);

            Assert.IsTrue(result, "Result is false");
            Assert.AreSame(expected.Object, candidate);
        }

        [Test]
        public void TryFindUnsuccessful()
        {
            var factory = new LocatorXmlMappingEngineFactory(this.Container.Resolve<IServiceLocator>());
            IXmlMappingEngine candidate;
            var result = factory.TryFind("Css.V1", out candidate);

            Assert.IsFalse(result, "Result is truel");
            Assert.IsNull(candidate, "Candidate not null");
        }
    }
}