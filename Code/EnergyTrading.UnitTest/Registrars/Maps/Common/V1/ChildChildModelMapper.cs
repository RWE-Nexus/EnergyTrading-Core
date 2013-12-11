namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V1
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class ChildChildModelMapper : Mapper<Child, ChildModel>
    {
        public override void Map(Child source, ChildModel destination)
        {
            destination.Id = source.Id;
            destination.Value = source.Value;
        }
    }
}