namespace EnergyTrading.ServiceModel.Channels
{
    using System.Collections.Generic;
    using System.ServiceModel.Channels;

    /// <summary>
    /// Factory for the <see cref="CustomTextMessageEncoder" />
    /// </summary>
    public class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageEncoder encoder;
        private readonly MessageVersion version;
        private readonly string mediaType;
        private readonly string charSet;
        private readonly IList<IMessageTransformer> transformers;

        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageEncoderFactory" />
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="charSet"></param>
        /// <param name="version"></param>
        /// <param name="transformers"></param>
        public CustomTextMessageEncoderFactory(string mediaType, string charSet, MessageVersion version, IList<IMessageTransformer> transformers)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.transformers = transformers;
            encoder = new CustomTextMessageEncoder(this);
        }

        /// <inheritdoc />
        public override MessageEncoder Encoder
        {
            get { return encoder; }
        }

        /// <inheritdoc />
        public override MessageVersion MessageVersion
        {
            get { return version; }
        }

        /// <summary>
        /// Gets the MIME media type to use.
        /// </summary>
        public string MediaType
        {
            get { return mediaType; }
        }

        /// <summary>
        /// Gets the character set to use.
        /// </summary>
        public string CharSet
        {
            get { return charSet; }
        }

        /// <summary>
        /// Gets the list of <see cref="IMessageTransformer" />s to use.
        /// </summary>
        public IList<IMessageTransformer> Transformers
        {
            get { return transformers; }
        }
    }
}