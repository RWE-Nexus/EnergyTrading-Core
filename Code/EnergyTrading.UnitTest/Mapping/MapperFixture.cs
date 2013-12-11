namespace EnergyTrading.UnitTest.Mapping
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    [TestClass]
    public class MapperFixture : Fixture
    {
        [TestMethod]
        public void NoMappingEngineReturnsNullMappingEngine()
        {
            var mapper = new AnimalAnimalModelMapper();
            var candidate = mapper.GetEngine();

            Assert.IsNotNull(candidate);
            Assert.IsTrue(candidate is NullXmlMappingEngine);
        }

        [TestMethod]
        public void MappingEngineReturned()
        {
            var engine = new Mock<IMappingEngine>();
            var mapper = new AnimalAnimalModelMapper(engine.Object);
            var candidate = mapper.GetEngine();

            Assert.AreSame(engine.Object, candidate);
        }

        [TestMethod]
        public void MappingNullReturnsDefault()
        {
            var mapper = new AnimalAnimalModelMapper();

            var candidate = mapper.Map(null);

            Assert.IsNull(candidate);
        }

        [TestMethod]
        public void MappingValue()
        {
            var mapper = new AnimalAnimalModelMapper();

            var source = new Animal { Id = 1, Name = "Test" };

            var expected = new AnimalModel { Id = 1, Name = "Test" };

            var candidate = mapper.Map(source);

            Check(expected, candidate);
        }
    }
}