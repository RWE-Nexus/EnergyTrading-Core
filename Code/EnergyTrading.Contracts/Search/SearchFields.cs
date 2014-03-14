namespace EnergyTrading.Contracts.Search
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.energytrading.com/mdm")]
    [XmlType(Namespace = "http://schemas.energytrading.com/mdm")]
    public class SearchFields
    {
        public SearchFields()
        {
            this.Criterias = new List<SearchCriteria>();
        }

        [DataMember(Order = 1)]
        [XmlElement("search-criteria")]
        public IList<SearchCriteria> Criterias { get; set; }

        [DataMember(Order = 2)]
        [XmlElement]
        public SearchCombinator Combinator { get; set; }
    }
}
