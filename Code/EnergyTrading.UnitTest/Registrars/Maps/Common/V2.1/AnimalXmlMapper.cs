namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V2_1
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    /// <summary>
    /// Override behavior for the V2 mapper
    /// </summary>
    public class AnimalXmlMapper : V1.AnimalXmlMapper
    {
        public override void Map(XPathProcessor source, Animal destination)
        {
            destination.Id = source.ToInt("Id2");
            destination.Name = source.ToString("Name2");
        }

        public override void Map(Animal source, System.Xml.Linq.XElement destination)
        {
            destination.Add(
                XElement("Id2", source.Id),
                XElement("Name2", source.Name));
        }
    }
}