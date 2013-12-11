namespace EnergyTrading.Threading
{
    using System.Threading;

    /// <summary>
    /// Abstraction over <see cref="T:System.Threading.CancellationTokenSource">CancellationTokenSource</see> that is can be reset.
    /// </summary>
    public interface ICancellationTokenOwner
    {
        /// <summary>
        /// Gets the <see cref="System.Threading.CancellationToken">CancellationToken</see> 
        /// associated with this <see cref="ICancellationTokenOwner"/>. 
        /// </summary>
        CancellationToken Token { get;  }

        /// <summary> 
        /// Gets whether cancellation has been requested for this <see cref="ICancellationTokenOwner">ICancellationTokenOwner</see>.
        /// </summary>
        /// <value>Whether cancellation has been requested for this <see cref="ICancellationTokenOwner">ICancellationTokenOwner</see>.</value>
        /// <remarks> 
        /// <para> 
        /// This property indicates whether cancellation has been requested for this token source, e.g.
        /// due to a call to its <see cref="ICancellationTokenOwner.Cancel()">Cancel</see> method.
        /// </para>
        /// <para>
        /// If this property returns true, it only guarantees that cancellation has been requested. It does not 
        /// guarantee that every handler registered with the corresponding token has finished executing, nor
        /// that cancellation requests have finished propagating to all registered handlers. Additional 
        /// synchronization may be required, particularly in situations where related objects are being 
        /// canceled concurrently.
        /// </para> 
        /// </remarks>
        bool IsCancellationRequested { get; }

        /// <summary>
        /// Communicates a request for cancellation.
        /// </summary> 
        /// <remarks>
        /// <para> 
        /// The associated <see cref="T:System.Threading.CancellationToken" /> will be 
        /// notified of the cancellation and will transition to a state where
        /// <see cref="System.Threading.CancellationToken.IsCancellationRequested">IsCancellationRequested</see> returns true. 
        /// Any callbacks or cancelable operations
        /// registered with the <see cref="T:System.Threading.CancellationToken"/>  will be executed.
        /// </para>
        /// <para> 
        /// Cancelable operations and callbacks registered with the token should not throw exceptions.
        /// However, this overload of Cancel will aggregate any exceptions thrown into a <see cref="System.AggregateException"/>, 
        /// such that one callback throwing an exception will not prevent other registered callbacks from being executed. 
        /// </para>
        /// <para> 
        /// The <see cref="T:System.Threading.ExecutionContext"/> that was captured when each callback was registered
        /// will be reestablished when the callback is invoked.
        /// </para>
        /// </remarks> 
        /// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown
        /// by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken"/>.</exception> 
        /// <exception cref="T:System.ObjectDisposedException">This <see 
        /// cref="T:System.Threading.CancellationTokenSource"/> has been disposed.</exception>
        void Cancel();

        /// <summary>
        /// Resets the cancellation token owner, subsequent calls to <see cref="Token" /> will return a new value.
        /// </summary>
        /// <remarks>
        /// Will throw an exception if called before Cancel is invoke - this is to avoid losing the
        /// underlying CancellationTokenSource which is not make public.
        /// </remarks>
        void Reset();
    }
}