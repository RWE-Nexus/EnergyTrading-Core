namespace EnergyTrading.Data.SimpleData
{
    using System.Collections.Generic;
    using System.Configuration;

    using EnergyTrading.Configuration;

    using Simple.Data;

    /// <summary>
    /// Class to cache and provide Simple.Data Database objects on a per connection string basis
    /// </summary>
    public class SimpleDataDatabaseProvider
    {
        public const string DefaultConnectionName = "Simple.Data.Properties.Settings.DefaultConnectionString";

        private static readonly Dictionary<string, Database> DatabaseCache = new Dictionary<string, Database>();

        public static Database GetDefaultDatabase(IConfigurationManager configurationManager)
        {
            return GetDatabase(configurationManager, null);
        }

        public static Database GetDatabase(IConfigurationManager configurationManager, string connectionName)
        {
            var connectionSettings = configurationManager.GetConnectionSettingsWithPassword(string.IsNullOrWhiteSpace(connectionName) ? DefaultConnectionName : connectionName);
            return GetDatabase(connectionSettings);
        }

        public static Database GetDatabase(ConnectionStringSettings connectionSettings)
        {
            if (!DatabaseCache.ContainsKey(connectionSettings.Name))
            {
                var db = Database.Opener.OpenConnection(connectionSettings.ConnectionString, connectionSettings.ProviderName);
                DatabaseCache.Add(connectionSettings.Name, db);
            }

            return DatabaseCache[connectionSettings.Name];
        }
    }
}