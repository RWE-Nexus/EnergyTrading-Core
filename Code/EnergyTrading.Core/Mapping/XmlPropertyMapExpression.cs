namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Fluent interface over a <see cref="XmlPropertyMap" />
    /// </summary>
    public class XmlPropertyMapExpression
    {
        private readonly XmlPropertyMap map;

        /// <summary>
        /// Creates a new instance of the <see cref="XmlPropertyMapExpression" /> class.
        /// </summary>
        /// <param name="map">XmlPropertyMap to use.</param>
        public XmlPropertyMapExpression(XmlPropertyMap map)
        {
            this.map = map;
        }

        /// <summary>
        /// Set the <see cref="XmlPropertyMap.Target" /> to <see cref="XmlMapTarget.Id" />
        /// </summary>
        /// <returns>The current XmlPropertyMapExpression.</returns>
        public XmlPropertyMapExpression Id()
        {
            map.Target = XmlMapTarget.Id;

            return this;
        }

        /// <summary>
        /// Set the <see cref="XmlPropertyMap.Target" /> to <see cref="XmlMapTarget.Entity" />
        /// </summary>
        /// <returns>The current XmlPropertyMapExpression.</returns>
        public XmlPropertyMapExpression Entity()
        {
            map.Target = XmlMapTarget.Entity;

            return this;
        }

        /// <summary>
        /// Set the <see cref="XmlPropertyMap.Target" /> to <see cref="XmlMapTarget.Count" />
        /// </summary>
        /// <returns>The current XmlPropertyMapExpression.</returns>
        public XmlPropertyMapExpression Count()
        {
            map.Target = XmlMapTarget.Count;

            return this;
        }

        /// <summary>
        /// Set the <see cref="XmlPropertyMap.Target" /> to <see cref="XmlMapTarget.Value" />
        /// </summary>
        /// <returns>The current XmlPropertyMapExpression.</returns>
        public XmlPropertyMapExpression Value()
        {
            map.Target = XmlMapTarget.Value;

            return this;
        }
    }
}