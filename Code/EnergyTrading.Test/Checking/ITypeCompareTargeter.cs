namespace EnergyTrading.Test.Checking
{
    using System;

    public interface ITypeCompareTargeter
    {
        CompareTarget DetermineCompareTarget(Type type);

        void Register(Type type, CompareTarget target);
    }
}
