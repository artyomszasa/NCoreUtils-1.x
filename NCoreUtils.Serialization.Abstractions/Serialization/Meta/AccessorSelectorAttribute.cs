using System;
using System.Reflection;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Allows overwriting accessor selector factory for the target class or struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class AccessorSelectorAttribute : Attribute
    {
        /// <summary>
        /// Accessor selector factory type to use.
        /// </summary>
        public readonly Type SelectorFactoryType;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.AccessorSelectorAttribute" />
        /// with specified type.
        /// </summary>
        /// <param name="selectorFactoryType">Accessor selector type.</param>
        public AccessorSelectorAttribute(Type selectorFactoryType)
        {
            SelectorFactoryType = selectorFactoryType;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Meta.AccessorSelectorAttribute" />
        /// with specified type name.
        /// </summary>
        /// <param name="selectorFactoryTypeName">Accessor selector type name.</param>
        public AccessorSelectorAttribute(string selectorFactoryTypeName) : this(Type.GetType(selectorFactoryTypeName, true)) { }
    }
}
