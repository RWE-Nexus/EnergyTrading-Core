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
            get { return source.Token; }
        }

        /// <inheritdoc />
        public bool IsCancellationRequested
        {
            get { return source.IsCancellationRequested; }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            source.Cancel();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown if the underlying CancellationTokenSource has not been cancelled.</exception>
        public void Reset()
        {
            if (source != null)
            {
                if (!source.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Must issue Cancel before Reset");
                }
            }

            source = new CancellationTokenSource();
        }
    }
}