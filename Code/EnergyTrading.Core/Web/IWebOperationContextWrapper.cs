namespace EnergyTrading.Web
{
    using System;
    using System.Collections.Specialized;
    using System.Net;

    /// <summary>
    /// Hides the WebOperationContext so we can test WCF services more easily
    /// </summary>
    public interface IWebOperationContextWrapper
    {
        /// <summary>
        /// Gets or sets the content type of the outgoing message.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the Location.
        /// </summary>
        string Location { get; set; }

        string InboundAbsoloutePath { get; }

        Uri BaseUri { get; }

        HttpStatusCode StatusCode { get; set; }

        NameValueCollection QueryParameters { get; }

        WebHeaderCollection Headers { get; }

        void CheckConditionalRetrieve(long entityTag);

        void SetETag(long entityTag);
    }
}