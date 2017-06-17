using System;
using System.Linq;
using System.Reflection;
using NCoreUtils.Collections;

namespace NCoreUtils
{
    /// <summary>
    /// Contains extension methods for invoking delegates with arguments resolved through service provider.
    /// </summary>
    public static class InvocationUtilities
    {
        /// <summary>
        /// Invokes specified delegate resolving arguments using the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <param name="delegate">Delegate to invoke.</param>
        /// <param name="explicitArguments">Explicit arguments.</param>
        /// <returns>Invocation result if any.</returns>
        public static object InvokeWith(this Delegate @delegate, IServiceProvider serviceProvider, params object[] explicitArguments)
        {
            var mInvoke = @delegate.GetType().GetTypeInfo().GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            if (null == mInvoke)
            {
                throw new InvalidOperationException($"Unable to determine delegate arguments.");
            }
            var arguments = mInvoke.GetParameters().Map(p => {
                var arg = explicitArguments.FirstOrDefault(value => null != value && p.ParameterType.GetTypeInfo().IsAssignableFrom(value.GetType())) ?? serviceProvider.GetService(p.ParameterType);
                if (null == arg)
                {
                    throw new InvalidOperationException($"Unable to resolve service for type '{p.ParameterType}' while attempting to invoke delegate.");
                }
                return arg;
            });
            return mInvoke.Invoke(@delegate, arguments);
        }
        /// <summary>
        /// Optimized version of <see cref="M:NCoreUtils.InvocationUtilities.InvokeWith" />.
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Invocation result.</returns>
        public static TResult InvokeWith<T, TResult>(this Func<T, TResult> func, IServiceProvider serviceProvider)
        {
            var arg = serviceProvider.GetService(typeof(T));
            if (null == arg)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T)}' while attempting to invoke delegate.");
            }
            return func((T)arg);
        }
        /// <summary>
        /// Optimized version of <see cref="M:NCoreUtils.InvocationUtilities.InvokeWith" />.
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Invocation result.</returns>
        public static TResult InvokeWith<T1, T2, TResult>(this Func<T1, T2, TResult> func, IServiceProvider serviceProvider)
        {
            var arg1 = serviceProvider.GetService(typeof(T1));
            var arg2 = serviceProvider.GetService(typeof(T2));
            if (null == arg1)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T1)}' while attempting to invoke delegate.");
            }
            if (null == arg2)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T2)}' while attempting to invoke delegate.");
            }
            return func((T1)arg1, (T2)arg2);
        }
        /// <summary>
        /// Optimized version of <see cref="M:NCoreUtils.InvocationUtilities.InvokeWith" />.
        /// </summary>
        /// <param name="func">Function to invoke.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Invocation result.</returns>
        public static TResult InvokeWith<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, IServiceProvider serviceProvider)
        {
            var arg1 = serviceProvider.GetService(typeof(T1));
            var arg2 = serviceProvider.GetService(typeof(T2));
            var arg3 = serviceProvider.GetService(typeof(T3));
            if (null == arg1)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T1)}' while attempting to invoke delegate.");
            }
            if (null == arg2)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T2)}' while attempting to invoke delegate.");
            }
            if (null == arg3)
            {
                 throw new InvalidOperationException($"Unable to resolve service for type '{typeof(T3)}' while attempting to invoke delegate.");
            }
            return func((T1)arg1, (T2)arg2, (T3)arg3);
        }
    }
}