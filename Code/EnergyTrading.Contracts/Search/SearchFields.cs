namespace EnergyTrading.Contracts.Search
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public class SearchFields
    {
        public SearchFields()
        {
            Criterias = new List<SearchCriteria>();
        }

        [DataMember(Order = 1)]
        [XmlElement("search-criteria")]
        public IList<SearchCriteria> Criterias { get; set; }

        [DataMember(Order = 2)]
        [XmlElement]
        public SearchCombinator Combinator { get; set; }
    }
}
