namespace RWEST.Nexus.Contracts.Atom
{
    /// <summary>
    /// Provides mapping extension methods to map to/from the obsolete Nexus.* class and the new EnergyTrading.* class, 
    /// intended to be used within legacy Nexus code
    /// </summary>
    public static class LinkExtensions
    {
        /// <summary>
        /// Maps an obsolete Nexus Link to the new EnergyTrading Link type
        /// </summary>
        public static EnergyTrading.Contracts.Atom.Link FromNexus(this Link nexusLink)
        {
            return new EnergyTrading.Contracts.Atom.Link
                       {
                           Rel = nexusLink.Rel,
                           Type = nexusLink.Type,
                           Uri = nexusLink.Uri
                       };
        }

        /// <summary>
        /// Maps an EnergyTrading Link to the obsolete Nexus Link type
        /// </summary>
        public static Link ToNexus(this EnergyTrading.Contracts.Atom.Link link)
        {
            return new Link
                       {
                           Rel = link.Rel,
                           Type = link.Type,
                           Uri = link.Uri
                       };
        }
    }
}