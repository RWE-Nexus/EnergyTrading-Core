namespace EnergyTrading.UnitTest.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;

    /// <summary>
    /// Configuration class for Child.
    /// </summary>
    public class ChildElement : NamedConfigElement
    {
        /// <summary>
        /// The Child's name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "Not Supplied")]
        public override string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
