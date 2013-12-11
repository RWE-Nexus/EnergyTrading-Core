namespace EnergyTrading.Container
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// Try to get an instance of a class from the service locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static T TryGetInstance<T>(this IServiceLocator locator)
        {
            try
            {
                return locator.GetInstance<T>();
            }
            catch (Exception)
            {
                return default(T); 
            }
        }

        /// <summary>
        /// Try to get an instance of a class from the service locator.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locator"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T TryGetInstance<T>(this IServiceLocator locator, string key)
        {
            try
            {
                return locator.GetInstance<T>(key);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}