namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;

    public class ChildXPathMapper : XPathMapper<Child>
    {
        public ChildXPathMapper(IXmlMappingEngine engine) : base("Child", engine)
        {            
        }
        protected override string Namespace
        {
            get { return XmlNamespaces.AppNamespace; }
        }

        protected override string NamespacePrefix
        {
            get { return XmlNamespaces.AppNamespacePrefix; }
        }

        public override void Map(XPathProcessor source, Child destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.SalesNamespacePrefix, XmlNamespaces.SalesNamespace);

            destination.Id = source.ToInt("Id");
            destination.Value = source.ToFloat("Value", XmlNamespaces.SalesNamespacePrefix);
            destination.Start = source.ToDateTime("Start");
            destination.Dog = this.Engine.Map<Dog>(source, "Animal", XmlNamespaces.AppNamespacePrefix);
        }
    }
}