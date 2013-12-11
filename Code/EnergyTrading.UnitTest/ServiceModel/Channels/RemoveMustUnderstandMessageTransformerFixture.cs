namespace EnergyTrading.UnitTest.ServiceModel.Channels
{
    using System.Xml.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using EnergyTrading.ServiceModel.Channels;
    using EnergyTrading.Test.Xml;

    [TestClass]
    public class RemoveMustUnderstandMessageTransformerFixture : XmlFixture
    {
        [TestMethod]
        public void JustRemoveMustUnderstand()
        {
            var value = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
    <s:Header>
        <wsse:Security s:mustUnderstand='0' xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
            <wsse:UsernameToken wsu:Id='SecurityToken-3f7f983f-66ce-480d-bce6-170632d33f92' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                <wsse:Username>paul.hatcher1@rwe.com</wsse:Username>
                <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>Bo0kT1red</wsse:Password>
            </wsse:UsernameToken>
        </wsse:Security>
    </s:Header>
    <s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
        <FPMLTradeReportRequest xmlns='http://sdb.derivserv.dtcc.com/sdbfpml'>
            <FpmlPayload id='1' />
        </FPMLTradeReportRequest>
    </s:Body>
</s:Envelope>";

            var expected = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
    <s:Header>
        <wsse:Security xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
            <wsse:UsernameToken wsu:Id='SecurityToken-3f7f983f-66ce-480d-bce6-170632d33f92' xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
                <wsse:Username>paul.hatcher1@rwe.com</wsse:Username>
                <wsse:Password Type='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText'>Bo0kT1red</wsse:Password>
            </wsse:UsernameToken>
        </wsse:Security>
    </s:Header>
    <s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
        <FPMLTradeReportRequest xmlns='http://sdb.derivserv.dtcc.com/sdbfpml'>
            <FpmlPayload id='1' />
        </FPMLTradeReportRequest>
    </s:Body>
</s:Envelope>";

            // Arrange
            var transformer = new RemoveMustUnderstandMessageTransformer();
            var candidate = XDocument.Parse(value);

            // Act
            transformer.Transform(candidate);

            // Assert
            CheckXml(expected, candidate.Root, true);
        }
    }
}