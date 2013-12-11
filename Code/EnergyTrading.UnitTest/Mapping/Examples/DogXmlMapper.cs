namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class DogXmlMapper : XmlMapper<Dog>
    {
        public DogXmlMapper(IXmlMappingEngine engine) : base("Animal", engine)
        {
            this.Namespace = XmlNamespaces.PetNamespace;
            this.NamespacePrefix = XmlNamespaces.PetNamespacePrefix;

            this.InitializeXmlType(XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace, "Dog");
        }

        public override void Map(XPathProcessor source, Dog destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace);

            this.Engine.Map(source, destination as Animal);
            destination.Tricks = source.ToString("Tricks");
        }

        public override void Map(Dog source, XElement destination)
        {
            this.Engine.Map<Animal, XElement>(source, destination);

            destination.Add(
                this.XElement("Tricks", source.Tricks));
        }
    }
}