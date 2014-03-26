namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using Microsoft.Practices.ServiceLocation;
    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class SimpleMappingEngineFixture : Fixture
    {
        [Test]
        public void RegisteredMapperIsReturned()
        {
            var locator = new Mock<IServiceLocator>(MockBehavior.Strict);
            var engine = new SimpleMappingEngine(locator.Object);

            engine.RegisterMap(new AnimalAnimalModelMapper());

            var candidate = engine.Mapper<Animal, AnimalModel>();

            Assert.IsNotNull(candidate);
        }

        [Test]
        public void LocatorCalledMultipleWhenNoCache()
        {
            var locator = new Mock<IServiceLocator>(MockBehavior.Strict);
            var engine = new SimpleMappingEngine(locator.Object) { CacheMappers = false };

            var expected = new AnimalAnimalModelMapper();
            var type = typeof(IMapper<Animal, AnimalModel>);
            locator.Setup(x => x.GetInstance(type)).Returns(expected);

            var candidate = engine.Mapper<Animal, AnimalModel>();
            candidate = engine.Mapper<Animal, AnimalModel>();

            Assert.IsNotNull(candidate);
            locator.Verify(x => x.GetInstance(type), Times.Exactly(2)); 
        }

        [Test]
        public void LocatorCalledOnceWhenCaching()
        {
            var locator = new Mock<IServiceLocator>(MockBehavior.Strict);
            var engine = new SimpleMappingEngine(locator.Object) { CacheMappers = true };

            var expected = new AnimalAnimalModelMapper();
            var type = typeof(IMapper<Animal, AnimalModel>);
            locator.Setup(x => x.GetInstance(type)).Returns(expected);

            var candidate = engine.Mapper<Animal, AnimalModel>();
            candidate = engine.Mapper<Animal, AnimalModel>();

            Assert.IsNotNull(candidate);
            locator.Verify(x => x.GetInstance(type), Times.Once());
        }

        [Test]
        public void MapCreatesDestination()
        {
            var locator = new Mock<IServiceLocator>(MockBehavior.Strict);
            var engine = new SimpleMappingEngine(locator.Object);

            engine.RegisterMap(new AnimalAnimalModelMapper());

            var source = new Animal { Id = 1, Name = "Test" };
            var expected = new AnimalModel { Id = 1, Name = "Test" };            

            var candidate = engine.Map<Animal, AnimalModel>(source);

            this.Check(expected, candidate);
        }

        [Test]
        public void MapWithCreatedDestination()
        {
            var locator = new Mock<IServiceLocator>(MockBehavior.Strict);
            var engine = new SimpleMappingEngine(locator.Object);

            engine.RegisterMap(new AnimalAnimalModelMapper());

            var source = new Animal { Id = 1, Name = "Test" };
            var expected = new AnimalModel { Id = 1, Name = "Test" };
            var candidate = new AnimalModel();

            engine.Map(source, candidate);

            this.Check(expected, candidate);
        }
    }
}