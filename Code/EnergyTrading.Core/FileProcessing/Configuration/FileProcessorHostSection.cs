namespace EnergyTrading.FileProcessing.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Configuration information for file processors
    /// </summary>
    public class FileProcessorHostSection : ConfigurationSection
    {
        [ConfigurationProperty("processors")]
        public FileProcessorCollection Processors
        {
            get { return (FileProcessorCollection)this["processors"]; }
            set { this["processors"] = value; }
        }
    }
}