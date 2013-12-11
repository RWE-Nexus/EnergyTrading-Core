namespace EnergyTrading.Container.Unity
{
    using System.Threading;

    using Microsoft.Practices.Unity;

    /// <summary> 
    /// Replaces <see cref="UnityDefaultBehaviorExtension"/> to eliminate  <see cref="SynchronizationLockException"/> 
    /// exceptions that would otherwise occur when using <c>RegisterInstance</c>.  
    /// </summary>
    /// <remarks>Not needed for 2.1.505.2 and above as bug has been fixed.</remarks>
    public class UnitySafeBehaviorExtension : UnityDefaultBehaviorExtension
    {
        protected override void Initialize()
        {
            Context.RegisteringInstance += PreRegisteringInstance;
            base.Initialize();
        }

        /// <summary>
        /// Handles the <see cref="ExtensionContext.RegisteringInstance"/> event by
        /// ensuring that, if the lifetime manager is a  <see cref="SynchronizedLifetimeManager"/> that its
        /// <see cref="SynchronizedLifetimeManager.GetValue"/> method has been called.
        /// </summary> 
        /// <param name="sender">The object responsible for raising the event.</param>
        /// <param name="e">A <see cref="RegisterInstanceEventArgs"/> containing the event's data.</param>     
        private void PreRegisteringInstance(object sender, RegisterInstanceEventArgs e)
        {
            if (e.LifetimeManager is SynchronizedLifetimeManager)
            {
                e.LifetimeManager.GetValue();
            }
        }
    }
}