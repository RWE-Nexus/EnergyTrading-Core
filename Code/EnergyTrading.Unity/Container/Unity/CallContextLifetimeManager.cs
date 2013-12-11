namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Runtime.Remoting.Messaging;

    using Microsoft.Practices.Unity;

    public class CallContextLifetimeManager : LifetimeManager
    {
        private readonly string key = string.Format("CallContextLifeTimeManager_{0}", Guid.NewGuid()); 

        public override object GetValue()
        {
            return CallContext.GetData(key);
        }

        public override void SetValue(object newValue)
        {
            CallContext.SetData(key, newValue);
        }

        public override void RemoveValue()
        {
            CallContext.FreeNamedDataSlot(key); 
        } 
    }
}