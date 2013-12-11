namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public class AnimalAnimalModelMapper : Mapper<Animal, AnimalModel>
    {
        public AnimalAnimalModelMapper()
        {            
        }

        public AnimalAnimalModelMapper(IMappingEngine engine) : base(engine)
        {            
        }

        public override void Map(Animal source, AnimalModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }

        public IMappingEngine GetEngine()
        {
            return Engine;
        }
    }
}