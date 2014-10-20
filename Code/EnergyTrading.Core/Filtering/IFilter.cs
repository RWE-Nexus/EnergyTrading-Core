namespace EnergyTrading.Filtering
{
    using System;

    public interface IFilter
    {
        bool IsExcluded(string inputValue, StringComparison comparison = StringComparison.InvariantCulture); 
    }
}