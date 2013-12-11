namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;

    public class ChildXmlMapper : XmlMapper<Child>
    {
        public ChildXmlMapper(IXmlMappingEngine engine) : base("Child", engine)
        {
            // this.RegisterNamespace(XmlMappingEngineFixture.SalesNamespace, XmlMappingEngineFixture.SalesNamespacePrefix);
            this.Namespace = XmlNamespaces.AppNamespace;
            this.NamespacePrefix = XmlNamespaces.AppNamespacePrefix;

            this.InitializeMap(x => x.Id);
            this.InitializeMap(x => x.Value, xmlNamespace: XmlNamespaces.SalesNamespace);
            this.InitializeMap(x => x.Start);
        }

        //protected override string NodeName
        //{
        //    get { return "Child"; }
        //}

        //protected override string Namespace
        //{
        //    get { return XmlMappingEngineFixture.AppNamespace; }
        //}

        //protected override string NamespacePrefix
        //{
        //    get { return XmlMappingEngineFixture.AppNamespacePrefix; }
        //}

        public override void Map(XPathProcessor source, Child destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.SalesNamespacePrefix, XmlNamespaces.SalesNamespace);

            destination.Id = source.ToInt("Id");
            destination.Value = source.ToFloat("Value", XmlNamespaces.SalesNamespacePrefix);
            destination.Start = source.ToDateTime("Start", XmlNamespaces.AppNamespacePrefix);
            destination.Dog = this.Engine.Map<Dog>(source, "Animal", XmlNamespaces.AppNamespacePrefix);
        }

        public override void Map(Child source, XElement destination)
        {
            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Value", source.Value),
                this.XElement("Start", source.Start),
                this.Engine.Map<Dog, XElement>(source.Dog));
        }
    }
}