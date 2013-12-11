namespace EnergyTrading.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public enum SearchCondition
    {
        [EnumMember]
        Equals,
        [EnumMember]
        Contains,
        [EnumMember]
        NumericEquals,
        [EnumMember]
        GreaterThan,
        [EnumMember]
        GreaterThanEquals,
        [EnumMember]
        LessThan,
        [EnumMember]
        LessThanEquals,
        [EnumMember]
        NotEquals
    }
}