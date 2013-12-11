namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Mapping.Extensions;
    using EnergyTrading.Xml;

    public class ScratchEntityXmlMapper : XmlMapper<ScratchEntity>
    {
        public ScratchEntityXmlMapper() : base("Entity")
        {
        }

        public bool AttributeNsSwitch
        {
            get { return AttributeDefaultNamespace; }
            set { AttributeDefaultNamespace = value; }
        }

        protected override string Namespace
        {
            get { return XmlNamespaces.AppNamespace; }
        }

        protected override string NamespacePrefix
        {
            get { return XmlNamespaces.AppNamespacePrefix; }
        }

        public override void Map(XPathProcessor source, ScratchEntity destination)
        {
            destination.Id = source.ToString("Id");
            destination.Id2 = source.ToString("Id2");
            destination.Name = source.ToString("Name");
            destination.Name2 = source.ToString("Name2");
            destination.Value = source.ToInt("Value", isAttribute: true);
            destination.Date = source.ToDateTime("Date");
            destination.DateTime = source.ToDateTime("DateTime");
            destination.DateTimeOffset = source.ToDateTimeOffset("DateTimeOffset");
            destination.Total = source.ToInt("Total");
        }

        public override void Map(ScratchEntity source, XElement destination)
        {
            destination.Add(Elements(source));
        }

        public IEnumerable<object> Elements(ScratchEntity source)
        {
            yield return XAttribute("Value", source.Value);
            yield return XElement("Id", source.Id, outputDefault: true);
            yield return XElement("Id2", source.Id2);
            yield return XElement("Name", source.Name, outputDefault: true);
            yield return XElement("Name2", source.Name2);
            yield return XElement("Date", source.Date, format: XmlExtensions.DateFormat);
            yield return XElement("DateTime", source.DateTime);
            yield return XElement("DateTimeOffset", source.DateTimeOffset);
            yield return XElement("Total", source.Total, outputDefault: !source.NullProperties["Total"]);
        }
    }
}