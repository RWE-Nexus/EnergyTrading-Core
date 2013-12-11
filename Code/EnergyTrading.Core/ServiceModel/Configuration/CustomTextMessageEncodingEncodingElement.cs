namespace EnergyTrading.ServiceModel.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Configuration;
    using System.Xml;

    using EnergyTrading.Extensions;
    using EnergyTrading.ServiceModel.Channels;

    /// <summary>
    /// Enables the use of <see cref="CustomTextMessageBindingElement" /> from a machine or application configuration files.
    /// </summary>
    public class CustomTextMessageEncodingElement : BindingElementExtensionElement
    {
        /// <inheritdoc />
        public override Type BindingElementType
        {
            get { return typeof(CustomTextMessageBindingElement); }
        }

        /// <summary>
        /// Gets or sets the message version.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.MessageVersion, DefaultValue = ConfigurationStrings.DefaultMessageVersion)]
        [TypeConverter(typeof(MessageVersionConverter))]
        public MessageVersion MessageVersion
        {
            get { return (MessageVersion)base[ConfigurationStrings.MessageVersion]; }
            set { base[ConfigurationStrings.MessageVersion] = value; }
        }

        /// <summary>
        /// Gets or sets the MIME media type.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.MediaType, DefaultValue = ConfigurationStrings.DefaultMediaType)]
        public string MediaType
        {
            get { return (string)base[ConfigurationStrings.MediaType]; }
            set { base[ConfigurationStrings.MediaType] = value; }
        }

        /// <summary>
        /// Gets or sets the text encoding.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.Encoding, DefaultValue = ConfigurationStrings.DefaultEncoding)]
        public string Encoding
        {
            get { return (string)base[ConfigurationStrings.Encoding]; }
            set { base[ConfigurationStrings.Encoding] = value; }
        }

        /// <summary>
        /// Gets the reader quotas
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.ReaderQuotas)]
        public XmlDictionaryReaderQuotasElement ReaderQuotasElement
        {
            get { return (XmlDictionaryReaderQuotasElement)base[ConfigurationStrings.ReaderQuotas]; }
        }

        /// <summary>
        /// Gets the transformers to apply.
        /// </summary>
        [ConfigurationProperty(ConfigurationStrings.Transformers)]
        public string Transformers
        {
            get { return (string)base[ConfigurationStrings.Transformers]; }
            set { base[ConfigurationStrings.Transformers] = value; }
        }

        /// <inheritdoc />
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
            var binding = (CustomTextMessageBindingElement)bindingElement;
            binding.MessageVersion = MessageVersion;
            binding.MediaType = MediaType;
            binding.Encoding = Encoding;
            binding.Transformers.AddRange(CreateTransformers());
            ApplyConfiguration(binding.ReaderQuotas);
        }

        /// <inheritdoc />
        protected override BindingElement CreateBindingElement()
        {
            var binding = new CustomTextMessageBindingElement();
            ApplyConfiguration(binding);

            return binding;
        }

        private void ApplyConfiguration(XmlDictionaryReaderQuotas readerQuotas)
        {
            if (readerQuotas == null)
            {
                throw new ArgumentNullException("readerQuotas");
            }

            if (ReaderQuotasElement.MaxDepth != 0)
            {
                readerQuotas.MaxDepth = ReaderQuotasElement.MaxDepth;
            }
            if (ReaderQuotasElement.MaxStringContentLength != 0)
            {
                readerQuotas.MaxStringContentLength = ReaderQuotasElement.MaxStringContentLength;
            }
            if (ReaderQuotasElement.MaxArrayLength != 0)
            {
                readerQuotas.MaxArrayLength = ReaderQuotasElement.MaxArrayLength;
            }
            if (ReaderQuotasElement.MaxBytesPerRead != 0)
            {
                readerQuotas.MaxBytesPerRead = ReaderQuotasElement.MaxBytesPerRead;
            }
            if (ReaderQuotasElement.MaxNameTableCharCount != 0)
            {
                readerQuotas.MaxNameTableCharCount = ReaderQuotasElement.MaxNameTableCharCount;
            }
        }

        private IEnumerable<IMessageTransformer> CreateTransformers()
        {
            var transformerNames = Transformers.Split(';');

            foreach (var typeName in transformerNames)
            {
                var transformer = typeName.CreateInstance<IMessageTransformer>();
                yield return transformer;
            }
        }
    }
}