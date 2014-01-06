namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;

    public class OrderXmlMapper : XmlMapper<Order>
    {
        public OrderXmlMapper(IXmlMappingEngine engine)
            : base("order", engine)
        {            
        }

        protected override string Namespace
        {
            get { return XmlNamespaces.SalesNamespace; }
        }

        protected override string NamespacePrefix
        {
            get { return XmlNamespaces.SalesNamespacePrefix; }
        }

        public override void Map(XPathProcessor source, Order destination)
        {
            destination.Ids = this.Engine.MapList<Id>(source, "ids");
        }

        public override void Map(Order source, XElement destination)
        {
            destination.Add(this.Engine.MapList<Id, XElement>(source.Ids, "ids"));
        }
    }
}