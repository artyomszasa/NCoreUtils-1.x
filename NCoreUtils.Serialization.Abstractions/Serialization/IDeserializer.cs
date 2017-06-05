using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality for deserializing from generic source.
    /// </summary>
    public interface IDeserializer<T>
    {
        /// <summary>
        /// Deserializes object of the specified type from specified generic source using specified service provider.
        /// </summary>
        /// <param name="source">Input source.</param>
        /// <param name="type">Type of the deserialized object.</param>
        /// <returns>Deserialized object.</returns>
        /// <exception cref="T:NCoreUtils.Serialization.SerializationException">
        /// Thrown if serialization fails.
        /// </exception>
        object Deserialize(T source, Type type);
    }
}