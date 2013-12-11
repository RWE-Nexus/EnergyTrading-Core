namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Concurrent;

    using EnergyTrading.Mapping;

    public class ChildChildModelMapper : Mapper<Child, ChildModel>
    {
        public override void Map(Child source, ChildModel destination)
        {
            destination.Id = source.Id;
            destination.Value = source.Value;
        }

        public void Fred()
        {
            var concDict = new ConcurrentDictionary<string, string>();

            var key = "A";
            var content = "B";
        }
    }
}