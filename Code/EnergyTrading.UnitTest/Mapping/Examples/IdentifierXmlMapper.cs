namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class IdentifierXmlMapper : XmlMapper<Identifier>
    {
        public IdentifierXmlMapper() : base("identifier")
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

        public override void Map(XPathProcessor source, Identifier destination)
        {
            destination.Scheme = source.ToString("scheme", isAttribute: true);
            destination.Value = source.ToString(CurrentXPath);
        }

        public override void Map(Identifier source, XElement destination)
        {
            destination.Add(XAttribute("scheme", source.Scheme));
            destination.Value = source.Value;
        }
    }
}