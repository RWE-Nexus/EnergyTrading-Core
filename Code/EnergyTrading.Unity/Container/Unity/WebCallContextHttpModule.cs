namespace EnergyTrading.Container.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Microsoft.Practices.Unity.Utility;

    /// <summary>
    /// Implementation of the <see cref="IHttpModule" /> interface that provides support to the <see cref="WebCallContextLifetimeManager"/>, and
    /// enables it to dispose the instances after the HTTP request ends.
    /// </summary>
    /// <remarks>Base on http://unity.codeplex.com/SourceControl/latest#source/Unity.Mvc/Src/UnityPerRequestHttpModule.cs </remarks>
    public class WebCallContextHttpModule : IHttpModule
    {
        private static readonly object ModuleKey = new object();

        /// <summary>
        /// Disposes of the resources used by this module.
        /// </summary>
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            Guard.ArgumentNotNull(context, "context");
            context.EndRequest += OnEndRequest;
        }

        internal static object GetValue(object lifetimeManagerKey)
        {
            var dict = GetDictionary(HttpContext.Current);
            if (dict != null)
            {
                object obj;
                if (dict.TryGetValue(lifetimeManagerKey, out obj))
                {
                    return obj;
                }
            }

            return null;
        }

        internal static void SetValue(object lifetimeManagerKey, object value)
        {
            var dict = GetDictionary(HttpContext.Current);
            if (dict == null)
            {
                dict = new Dictionary<object, object>();

                HttpContext.Current.Items[ModuleKey] = dict;
            }

            dict[lifetimeManagerKey] = value;
        }

        private static Dictionary<object, object> GetDictionary(HttpContext context)
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext not available");
            }

            var dict = (Dictionary<object, object>)context.Items[ModuleKey];

            return dict;
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var dict = GetDictionary(app.Context);
            if (dict != null)
            {
                foreach (var dispoable in dict.Values.OfType<IDisposable>())
                {
                    dispoable.Dispose();
                }
            }
        }
    }
}