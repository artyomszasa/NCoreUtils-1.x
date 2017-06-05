using System;
using System.Collections.Immutable;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to instantiate objects using given set of accessor/value pairs and service provider.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Instantiates object using given set of accessor/value pairs and service provider.
        /// </summary>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <param name="values">Set of accessor/value pairs.</param>
        /// <returns>Instance of the created object.</returns>
        object CreateObject(IServiceProvider serviceProvider, ImmutableDictionary<IAccessor, object> values);
    }
    /// <summary>
    /// Defines functionality to instantiate objects using given set of accessor/value pairs and service provider for
    /// the specific type. Implementations a re intended to be used as singleton services.
    /// </summary>
    public interface IObjectFactory<T> : IObjectFactory where T : class { }
}