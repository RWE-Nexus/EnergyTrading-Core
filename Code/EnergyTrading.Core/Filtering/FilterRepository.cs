namespace EnergyTrading.Filtering
{
    using System.Collections.Generic;

    public class FilterRepository : IFilterRepository
    {
        public FilterRepository()
        {
            Included = new List<string>();
            Excluded = new List<string>();
        }

        public IList<string> Included { get; private set; }
        public IList<string> Excluded { get; private set; }
    }
}