namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Whether an entity supports nullable properties.
    /// </summary>
    /// <remarks>
    /// This is used to support serialization behaviour so we can distinguish between the default value
    /// of a property and whether the value was explicitly assigned in code. In the first case, we
    /// do not necessarily want to serialize the value, e.g. serializing every zero or empty string
    /// adds no semantic value.
    /// </remarks>
    public interface INullableProperties
    {
        /// <summary>
        /// Get the null property bag.
        /// </summary>
        NullPropertyBag NullProperties { get; }
    }
}