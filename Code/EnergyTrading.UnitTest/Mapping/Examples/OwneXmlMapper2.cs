namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class OwnerXmlMapper2 : XmlMapper<Owner>
    {
        public OwnerXmlMapper2(IXmlMappingEngine engine) : base(engine)
        {
            this.InitializeMap(x => x.Name);
            this.InitializeMap(x => x.Pets, "Pets");
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
            destination.Name = source.ToString("Name");
            destination.Pets = this.Engine.MapList<XPathProcessor, Animal>(source, "Pets", string.Empty, collectionItemNodeNamespacePrefix: this.NamespacePrefix);
        }

        public override void Map(Owner source, XElement destination)
        {
            destination.Add(
                this.XElement("Name", source.Name),
                this.Engine.MapList<Animal, XElement>(source.Pets, "Pets"));
        }
    }
}