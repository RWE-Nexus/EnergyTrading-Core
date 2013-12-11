namespace EnergyTrading.Container.Unity
{
    using System.Web;

    using Microsoft.Practices.Unity;

    public static class CallContextLifetimeFactory
    {
        public static LifetimeManager Manager()
        {
            if (HttpContext.Current != null)
            {
                return new WebCallContextLifetimeManager();
            }
            
            return new CallContextLifetimeManager();
        }
    }
}