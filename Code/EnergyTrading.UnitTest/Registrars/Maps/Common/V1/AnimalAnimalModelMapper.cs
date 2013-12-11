namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V1
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class AnimalAnimalModelMapper : Mapper<Animal, AnimalModel>
    {
        public override void Map(Animal source, AnimalModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }
    }
}