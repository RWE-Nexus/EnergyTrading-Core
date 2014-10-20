namespace EnergyTrading.Filtering.Configuration
{
    using EnergyTrading.Configuration;

    public class FilterValueCollection : NamedConfigElementCollection<FilterValueElement>
    {
        protected override string ElementName
        {
            get
            {
                return "filterValue";
            }
        }
    }
}