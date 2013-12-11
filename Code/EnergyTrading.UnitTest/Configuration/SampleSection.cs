namespace EnergyTrading.UnitTest.Configuration
{
    using System.Configuration;

    public sealed class SampleSection : ConfigurationSection
    {
        /// <summary>
        /// Subscriber information for the broker.
        /// </summary>
        [ConfigurationProperty("parents", IsDefaultCollection = true)]
        public ParentCollection Parents
        {
            get { return (ParentCollection)base["parents"]; }
        }
    }
}
