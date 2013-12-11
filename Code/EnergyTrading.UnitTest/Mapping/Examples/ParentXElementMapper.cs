namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class ParentXElementMapper : XElementMapper<Parent>
    {
        private readonly IXmlMappingEngine engine;

        public ParentXElementMapper(IXmlMappingEngine engine) : base("Parent")
        {
            this.engine = engine;

            this.Namespace = XmlNamespaces.CommonNamespace;
        }

        public override void Map(Parent source, XElement destination)
        {
            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Name", source.Name),
                this.XElement("Cost", source.Cost),
                this.engine.MapList<Child, XElement>(source.Children, "Children"));
        }
    }
}