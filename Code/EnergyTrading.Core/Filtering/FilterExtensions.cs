namespace EnergyTrading.Filtering
{
    using EnergyTrading.Configuration;
    using EnergyTrading.Filtering.Configuration;

    public static class FilterExtensions
    {
        public static IFilterRepository ToFilterRepository(this IConfigurationManager configurationManager, string sectionName = "filterSection")
        {
            if (configurationManager == null || string.IsNullOrWhiteSpace(sectionName))
            {
                return null;
            }

            var section = configurationManager.GetSection(sectionName) as FilterSection;
            return section != null ? section.ToFilterRepository() : null;
        }


        public static IFilterRepository ToFilterRepository(this FilterSection filterSection)
        {
            return filterSection == null ? null : CreateFilterRepository(filterSection.Included, filterSection.Excluded);
        }

        public static IFilterRepository ToFilterRepository(this FilterElement filterElement)
        {
            return filterElement == null ? null : CreateFilterRepository(filterElement.Included, filterElement.Excluded);
        }

        public static IFilter ToFilter(this IConfigurationManager configurationManager, string sectionName = "filterSection")
        {
            if (configurationManager == null || string.IsNullOrWhiteSpace(sectionName))
            {
                return null;
            }

            var section = configurationManager.GetSection(sectionName) as FilterSection;
            var filterRepository = section != null ? section.ToFilterRepository() : null;
            return filterRepository == null ? null : new ItemFilter(filterRepository);
        }


        public static IFilter ToFilter(this FilterSection filterSection)
        {
            var filterRepository = filterSection == null ? null : CreateFilterRepository(filterSection.Included, filterSection.Excluded);
            return filterRepository == null ? null : new ItemFilter(filterRepository);
        }

        public static IFilter ToFilter(this FilterElement filterElement)
        {
            var filterRepository = filterElement == null ? null : CreateFilterRepository(filterElement.Included, filterElement.Excluded);
            return filterRepository == null ? null : new ItemFilter(filterRepository);
        }

        private static IFilterRepository CreateFilterRepository(FilterValueCollection included, FilterValueCollection excluded)
        {
            var ret = new FilterRepository();
            if (included != null)
            {
                foreach (FilterValueElement item in included)
                {
                    if (!string.IsNullOrWhiteSpace(item.Name))
                    {
                        ret.Included.Add(item.Name);
                    }
                }
            }
            if (excluded != null)
            {
                foreach (FilterValueElement item in excluded)
                {
                    if (!string.IsNullOrWhiteSpace(item.Name))
                    {
                        ret.Excluded.Add(item.Name);
                    }
                }
            }
            return ret;
        }
    }
}