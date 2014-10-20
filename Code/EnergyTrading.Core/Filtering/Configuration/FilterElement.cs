namespace EnergyTrading.Filtering.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;

    public class FilterElement : NamedConfigElement
    {
        [ConfigurationProperty("included")]
        public FilterValueCollection Included
        {
            get
            {
                return (FilterValueCollection)this["included"];
            }
            set
            {
                this["included"] = value;
            }
        }

        [ConfigurationProperty("excluded")]
        public FilterValueCollection Excluded
        {
            get
            {
                return (FilterValueCollection)this["excluded"];
            }
            set
            {
                this["excluded"] = value;
            }
        }
    }
}