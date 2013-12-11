namespace EnergyTrading.ServiceModel.Channels
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.Xml;

    /// <summary>
    /// The binding element that specifies the details of the <see cref="CustomTextMessageEncoder" />.
    /// </summary>
    public class CustomTextMessageBindingElement : MessageEncodingBindingElement, IWsdlExportExtension
    {
        private readonly XmlDictionaryReaderQuotas readerQuotas;
        private List<IMessageTransformer> transformers;
        private MessageVersion msgVersion;
        private string mediaType;
        private string encoding;

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        public CustomTextMessageBindingElement()
            : this("UTF-8")
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding"></param>
        public CustomTextMessageBindingElement(string encoding)
            : this(encoding, "text/xml")
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        public CustomTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, MessageVersion.Soap11WSAddressing10)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="binding"></param>
        public CustomTextMessageBindingElement(CustomTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion)
        {
            // NB Need to copy other elements here - invoked by WCF on Clone
            Transformers.AddRange(binding.Transformers);
            readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(readerQuotas);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageBindingElement" /> class.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <param name="msgVersion"></param>
        public CustomTextMessageBindingElement(string encoding, string mediaType, MessageVersion msgVersion)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            if (msgVersion == null)
            {
                throw new ArgumentNullException("msgVersion");
            }

            this.msgVersion = msgVersion;
            this.mediaType = mediaType;
            this.encoding = encoding;
            readerQuotas = new XmlDictionaryReaderQuotas();
        }

        /// <summary>
        /// Gets the message encoding
        /// </summary>
        public string Encoding
        {
            get { return encoding; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                encoding = value;
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary>
        public string MediaType
        {
            get { return mediaType; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                mediaType = value;
            }
        }

        /// <inheritdoc />
        public override MessageVersion MessageVersion
        {
            get { return msgVersion; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                msgVersion = value;
            }
        }

        /// <summary>
        /// This encoder does not enforces any quotas for the unsecure messages. The 
        /// quotas are enforced for the secure portions of messages when this encoder
        /// is used in a binding that is configured with security. 
        /// </summary>
        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get { return readerQuotas; }
        }

        /// <summary>
        /// Gets or sets the list of <see cref="IMessageTransformer" />s to apply.
        /// </summary>
        public List<IMessageTransformer> Transformers
        {
            get { return transformers ?? (transformers = new List<IMessageTransformer>()); }
            set { transformers = value; }
        }

        /// <inheritdoc />
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new CustomTextMessageEncoderFactory(MediaType, Encoding, MessageVersion, Transformers);
        }

        /// <inheritdoc />
        public override BindingElement Clone()
        {
            return new CustomTextMessageBindingElement(this);
        }

        /// <inheritdoc />
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        /// <inheritdoc />
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        /// <inheritdoc />
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        /// <inheritdoc />
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        /// <inheritdoc />
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)readerQuotas;
            }

            return base.GetProperty<T>(context);
        }

        void IWsdlExportExtension.ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        void IWsdlExportExtension.ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // The MessageEncodingBindingElement is responsible for ensuring that the WSDL has the correct
            // SOAP version. We can delegate to the WCF implementation of TextMessageEncodingBindingElement for this.
            var mebe = new TextMessageEncodingBindingElement { MessageVersion = msgVersion };
            ((IWsdlExportExtension)mebe).ExportEndpoint(exporter, context);
        }
    }
}
