namespace EnergyTrading.ProducerConsumer
{
    using System;
    using System.Threading;

    public abstract class Disposable : IDisposable
    {
        private int disposed;

        protected Disposable()
        {
            this.disposed = 0;
        }

        public bool IsDisposed { get { return this.disposed == 1; } }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref this.disposed, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeManagedResources();
            }

            this.DisposeUnmanagedResources();
        }

        /// <summary>
        /// Helper method so subclasses can easily throw if disposed
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeUnmanagedResources()
        {
        }
    }
}