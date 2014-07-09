namespace EnergyTrading.Container.Unity
{
    using System.Web;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Factory for creating call context lifetime managers.
    /// </summary>
    public static class CallContextLifetimeFactory
    {
        /// <summary>
        /// Gets a new <see cref="LifetimeManager"/>
        /// </summary>
        /// <returns>A new <see cref="WebCallContextLifetimeManager"/> if we are running in a web application, otherwise a new <see cref="CallContextLifetimeManager"/></returns>
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