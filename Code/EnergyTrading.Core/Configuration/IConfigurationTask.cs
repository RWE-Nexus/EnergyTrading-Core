namespace EnergyTrading.Configuration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Tasks that perform configuration logic
    /// </summary>
    public interface IConfigurationTask
    {
        IList<Type> DependsOn { get; }

        void Configure();
    }
}