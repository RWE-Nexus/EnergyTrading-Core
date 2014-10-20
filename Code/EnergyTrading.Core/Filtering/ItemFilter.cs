namespace EnergyTrading.Filtering
{
    using System;
    using System.Linq;

    public class ItemFilter : IFilter
    {
        private readonly IFilterRepository repository;

        public ItemFilter(IFilterRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            this.repository = repository;
        }

        public bool IsExcluded(string inputValue, StringComparison comparison = StringComparison.InvariantCulture)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return false;
            }

            if (repository.Included.Count > 0 && repository.Included.FirstOrDefault(x => string.Compare(x, inputValue, comparison) == 0) == null)
            {
                return true;
            }

            if (repository.Excluded.Count > 0 && repository.Excluded.FirstOrDefault(x => string.Compare(x, inputValue, comparison) == 0) != null)
            {
                return true;
            }

            return false;
        }
    }
}