using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to instantiate type serializers.
    /// </summary>
    public interface ITypeSerializerFactory : IDisposable
    {
        /// <summary>
        /// Gets type serializer for the specified type.
        /// </summary>
        /// <param name="type">Type to get type serializer for.</param>
        /// <param name="serviceProvider">Service provider to use</param>
        /// <returns>Type serializer.</returns>
        TypeSerializer GetSerializer(Type type, IServiceProvider serviceProvider);
        /// <summary>
        /// Stringifies type name.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>String representation of the type name.</returns>
        string GetTypeName(Type type);
        /// <summary>
        /// Resolves type based on its string representation.
        /// </summary>
        /// <param name="typeName">String representation of the type name.</param>
        /// <returns>Resolved type.</returns>
        Type ResolveType(string typeName);

    }
}