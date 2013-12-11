namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class AnimalXmlMapper : XmlMapper<Animal>
    {
        public AnimalXmlMapper(IXmlMappingEngine engine) : base("Animal", engine)
        {
            this.Namespace = XmlNamespaces.AppNamespace;
            this.NamespacePrefix = XmlNamespaces.AppNamespacePrefix;

            this.InitializeXmlType(string.Empty, XmlNamespaces.AppNamespace, "Animal");

            this.Engine.RegisterXmlType(XmlNamespaces.PetNamespace, "Dog", typeof(Dog));
            this.Engine.RegisterXmlType(XmlNamespaces.PetNamespace, "Cat", typeof(Cat));

            // NB Due to children potentially being in different namespace, properties must specify prefix here           
            this.InitializeMap(x => x.Id, xmlNamespace: XmlNamespaces.AppNamespace);
            this.InitializeMap(x => x.Name, xmlNamespace: XmlNamespaces.AppNamespace);
        }

        public override void Map(XPathProcessor source, Animal destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.AppNamespacePrefix, XmlNamespaces.AppNamespace);

            // NB Due to children potentially being in different namespace, properties must specify prefix here
            destination.Id = source.ToInt("Id", XmlNamespaces.AppNamespacePrefix);
            destination.Name = source.ToString("Name", XmlNamespaces.AppNamespacePrefix);
        }

        public override void Map(Animal source, XElement destination)
        {
            // NB Due to children potentially being in different namespace, properties must specify namespace here
            destination.Add(
                this.XElement("Id", source.Id, XmlNamespaces.AppNamespace),
                this.XElement("Name", source.Name, XmlNamespaces.AppNamespace));
        }
    }
}