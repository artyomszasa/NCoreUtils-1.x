using System;

namespace NCoreUtils
{
    /// <summary>
    /// Contains commonly used service provider extensions.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Attempts to get service of the specified type.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="service">Variable to store service implementation.</param>
        /// <returns>
        /// <c>true</c> if service was found and has been stored to the specified variable, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetService<T>(this IServiceProvider serviceProvider, out T service) where T : class
        {
            var svc = serviceProvider.GetService(typeof(T));
            if (null == svc)
            {
                service = default(T);
                return false;
            }
            service = (T)svc;
            return true;
        }
        /// <summary>
        /// Attempts to get service of the specified type.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="service">Variable to store service implementation.</param>
        /// <returns>
        /// <c>true</c> if service was found and has been stored to the specified variable, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetService(this IServiceProvider serviceProvider, Type serviceType, out object service)
        {
            var svc = serviceProvider.GetService(serviceType);
            if (null == svc)
            {
                service = default(object);
                return false;
            }
            service = svc;
            return true;
        }
    }
}