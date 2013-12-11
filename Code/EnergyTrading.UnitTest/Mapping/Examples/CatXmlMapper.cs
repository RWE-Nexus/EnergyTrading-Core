namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class CatXmlMapper : XmlMapper<Cat>
    {
        public CatXmlMapper(IXmlMappingEngine engine) : base("Animal", engine)
        {
            this.Namespace = XmlNamespaces.PetNamespace;
            this.NamespacePrefix = XmlNamespaces.PetNamespacePrefix;

            this.InitializeXmlType(XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace, "Cat");

            // InitializeParent<Animal>();
            this.InitializeMap(x => x.Spayed);
        }

        public override void Map(XPathProcessor source, Cat destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace);

            this.Engine.Map(source, destination as Animal);
            destination.Spayed = source.ToBool("Spayed");
        }

        public override void Map(Cat source, XElement destination)
        {
            this.Engine.Map<Animal, XElement>(source, destination);

            destination.Add(
                this.XElement("Spayed", source.Spayed));
        }
    }
}