namespace EnergyTrading.ServiceModel.Channels
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Syndication;
    using System.Xml;

    /// <summary>
    /// A <see cref="BodyWriter"/> that can handle syndication feeds.
    /// </summary>
    public class FeedBodyWriter : BodyWriter
    {
        private readonly SyndicationFeedFormatter formatter;

        public FeedBodyWriter(SyndicationFeedFormatter formatter) : base(false)
        {
            this.formatter = formatter;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            this.formatter.WriteTo(writer);
        }
    }
}