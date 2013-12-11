namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Collections.Generic;

    public interface IWcfConfig
    {
        bool IsConsoleMode { get; }

        ICollection<WcfServiceConfig> Services { get; }

        WcfServiceConfig Get(Type type);

        WcfServiceConfig Get(string key);
    }
}
