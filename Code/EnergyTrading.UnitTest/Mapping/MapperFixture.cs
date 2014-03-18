namespace EnergyTrading.UnitTest.Mapping
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    using NUnit.Framework;

    using Moq;

    [TestFixture]
    public class MapperFixture : Fixture
    {
        [Test]
        public void NoMappingEngineReturnsNullMappingEngine()
        {
            var mapper = new AnimalAnimalModelMapper();
            var candidate = mapper.GetEngine();

            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is NullXmlMappingEngine);
        }

        [Test]
        public void MappingEngineReturned()
        {
            var engine = new Mock<IMappingEngine>();
            var mapper = new AnimalAnimalModelMapper(engine.Object);
            var candidate = mapper.GetEngine();

            Assert.AreSame(engine.Object, candidate);
        }

        [Test]
        public void MappingNullReturnsDefault()
        {
            var mapper = new AnimalAnimalModelMapper();

            var candidate = mapper.Map(null);

            Assert.IsNull(candidate);
        }

        [Test]
        public void MappingValue()
        {
            var mapper = new AnimalAnimalModelMapper();

            var source = new Animal { Id = 1, Name = "Test" };

            var expected = new AnimalModel { Id = 1, Name = "Test" };

            var candidate = mapper.Map(source);

            this.Check(expected, candidate);
        }
    }
}