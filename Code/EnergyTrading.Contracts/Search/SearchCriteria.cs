namespace EnergyTrading.Contracts.Search
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public class SearchCriteria
    {
        [DataMember(Order = 1)]
        [XmlElement]
        public List<Criteria> Criteria { get; set; }

        [DataMember(Order = 2)]
        [XmlElement]
        public SearchCombinator Combinator { get; set; }
    }
}