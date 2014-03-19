namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class CatXmlMapper : XmlMapper<Cat>
    {
        public CatXmlMapper(IXmlMappingEngine engine) : base("Animal", engine)
        {
            Namespace = XmlNamespaces.PetNamespace;
            NamespacePrefix = XmlNamespaces.PetNamespacePrefix;

            InitializeXmlType(XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace, "Cat");

            // InitializeParent<Animal>();
            InitializeMap(x => x.Spayed);
        }

        public override void Map(XPathProcessor source, Cat destination)
        {
            RegisterNamespace(source, XmlNamespaces.PetNamespacePrefix, XmlNamespaces.PetNamespace);

            Engine.Map(source, destination as Animal);
            destination.Spayed = source.ToBool("Spayed");
        }

        public override void Map(Cat source, XElement destination)
        {
            Engine.Map<Animal, XElement>(source, destination);

            destination.Add(
                XElement("Spayed", source.Spayed));
        }
    }
}