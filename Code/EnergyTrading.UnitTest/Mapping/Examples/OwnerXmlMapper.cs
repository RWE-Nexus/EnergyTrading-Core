namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;

    public class OwnerXmlMapper : XmlMapper<Owner>
    {
        public OwnerXmlMapper(IXmlMappingEngine engine) : base(engine)
        {
            InitializeMap(x => x.Name);
            InitializeMap(x => x.Pets, "Pets");
        }

        protected override string NodeName
        {
            get { return "Owner"; }
        }

        protected override string Namespace
        {
            get { return XmlNamespaces.AppNamespace; }
        }

        protected override string NamespacePrefix
        {
            get { return XmlNamespaces.AppNamespacePrefix; }
        }

        public override void Map(XPathProcessor source, Owner destination)
        {
            destination.Id = Engine.Map<Identifier>(source, "Identifier");
            destination.Name = source.ToString("Name");
            destination.Pets = Engine.MapList<XPathProcessor, Animal>(source, "Pets");
        }

        public override void Map(Owner source, XElement destination)
        {
            destination.Add(
                Engine.Map(source.Id, "Identifier"),
                XElement("Name", source.Name),
                Engine.MapList<Animal, XElement>(source.Pets, "Pets"));
        }
    }
}