namespace EnergyTrading.Contracts.Search
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public class Search
    {
        public Search()
        {
            SearchFields = new SearchFields();
            SearchOptions = new SearchOptions();
        }

        [DataMember(Order = 1)]
        [XmlElement("search-fields")]
        public SearchFields SearchFields { get; set; }

        [DataMember(Order = 2)]
        [XmlElement("search-options")]
        public SearchOptions SearchOptions { get; set; }

        [DataMember(Order = 3)]
        [XmlElement("as-of")]
        public DateTime? AsOf { get; set; }
    } 
}