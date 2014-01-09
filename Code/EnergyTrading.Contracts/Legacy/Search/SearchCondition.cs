namespace RWEST.Nexus.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/nexus")]
    [XmlType(Namespace = "http://schemas.rwe.com/nexus")]
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