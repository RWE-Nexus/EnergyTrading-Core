namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Web;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// Contextual lifetime manager for use by ASP.NET.
    /// <para>
    /// Differs from <see cref="CallContextLifetimeManager" /> as we use <see cref="HttpContext.Current" /> to hold object references.
    /// This is required as ASP.NET can use multiple threads for a single request under heavy load and
    /// so CallContext is not adequate.
    /// </para>
    /// </summary>
    public class WebCallContextLifetimeManager : LifetimeManager
    {
        private readonly string key = string.Format("WebCallContextLifeTimeManager_{0}", Guid.NewGuid());

        /// <copydocfrom cref="LifetimeManager.GetValue" />
        public override object GetValue()
        {
            return HttpContext.Current.Items[key];
        }

        /// <copydocfrom cref="LifetimeManager.SetValue" />
        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[key] = newValue;
        }

        /// <copydocfrom cref="LifetimeManager.RemoveValue" />
        public override void RemoveValue()
        {
            HttpContext.Current.Items[key] = null;
        }
    }
}