namespace EnergyTrading.Threading
{
    using System;
    using System.Threading;

    /// <inheritdoc />
    public class CancellationTokenOwner : ICancellationTokenOwner
    {
        private CancellationTokenSource source;

        public CancellationTokenOwner()
        {
            this.Reset();
        }

        /// <inheritdoc />
        public CancellationToken Token
        {
            get { return this.source.Token; }
        }

        /// <inheritdoc />
        public bool IsCancellationRequested
        {
            get { return this.source.IsCancellationRequested; }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            this.source.Cancel();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown if the underlying CancellationTokenSource has not been cancelled.</exception>
        public void Reset()
        {
            if (this.source != null)
            {
                if (!this.source.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Must issue Cancel before Reset");
                }
            }

            this.source = new CancellationTokenSource();
        }
    }
}