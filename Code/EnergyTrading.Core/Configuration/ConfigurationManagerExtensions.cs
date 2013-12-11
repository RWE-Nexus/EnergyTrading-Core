namespace EnergyTrading.Configuration
{
    using System;
    using System.Configuration;
    using System.Linq;

    using EnergyTrading.Crypto;

    public static class ConfigurationManagerExtensions
    {
        public static readonly string DefaultVerificationPrefix = "Verification";
        public static readonly string VerificationPrefixAppSetting = "VerificationPrefix";
        public static readonly string VerificationEnvironmentPrefixAppSetting = "VerificationEnvironmentPrefix";
        public static readonly string StoreTestDataToEndpointAppSetting = "StoreTestDataToEndpoint";

        public static ConnectionStringSettings GetConnectionSettingsWithPassword(this IConfigurationManager configurationManager, string connectionName)
        {
            var connectionSettings = configurationManager.ConnectionStrings[connectionName];
            if (connectionSettings == null)
            {
                throw new InvalidOperationException(connectionName + "not found in the config file connection strings section");
            }

            var passwordKey = connectionName + ".EncryptedPassword";
            var passwordSection = string.Empty;
            if (configurationManager.AppSettings.AllKeys.Contains(passwordKey))
            {
                var encrypted = configurationManager.AppSettings[passwordKey];
                passwordSection = ";Password=" + encrypted.DecryptString();
            }

            return new ConnectionStringSettings(connectionName, connectionSettings.ConnectionString + passwordSection, connectionSettings.ProviderName);
        }

        public static string GetVerificationPrefix(this IConfigurationManager configurationManager)
        {
            return configurationManager.GetAppSettingValue(VerificationPrefixAppSetting) ?? DefaultVerificationPrefix;
        }

        public static string GetAppSettingValue(this IConfigurationManager configurationManager, string appSettingKey)
        {
            if (configurationManager != null && configurationManager.AppSettings.AllKeys.Contains(appSettingKey) && !string.IsNullOrWhiteSpace(configurationManager.AppSettings[appSettingKey]))
            {
                return configurationManager.AppSettings[appSettingKey];
            }
            return null;
        }

        public static string GetVerificationEnvironment(this IConfigurationManager configurationManager)
        {
            return configurationManager.GetAppSettingValue(VerificationEnvironmentPrefixAppSetting);
        }
    }
}