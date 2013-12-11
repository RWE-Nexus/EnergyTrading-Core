namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class ParentXmlMapper : XmlMapper<Parent>
    {
        public ParentXmlMapper(IXmlMappingEngine engine) : base(engine)
        {
        }

        protected override string NodeName
        {
            get { return "Parent"; }
        }

        protected override string Namespace
        {
            get { return XmlNamespaces.CommonNamespace; }
        }

        protected override string NamespacePrefix
        {
            get { return XmlNamespaces.CommonNamespacePrefix; }
        }

        public override void Map(XPathProcessor source, Parent destination)
        {
            destination.Id = source.ToInt("Id");
            destination.Name = source.ToString("Name");
            destination.Cost = source.ToDecimal("Cost");
            destination.Children = this.Engine.MapList<XPathProcessor, Child>(source, "Children");
        }

        public override void Map(Parent source, XElement destination)
        {
            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Name", source.Name),
                this.XElement("Cost", source.Cost),
                this.Engine.MapList<Child, XElement>(source.Children, "Children"));
        }
    }
}