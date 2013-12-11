namespace EnergyTrading.ServiceModel.Channels
{
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Removes the xsi declaration from a soap:Envelope
    /// </summary>
    public class SoapBodyXsiMessageTransformer : IMessageTransformer
    {
        /// <copydocfrom cref="IMessageTransformer.Transform" />
        public void Transform(XDocument document)
        {
            XNamespace soapEnvelope = "http://schemas.xmlsoap.org/soap/envelope/";
            const string XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

            var soapBody = document.Descendants(soapEnvelope + "Body").FirstOrDefault();
            if (soapBody == null)
            {
                return;
            }

            var attribute = soapBody.Attributes().FirstOrDefault(x => x.IsNamespaceDeclaration && x.Value == XsiNamespace);
            if (attribute != null)
            {
                attribute.Remove();
            }
        }
    }
}