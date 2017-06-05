using System;

namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Overrides default object factory for the target class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class ObjectFactoryAttribute : Attribute
    {
        /// <summary>
        /// Type of the object factory to use.
        /// </summary>
        public readonly Type ObjectFactoryType;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.ObjectFactoryAttribute" /> with
        /// the specified object factory type.
        /// </summary>
        /// <param name="objectFactoryType">Object factory type to use.</param>
        public ObjectFactoryAttribute(Type objectFactoryType)
        {
            RuntimeAssert.ArgumentNotNull(objectFactoryType, nameof(objectFactoryType));
            ObjectFactoryType = objectFactoryType;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.ObjectFactoryAttribute" /> with
        /// the specified object factory type name.
        /// </summary>
        /// <param name="objectFactoryTypeName">Object factory type name to use.</param>
        public ObjectFactoryAttribute(string objectFactoryTypeName) : this(Type.GetType(objectFactoryTypeName, true)) { }
    }
}