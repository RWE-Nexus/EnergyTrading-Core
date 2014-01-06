namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using EnergyTrading.Mapping;
    using EnergyTrading.Xml;

    public class ScratchEntityXmlMapper : XmlMapper<ScratchEntity>
    {
        public ScratchEntityXmlMapper() : base("Entity")
        {
        }

        public bool AttributeNsSwitch
        {
            get { return this.AttributeDefaultNamespace; }
            set { this.AttributeDefaultNamespace = value; }
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
            destination.Add(this.Elements(source));
        }

        public IEnumerable<object> Elements(ScratchEntity source)
        {
            yield return this.XAttribute("Value", source.Value);
            yield return this.XElement("Id", source.Id, outputDefault: true);
            yield return this.XElement("Id2", source.Id2);
            yield return this.XElement("Name", source.Name, outputDefault: true);
            yield return this.XElement("Name2", source.Name2);
            yield return this.XElement("Date", source.Date, format: XmlExtensions.DateFormat);
            yield return this.XElement("DateTime", source.DateTime);
            yield return this.XElement("DateTimeOffset", source.DateTimeOffset);
            yield return this.XElement("Total", source.Total, outputDefault: !source.NullProperties["Total"]);
        }
    }
}