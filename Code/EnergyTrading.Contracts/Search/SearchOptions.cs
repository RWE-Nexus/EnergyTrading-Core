namespace EnergyTrading.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public class SearchOptions
    {
        public static SearchOptions UnlimitedResults = new SearchOptions { ResultsPerPage = null, MultiPage = false };

        public SearchOptions()
        {
            this.ResultsPerPage = 200;
            this.MultiPage = true;
        }

        [DataMember(Order = 1)]
        [XmlElement("case-sensitivity")]
        public bool CaseSensitivity { get; set; }

        [DataMember(Order = 2)]
        [XmlElement("max-results")]
        public int? ResultsPerPage { get; set; }

        [DataMember(Order = 3)]
        [XmlElement("multi-page")]
        public bool MultiPage { get; set; }

        [DataMember(Order = 4)]
        [XmlElement("mapping-search")]
        public bool IsMappingSearch { get; set; }

        [DataMember(Order = 5)]
        [XmlElement("order-by")]
        public string OrderBy { get; set; }
    }
}

