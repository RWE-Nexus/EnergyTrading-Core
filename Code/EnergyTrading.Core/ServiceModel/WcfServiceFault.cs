namespace EnergyTrading.ServiceModel
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class WcfServiceFault
    {
        public WcfServiceFault Inner { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string Target { get; set; }

        public override string ToString()
        {
            return this.Inner == null ? 
                   string.Format("Target: {0} / Source: {1} / Message: {2}", this.Target, this.Source, this.Message) : 
                   string.Format("Target: {0} / Source: {1} / Message: {2}{3}/ Inner: {4}", this.Target, this.Source, this.Message, Environment.NewLine, this.Inner.ToString());
        }
    }
}