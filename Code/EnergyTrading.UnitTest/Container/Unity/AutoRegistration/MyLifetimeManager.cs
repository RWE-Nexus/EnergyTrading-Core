namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    using System;
    using Microsoft.Practices.Unity;

    internal class MyLifetimeManager : LifetimeManager
    {
        public override object GetValue()
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object newValue)
        {
            throw new NotImplementedException();
        }

        public override void RemoveValue()
        {
            throw new NotImplementedException();
        }
    }
}
