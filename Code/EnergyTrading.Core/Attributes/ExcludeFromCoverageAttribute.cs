namespace EnergyTrading.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ExcludeFromCoverageAttribute : Attribute
    {
    }
}