namespace EnergyTrading.Polling
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Timers;

    using EnergyTrading.Logging;

    using Timer = System.Timers.Timer;

    public class PollProcessor : IPollProcessor
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private PollProcessorEndpoint Endpoint { get; set; }

        private IPoller Poller { get; set; }

        private readonly Timer timer;
        private int syncPoint;

        public PollProcessor(PollProcessorEndpoint endpoint, IPoller poller)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (poller == null)
            {
                throw new ArgumentNullException("poller");
            }

            this.Endpoint = endpoint;
            this.Poller = poller;

            this.timer = new Timer(this.Endpoint.IntervalSecs * 1000) { AutoReset = true };
            this.timer.Elapsed += this.TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // prevent timer event from running if there is one already going
            var sync = Interlocked.CompareExchange(ref this.syncPoint, 1, 0);
            if (sync != 0)
            {
                return;
            }

            try
            {
                this.Poller.Poll();
            }
            catch (Exception exception)
            {
                Logger.Error("Exception in poll processor", exception);
            }
            this.syncPoint = 0;
        }

        public void Start()
        {
            Logger.Info("Starting");
            this.timer.Start();
            Logger.Info("Started");
        }

        public void Stop()
        {
            Logger.Info("Stopping");
            // If elapsed is running try to let it finish
            var count = 0;
            var isRunning = true;
            while (count < 20 && isRunning)
            {
                if (this.syncPoint == 0)
                {
                    isRunning = false;
                }
                else
                {
                    Thread.Sleep(500);
                    ++count;
                }
            }
            // done waiting stop the timer anyway
            this.timer.Stop();
            Logger.Info("Stopped");
        }

        public string Name
        {
            get
            {
                return this.Endpoint.Name;
            }
        }
    }
}
