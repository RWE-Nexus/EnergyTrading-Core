namespace EnergyTrading.UnitTest.Mapping.Examples
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    using EnergyTrading.Mapping;

    public class EntityXmlMapper : XmlMapper<Entity>
    {
        public EntityXmlMapper() : base("Entity")
        {
        }

        public override void Map(XPathProcessor source, Entity destination)
        {
            destination.Id = source.ToString("Id");
            destination.Id2 = source.ToString("Id2");
            destination.Name = source.ToString("Name");
            destination.Name2 = source.ToString("Name2");
            destination.Value = source.ToInt("Value", isAttribute: true);
            destination.Total = source.ToInt("Total");
            destination.NullProperties["Total"] = source.IsNull("Total");
        }

        public override void Map(Entity source, XElement destination)
        {
            destination.Add(XAttribute("Value", source.Value));
            destination.Add(Elements(source));
        }

        public IEnumerable<XElement> Elements(Entity source)
        {
            yield return XElement("Id", source.Id, outputDefault: true);
            yield return XElement("Id2", source.Id2);
            yield return XElement("Name", source.Name, outputDefault: true);
            yield return XElement("Name2", source.Name2);
            yield return XElement("Total", source.Total, outputDefault: !source.NullProperties["Total"]);
        }
    }
}
