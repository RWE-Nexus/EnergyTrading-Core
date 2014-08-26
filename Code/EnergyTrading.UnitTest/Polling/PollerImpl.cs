namespace EnergyTrading.UnitTest.Polling
{
    using System;

    using EnergyTrading.Logging;
    using EnergyTrading.Polling;

    public class PollerImpl : IPoller
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(typeof(PollerImpl));

        public PollerImpl()
        {
            this.Count = 0;
            this.ThrowsInPoll = false;
        }

        public PollerImpl ShouldThrow()
        {
            this.ThrowsInPoll = true;
            return this;
        }

        public void Poll()
        {
            Logger.Info("PollerImpl.Poll was called");
            ++this.Count;
            ++PollCount;
            if (this.ThrowsInPoll)
            {
                throw new InvalidOperationException();
            }
        }

        public int Count { get; private set; }

        public static int PollCount { get; set; }

        private bool ThrowsInPoll { get; set; }

        public void ResetCount()
        {
            this.Count = 0;
        }
    }
}
