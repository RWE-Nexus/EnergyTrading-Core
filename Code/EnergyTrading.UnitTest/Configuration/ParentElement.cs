namespace EnergyTrading.UnitTest.Configuration
{
    using System.Configuration;

    using EnergyTrading.Configuration;

    /// <summary>
    /// Configuration class for Parent.
    /// </summary>
    public class ParentElement : NamedConfigElement
    {
        /// <summary>
        /// The Parent's type.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true, DefaultValue = "Not Supplied")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        /// <summary>
        /// Collection of applications that the the subscriber is interested in.
        /// </summary>
        [ConfigurationProperty("children", IsRequired = true, IsDefaultCollection = true)]
        public ChildCollection Children
        {
            get { return (ChildCollection)base["children"]; }
        }
    }
}
