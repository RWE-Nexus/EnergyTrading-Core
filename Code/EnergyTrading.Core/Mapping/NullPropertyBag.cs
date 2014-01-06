namespace EnergyTrading.Mapping
{
    using System.Collections.Generic;

    /// <summary>
    /// Records whether properties of an object were null on deserialization.
    /// </summary>
    public class NullPropertyBag
    {
        private readonly HashSet<string> properties;

        /// <summary>
        /// Creates a new instance of the <see cref="NullPropertyBag" /> class.
        /// </summary>
        public NullPropertyBag()
        {
            this.properties = new HashSet<string>();
        }

        /// <summary>
        /// Get or set whether the property bag is in loading mode.
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        /// Determine whether a named property was null or not.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>true if the property was not null, false otherwise.</returns>
        public bool this[string name]
        {
            get
            {
                return this.properties.Contains(name);
            }

            set
            {
                if (value == false)
                {
                    this.properties.Remove(name);
                }
                else
                {
                    this.properties.Add(name);
                }
            }
        }

        /// <summary>
        /// Sets that the property was assigned.
        /// <para>
        /// No action if <see cref="Loading" /> is true.
        /// </para>
        /// </summary>
        /// <param name="name">Name of the property to assign.</param>
        public void Assigned(string name)
        {
            if (this.Loading)
            {
                return;
            }

            this[name] = false;
        }
    }
}