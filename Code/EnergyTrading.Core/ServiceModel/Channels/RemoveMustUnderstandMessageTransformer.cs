namespace EnergyTrading.ServiceModel.Channels
{
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Remove the SOAP mustUnderstand attribute from the security header.
    /// </summary>
    public class RemoveMustUnderstandMessageTransformer : IMessageTransformer
    {
        /// <copydocfrom cref="IMessageTransformer.Transform" />
        public void Transform(XDocument document)
        {
            XNamespace soapEnvelope = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace sec = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

            var securityHeader = document.Descendants(sec + "Security").FirstOrDefault();
            if (securityHeader == null)
            {
                return;
            }

            // TODO: Generalise to add/remove set to 0 or 1.
            var attribute = securityHeader.Attributes(soapEnvelope + "mustUnderstand").FirstOrDefault();
            if (attribute != null)
            {
                attribute.Remove();
            }
        }
    }
}