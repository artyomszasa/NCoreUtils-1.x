using System.IO;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Contains serialzer extensions.
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// Deserializes object from stream.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="serializer"></param>
        /// <param name="source">Input stream.</param>
        /// <returns>Deserialized object.</returns>
        public static T Deserialize<T>(this Serializer serializer, Stream source)
            => (T)serializer.Deserialize(source, typeof(T));
    }
}