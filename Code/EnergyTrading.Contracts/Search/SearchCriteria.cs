namespace EnergyTrading.Contracts.Search
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.energytrading.com/mdm")]
    [XmlType(Namespace = "http://schemas.energytrading.com/mdm")]
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