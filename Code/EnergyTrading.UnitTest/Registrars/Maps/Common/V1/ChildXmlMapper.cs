namespace EnergyTrading.UnitTest.Registrars.Maps.Common.V1
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class ChildXmlMapper : XmlMapper<Child>
    {
        public ChildXmlMapper() : base("Child")
        {
            this.Namespace = XmlNamespaces.AppNamespace;
            this.NamespacePrefix = XmlNamespaces.AppNamespacePrefix;            
        }

        public override void Map(XPathProcessor source, Child destination)
        {
            this.RegisterNamespace(source, XmlNamespaces.SalesNamespacePrefix, XmlNamespaces.SalesNamespace);

            destination.Id = source.ToInt("Id");
            destination.Value = source.ToFloat("Value", XmlNamespaces.SalesNamespacePrefix);
            destination.Start = source.ToDateTime("Start");
        }

        public override void Map(Child source, XElement destination)
        {
            destination.Add(
                this.XElement("Id", source.Id),
                this.XElement("Value", source.Value),
                this.XElement("Start", source.Start));
        }
    }
}
