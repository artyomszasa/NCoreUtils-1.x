using System;
using System.Collections.Generic;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to select accessors to serialize from type.
    /// </summary>
    public interface IAccessorSelector
    {
        /// <summary>
        /// Selects accessors to serialize from specified type.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Collection of accessors to serialize.</returns>
        IEnumerable<IAccessor> GetSerializableAccessors(Type type);
    }
    /// <summary>
    /// Defines functionality to select accessors to serialize from particular type. Implementations intended to be used
    /// as singleton services.
    /// </summary>
    public interface IAccessorSelector<T> : IAccessorSelector { }

}