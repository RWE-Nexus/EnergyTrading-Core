namespace EnergyTrading.Filtering
{
    using System.Collections.Generic;

    public interface IFilterRepository
    {
        IList<string> Included { get; }

        IList<string> Excluded { get; }
    }
}