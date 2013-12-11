namespace EnergyTrading.ServiceModel.Channels
{
    using System.Xml.Linq;

    /// <summary>
    /// Transforms a message in some way.
    /// </summary>
    public interface IMessageTransformer
    {
        /// <summary>
        /// Apply a transformation to the XML.
        /// </summary>
        /// <param name="document">XDocument to use.</param>
        void Transform(XDocument document);
    }
}