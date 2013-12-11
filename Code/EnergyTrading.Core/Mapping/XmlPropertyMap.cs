namespace EnergyTrading.Mapping
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Records a mapping between a <see cref="PropertyInfo" /> and a <see cref="XmlMapTarget" />
    /// <para>
    /// Provides sufficient information to achieve a two-way mapping between an object and XML.
    /// </para>
    /// </summary>
    public class XmlPropertyMap
    {
        /// <summary>
        /// Creates a new instance of the <see cref="XmlPropertyMap" /> class.
        /// </summary>
        /// <param name="info">PropertyInfo to use.</param>
        /// <param name="target"></param>
        public XmlPropertyMap(PropertyInfo info, XmlMapTarget target)
        {
            this.Info = info;
            this.Target = target;
        }

        /// <summary>
        /// Gets the property for the mapping.
        /// </summary>
        public PropertyInfo Info { get; private set; }

        /// <summary>
        /// Gets the mapping target type
        /// </summary>
        public XmlMapTarget Target { get; set; }

        /// <summary>
        /// Gets the XPath to be used to do the mapping
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// Uses a <see cref="XPathProcessor" /> to populate an object based on the mapping.
        /// </summary>
        /// <param name="processor">XPathProcessor to use</param>
        /// <param name="value">Object to populate</param>
        public void FromXml(XPathProcessor processor, object value)
        {
            throw new NotImplementedException("Not Implemented");
        }
    }
}