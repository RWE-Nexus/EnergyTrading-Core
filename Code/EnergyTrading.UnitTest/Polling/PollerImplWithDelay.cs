namespace EnergyTrading.UnitTest.Polling
{
    using System.Threading;
    using EnergyTrading.Polling;

    public class PollerImplWithDelay : IPoller
    {
        private readonly int delay;

        public PollerImplWithDelay(int delay)
        {
            this.delay = delay;
            this.Count = 0;
        }

        public void Poll()
        {
            ++this.Count;
            Thread.Sleep(this.delay);
        }

        public void Reset()
        {
            this.Count = 0;
        }

        public int Count { get; private set; }
    }
}
