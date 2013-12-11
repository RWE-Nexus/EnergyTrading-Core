namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V1
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class AnimalXmlMapper : XmlMapper<Animal>
    {
        public AnimalXmlMapper() : base("Animal")
        {
            Namespace = XmlNamespaces.AppNamespace;
            NamespacePrefix = XmlNamespaces.AppNamespacePrefix;
        }

        public override void Map(XPathProcessor source, Animal destination)
        {
            destination.Id = source.ToInt("Id");
            destination.Name = source.ToString("Name");
        }

        public override void Map(Animal source, System.Xml.Linq.XElement destination)
        {
            destination.Add(
                XElement("Id", source.Id),
                XElement("Name", source.Name));
        }
    }
}
