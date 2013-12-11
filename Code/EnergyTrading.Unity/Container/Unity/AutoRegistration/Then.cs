namespace EnergyTrading.Container.Unity.AutoRegistration
{
    /// <summary>
    /// Extension methods for fluent registration options
    /// </summary>
    public static class Then
    {
        /// <summary>
        /// Creates new registration options
        /// </summary>
        /// <returns>Fluent registration options</returns>
        public static IFluentRegistration Register()
        {
            return new RegistrationOptions();
        }
    }
}