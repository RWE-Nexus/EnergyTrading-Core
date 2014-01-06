namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Collections.Generic;

    public class WcfConfig : IWcfConfig
    {
        private readonly Dictionary<Type, WcfServiceConfig> typeConfig;
        private readonly Dictionary<string, WcfServiceConfig> keyConfig;

        public WcfConfig()
        {
            this.typeConfig = new Dictionary<Type, WcfServiceConfig>();
            this.keyConfig = new Dictionary<string, WcfServiceConfig>();
        }

        public WcfConfig(bool consoleMode) : this()
        {
            this.IsConsoleMode = consoleMode;
        }

        public bool IsConsoleMode { get; private set; }

        public ICollection<WcfServiceConfig> Services
        {
            get { throw new NotImplementedException(); }
        }

        public WcfServiceConfig Get(Type type)
        {
            WcfServiceConfig config;
            return this.typeConfig.TryGetValue(type, out config) ? config : null;
        }

        public WcfServiceConfig Get(string key)
        {
            WcfServiceConfig config;
            return this.keyConfig.TryGetValue(key, out config) ? config : null;
        }

        public void Add(WcfServiceConfig config)
        {
            this.Add(config, config.HostType);
            this.Add(config, config.ContractType);
            this.Add(config, config.Key);
        }

        private void Add(WcfServiceConfig config, Type type)
        {
            if (type != null && !this.typeConfig.ContainsKey(type))
            {
                this.typeConfig.Add(type, config);
            } 
        }

        private void Add(WcfServiceConfig config, string key)
        {
            if (!this.keyConfig.ContainsKey(key))
            {
                this.keyConfig.Add(key, config);
            }  
        }
    }
}