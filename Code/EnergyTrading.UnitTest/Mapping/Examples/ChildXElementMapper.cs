namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class ChildXElementMapper : XElementMapper<Child>
    {
        public ChildXElementMapper(IXmlMappingEngine engine) : base("Child")
        {
            this.Engine = engine;

            this.Namespace = XmlNamespaces.AppNamespace;
        }

        private IXmlMappingEngine Engine { get; set; }

        public override void Map(Child source, XElement destination)
        {
            //this.RegisterNamespace(source, XmlNamespaces.SalesNamespacePrefix, XmlNamespaces.SalesNamespace);

            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Value", source.Value, XmlNamespaces.SalesNamespace),
                this.XElement("Start", source.Start),
                this.Engine.Map<Dog, XElement>(source.Dog));
        }
    }
}