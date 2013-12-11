namespace EnergyTrading.Data
{
    /// <summary>
    /// An entity that has a unique identity.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        /// Get the identity.
        /// </summary>
        object Id { get; }
    }
}