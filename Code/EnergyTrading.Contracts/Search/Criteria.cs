namespace EnergyTrading.Contracts.Search
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    [XmlType(Namespace = "http://schemas.rwe.com/EnergyTrading")]
    public class Criteria
    {
        [DataMember(Order = 1)]
        [XmlElement]
        public string Field { get; set; }

        [DataMember(Order = 2)]
        [XmlElement]
        public SearchCondition Condition { get; set; }

        [DataMember(Order = 3)]
        [XmlElement("comparison-value")]
        public string ComparisonValue { get; set; }

        [DataMember(Order = 4)]
        [XmlElement]
        public bool? IsNumeric { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = (hash * 29) + (this.Field == null ? 0 : this.Field.GetHashCode());
                hash = (hash * 29) + this.Condition.GetHashCode();
                hash = (hash * 29) + (this.ComparisonValue == null ? 0 : this.ComparisonValue.GetHashCode());
                hash = (hash * 29) + this.IsNumeric.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            var cv = this.ComparisonValue;
            if (!this.IsNumeric.HasValue || this.IsNumeric.Value == false)
            {
                cv = "\"" + cv + "\"";
            }

            return string.Format("{0} {1} {2}", this.Field, this.Condition.ToOperator(), cv);
        }
    }
}