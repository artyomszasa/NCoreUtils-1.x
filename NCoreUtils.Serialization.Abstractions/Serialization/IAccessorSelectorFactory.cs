using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to create accessor selectors.
    /// </summary>
    public interface IAccessorSelectorFactory
    {
        /// <summary>
        /// Initializes and returns an accessor selector.
        /// </summary>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Accessor selector.</returns>
        IAccessorSelector Create(IServiceProvider serviceProvider);
    }
    /// <summary>
    /// Defines functionality to create accessor selectors for the specified type. Implementations intended to be used
    /// as singleton services.
    /// </summary>
    public interface IAccessorSelectorFactory<T> : IAccessorSelectorFactory { }
}