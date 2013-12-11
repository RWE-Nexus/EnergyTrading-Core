namespace EnergyTrading.ProducerConsumer
{
    using System;
    using System.Threading;

    public abstract class Disposable : IDisposable
    {
        private int disposed;

        protected Disposable()
        {
            disposed = 0;
        }

        public bool IsDisposed { get { return disposed == 1; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0)
            {
                return;
            }

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();
        }

        /// <summary>
        /// Helper method so subclasses can easily throw if disposed
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
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