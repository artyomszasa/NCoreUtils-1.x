using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality for serializing to generic source.
    /// </summary>
    public interface ISerializer<T>
    {
        /// <summary>
        /// Serializes the specified object to the generic output using specified service provider.
        /// </summary>
        /// <param name="target">Target output.</param>
        /// <param name="obj">Object to serialize.</param>
        void Serialize(T target, object obj);
    }
}