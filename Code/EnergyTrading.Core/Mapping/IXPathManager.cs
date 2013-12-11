namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Constructs fully qualified XPaths from various values.
    /// </summary>
    public interface IXPathManager
    {
        /// <summary>
        /// Construct the qualified XPath for the supplied path.
        /// </summary>
        /// <param name="xpath">Base XPath to qualify.</param>
        /// <param name="prefix">Prefix to apply.</param>
        /// <param name="uri">XML namespace to apply.</param>
        /// <param name="index">Index if part of a node collection.</param>
        /// <param name="isAttribute">Whether we are accessing a element or attribute.</param>
        /// <returns>The qualified path.</returns>
        string QualifyXPath(string xpath, string prefix, string uri = null, int index = -1, bool isAttribute = false);
    }
}