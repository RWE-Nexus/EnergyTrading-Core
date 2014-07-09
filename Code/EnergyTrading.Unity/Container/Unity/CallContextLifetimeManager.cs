namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Runtime.Remoting.Messaging;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Contextual lifetime manager.
    /// <para>
    /// Uses <see cref="CallContext" /> to store values.
    /// </para>
    /// </summary>
    public class CallContextLifetimeManager : LifetimeManager
    {
        private readonly string key = string.Format("CallContextLifeTimeManager_{0}", Guid.NewGuid());

        /// <copydocfrom cref="LifetimeManager.GetValue" />
        public override object GetValue()
        {
            return CallContext.GetData(key);
        }

        /// <copydocfrom cref="LifetimeManager.GetValue" />
        public override void SetValue(object newValue)
        {
            CallContext.SetData(key, newValue);
        }

        /// <copydocfrom cref="LifetimeManager.GetValue" />
        public override void RemoveValue()
        {
            CallContext.FreeNamedDataSlot(key); 
        } 
    }
}