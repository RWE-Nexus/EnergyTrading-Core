namespace EnergyTrading.ServiceModel.Channels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using EnergyTrading.ServiceModel.Loggers;
    using EnergyTrading.Xml.Serialization;

    /// <summary>
    /// Custom text encoder that can apply multiple transformations.
    /// </summary>
    public class CustomTextMessageEncoder : MessageEncoder
    {
        private readonly CustomTextMessageEncoderFactory factory;
        private readonly XmlWriterSettings writerSettings;
        private readonly IList<IMessageTransformer> transformers;
        private readonly string contentType;
        
        /// <summary>
        /// Creates a new instance of the <see cref="CustomTextMessageEncoder" /> class.
        /// </summary>
        /// <param name="factory">Factory to use</param>
        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            this.factory = factory;
            transformers = factory.Transformers;
           
            writerSettings = new XmlWriterSettings { Encoding = Encoding.GetEncoding(factory.CharSet) };
            contentType = string.Format("{0}; charset={1}", factory.MediaType, writerSettings.Encoding.HeaderName);
            transformers = factory.Transformers;
        }

        /// <summary>
        /// Alternate content type that should be treated the same as the actual content type.
        /// <para>
        /// This allows for slightly rogue services which don't want to obey the rules, e.g. returning faults as text/html rather than text/xml
        /// </para>
        /// </summary>
        public string AlternateContentType
        {
            get { return factory.AlternateContentType; }
        }

        /// <inheritdoc />
        public override string ContentType
        {
            get { return contentType; }
        }

        /// <inheritdoc />
        public override string MediaType
        {
            get { return factory.MediaType; }
        }

        /// <summary>
        /// Gets the message logger to use.
        /// </summary>
        public IMessageLogger MessageLogger
        {
            get { return factory.MessageLogger; }
        }

        /// <inheritdoc />
        public override MessageVersion MessageVersion
        {
            get { return factory.MessageVersion; }
        }

        /// <inheritdoc />
        public override bool IsContentTypeSupported(string contentType)
        {
            return base.IsContentTypeSupported(contentType) || IsAlternateContent(contentType);
        }

        /// <inheritdoc />
        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            // Treat the alternate as native
            if (IsAlternateContent(contentType))
            {
                // HACK: Should see if we have a declr or some xml
               contentType = this.contentType;
            }

            var msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            // Most interoperable to include the xml declaration
            writerSettings.OmitXmlDeclaration = false;

            // Save the encoding for when we write the response
            writerSettings.Encoding = msgContents.GetEncoding(contentType);
            var xmlDeclEncoding = msgContents.GetXmlDeclEncoding(writerSettings.Encoding);

            // Check if the two encodings align
            if (xmlDeclEncoding != null && xmlDeclEncoding.WebName == writerSettings.Encoding.WebName)
            {
                // Need to recode
                msgContents = Encoding.Convert(writerSettings.Encoding, xmlDeclEncoding, msgContents);
            }

            var stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        /// <inheritdoc />
        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            var reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, MessageVersion);
        }

        /// <inheritdoc />
        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            // Produce the basic stream
            var stream = new MemoryStream();
            message.XmlToStream(writerSettings, stream);

            // Apply the transformers
            stream = Transform(stream);

            // Now produce the final version of the message.
            var streamBuffer = stream.GetBuffer();
            var messageLength = (int)stream.Position;

            // And log it
            LogMessage(stream);
            stream.Close();

            var totalLength = messageLength + messageOffset;
            var wcfBuffer = bufferManager.TakeBuffer(totalLength);
            Array.Copy(streamBuffer, 0, wcfBuffer, messageOffset, messageLength);

            var byteArray = new ArraySegment<byte>(wcfBuffer, messageOffset, messageLength);
            return byteArray;
        }

        /// <inheritdoc />
        public override void WriteMessage(Message message, Stream stream)
        {
            message.XmlToStream(writerSettings, stream);
        }

        private bool IsAlternateContent(string value)
        {
            return !string.IsNullOrEmpty(AlternateContentType) && value.StartsWith(AlternateContentType);
        }

        private void LogMessage(Stream stream)
        {
            var message = MessageToLog(stream);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            MessageLogger.Sent(message);
        }

        private string MessageToLog(Stream stream)
        {           
            if (MessageLogger == null || MessageLogger is NullMessageLogger)
            {
                return null;
            }

            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private MemoryStream Transform(Stream stream)
        {
            stream.Position = 0;
            var document = XDocument.Load(stream);

            // Apply the transformers
            foreach (var transformer in transformers)
            {
                transformer.Transform(document);
            }

            return document.WriteTo(writerSettings);
        }
    }
}