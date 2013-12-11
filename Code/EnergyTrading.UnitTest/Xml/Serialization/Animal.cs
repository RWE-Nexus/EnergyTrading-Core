namespace EnergyTrading.UnitTest.Xml.Serialization
{
    using System.Xml.Serialization;

    [XmlRoot("animal")]
    public class Animal
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("tricks")]
        public string Tricks { get; set; }
    }
}