namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class IdXmlMapper : XmlMapper<Id>
    {
        public IdXmlMapper() : base("id")
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

        public override void Map(XPathProcessor source, Id destination)
        {
            destination.System = source.ToString("system", isAttribute: true);
            destination.Value = source.ToString(CurrentXPath);
        }

        public override void Map(Id source, XElement destination)
        {
            destination.Add(this.XAttribute("system", source.System));
            destination.Value = source.Value;
        }
    }
}