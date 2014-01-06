namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V1
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class AnimalXmlMapper : XmlMapper<Animal>
    {
        public AnimalXmlMapper() : base("Animal")
        {
            this.Namespace = XmlNamespaces.AppNamespace;
            this.NamespacePrefix = XmlNamespaces.AppNamespacePrefix;
        }

        public override void Map(XPathProcessor source, Animal destination)
        {
            destination.Id = source.ToInt("Id");
            destination.Name = source.ToString("Name");
        }

        public override void Map(Animal source, System.Xml.Linq.XElement destination)
        {
            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Name", source.Name));
        }
    }
}
