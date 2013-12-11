namespace EnergyTrading.Configuration
{
    using System.Collections.Specialized;
    using System.Configuration;

    /// <summary>
    /// Abstraction over ConfigurationManager
    /// </summary>
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }

        ConnectionStringSettingsCollection ConnectionStrings { get; }

        object GetSection(string sectionName);
    }
}