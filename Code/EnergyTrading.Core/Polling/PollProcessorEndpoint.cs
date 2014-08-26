namespace EnergyTrading.Polling
{
    using System;

    public class PollProcessorEndpoint
    {
        public string Name { get; set; }

        public int IntervalSecs { get; set; }

        public Type Handler { get; set; }

        public bool SinglePolling { get; set; }

        public int Workers { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new NotSupportedException("Must supply a name");
            }

            if (this.Handler == null)
            {
                throw new NotSupportedException("Must supply a handler type");
            }

            if (this.IntervalSecs <= 0)
            {
                throw new NotSupportedException("IntervalSecs must be greater than 0");
            }

            if (this.Workers <= 0)
            {
                throw new NotSupportedException("Workers must be greater than 0");
            }

            return true;
        }
    }
}
