using System;

namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Overrides default type serializer for target class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class TypeSerializerAttribute : Attribute
    {
        /// <summary>
        /// Type of type serializer to use.
        /// </summary>
        public readonly Type TypeSerializerType;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.TypeSerializerAttribute" /> with the
        /// specified type of the type serializer.
        /// </summary>
        /// <param name="typeSerializerType">Type of the type serializer to use.</param>
        public TypeSerializerAttribute(Type typeSerializerType)
        {
            TypeSerializerType = typeSerializerType;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.TypeSerializerAttribute" /> with the
        /// specified type name of the type serializer.
        /// </summary>
        /// <param name="typeSerializerTypeName">Type name of the type serializer to use.</param>
        public TypeSerializerAttribute(string typeSerializerTypeName) : this(Type.GetType(typeSerializerTypeName, true)) { }
    }
}