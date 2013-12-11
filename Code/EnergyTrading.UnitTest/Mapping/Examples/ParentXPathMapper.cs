namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;

    public class ParentXPathMapper : XPathMapper<Parent>
    {
        private readonly IXmlMappingEngine engine;

        public ParentXPathMapper(IXmlMappingEngine engine)
        {
            this.engine = engine;
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
            destination.Children = this.engine.MapList<XPathProcessor, Child>(source, "Children");
        }
    }
}