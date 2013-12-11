namespace EnergyTrading.UnitTest.Registrars.Maps
{
    using EnergyTrading.Mapping;
    using EnergyTrading.UnitTest.Mapping.Examples;

    public class OwnerXmlMapper : XmlMapper<Owner>
    {
        public override void Map(Owner source, System.Xml.Linq.XElement destination)
        {
        }

        public override void Map(XPathProcessor source, Owner destination)
        {
        }
    }
}
