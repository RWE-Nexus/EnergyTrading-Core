namespace EnergyTrading.ServiceModel.Channels
{
    using System.Collections.Generic;
    using System.ServiceModel.Channels;

    using EnergyTrading.ServiceModel.Loggers;

    /// <summary>
    /// Factory for the <see cref="CustomTextMessageEncoder" />
    /// </summary>
    public class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageEncoder encoder;
        private readonly MessageVersion version;
        private readonly string mediaType;
        private readonly string charSet;
        private readonly string alternateContentType;
        private readonly IMessageLogger messageLogger;
        private readonly IList<IMessageTransformer> transformers;

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageEncoderFactory" />
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="alternateContentType"></param>
        /// <param name="messageLogger"></param>
        /// <param name="version"></param>
        /// <param name="transformers"></param>
        public CustomTextMessageEncoderFactory(string mediaType, string charSet, string alternateContentType, IMessageLogger messageLogger, MessageVersion version, IList<IMessageTransformer> transformers)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.alternateContentType = alternateContentType;
            this.messageLogger = messageLogger ?? new NullMessageLogger();
            this.transformers = transformers;
            this.encoder = new CustomTextMessageEncoder(this);
        }

        /// <inheritdoc />
        public override MessageEncoder Encoder
        {
            get { return this.encoder; }
        }

        /// <inheritdoc />
        public override MessageVersion MessageVersion
        {
            get { return this.version; }
        }

        /// <summary>
        /// Gets the alternate content type to use.
        /// </summary>
        public string AlternateContentType
        {
            get { return alternateContentType; }
        }

        /// <summary>
        /// Gets the MIME media type to use.
        /// </summary>
        public string MediaType
        {
            get { return this.mediaType; }
        }

        /// <summary>
        /// Gets the character set to use.
        /// </summary>
        public string CharSet
        {
            get { return this.charSet; }
        }

        /// <summary>
        /// Gets the message logger to use
        /// </summary>
        public IMessageLogger MessageLogger
        {
            get { return messageLogger; }
        }

        /// <summary>
        /// Gets the list of <see cref="IMessageTransformer" />s to use.
        /// </summary>
        public IList<IMessageTransformer> Transformers
        {
            get { return this.transformers; }
        }
    }
}