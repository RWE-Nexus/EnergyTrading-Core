namespace EnergyTrading.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public enum SearchCombinator
    {
        [EnumMember]
        And,
        [EnumMember]
        Or
    }
}

