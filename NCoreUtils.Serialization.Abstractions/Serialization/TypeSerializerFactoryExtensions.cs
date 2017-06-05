using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Contains extensions for type serializer factory.
    /// </summary>
    public static class TypeSerializerFactoryExtensions
    {
        /// <summary>
        /// Retrieves typed type serializer.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="factory">Source factory.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Typed type serializer for <typeparamref name="T" /></returns>
        public static TypeSerializer<T> GetSerializer<T>(this ITypeSerializerFactory factory, IServiceProvider serviceProvider)
            => factory.GetSerializer(typeof(T), serviceProvider).AsTyped<T>();
    }
}