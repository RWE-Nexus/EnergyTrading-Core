namespace EnergyTrading.Container.Unity
{
    using System;
    using System.ServiceModel;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Unity WCF service host
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {
        public UnityServiceHost(Type serviceType, IUnityContainer container, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        public IUnityContainer Container { get; set; }

        protected override void OnOpening()
        {
            if (this.Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                this.Description.Behaviors.Add(new UnityServiceBehavior(this.Container));
            }

            base.OnOpening();
        }
    }
}