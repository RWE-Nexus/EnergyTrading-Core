namespace EnergyTrading.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.energytrading.com/mdm")]
    [XmlType(Namespace = "http://schemas.energytrading.com/mdm")]
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