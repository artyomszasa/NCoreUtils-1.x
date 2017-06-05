using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Default accessor selector.
    /// </summary>
    public class DefaultAccessorSelector : IAccessorSelector
    {
        static readonly IEnumerable<IAccessor> _empty = new IAccessor[0];
        /// <summary>
        /// Gets whether fields are collected during the accessor selection.
        /// </summary>
        public bool IncludeFields { get; private set; }
        /// <summary>
        /// Gets whether properties are collected during the accessor selection.
        /// </summary>
        public bool IncludeProperties { get; private set; }
        /// <summary>
        /// Gets binding flags value to be used during the accessor selection.
        /// </summary>
        public BindingFlags BindingFlags { get; private set; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.DefaultAccessorSelector" /> with the specified parameters.
        /// </summary>
        /// <param name="includeFields">Whether fields are to be collected during the accessor selection.</param>
        /// <param name="includeProperties">Whether properties are to be collected during the accessor selection.</param>
        /// <param name="bindingFlags">Binding flags value to be used during the accessor selection.</param>
        public DefaultAccessorSelector(bool includeFields = true, bool includeProperties = true, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            IncludeFields = includeFields;
            IncludeProperties = includeProperties;
            BindingFlags = bindingFlags;
        }
        /// <summary>
        /// Collects all fields to be serialized.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Collection of fields that should be serialized.</returns>
        protected virtual IEnumerable<FieldInfo> CollectFields(Type type)
            => type.GetTypeInfo()
                .GetFields(BindingFlags)
                .Where(f => (FieldAttributes)0 == (f.Attributes & (FieldAttributes.InitOnly | FieldAttributes.Literal)));
        /// <summary>
        /// Collects all properties to be serialized.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Collection of properties that should be serialized.</returns>
        protected virtual IEnumerable<PropertyInfo> CollectProperties(Type type)
            => type.GetTypeInfo()
                .GetProperties(BindingFlags)
                .Where(p => p.CanRead && p.CanWrite);
        /// <summary>
        /// Collects all custom accessors to be serialized.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Collection of custom accessors that should be serialized.</returns>
        protected virtual IEnumerable<IAccessor> CollectCustomAccessors(Type type) => _empty;
        /// <summary>
        /// Collects all accessors to be serialized.
        /// </summary>
        /// <param name="type">Source type.</param>
        /// <returns>Collection of accessors that should be serialized.</returns>
        public virtual IEnumerable<IAccessor> GetSerializableAccessors(Type type)
        {
            var accessors = CollectCustomAccessors(type);
            if (IncludeFields)
            {
                accessors = accessors.Concat(CollectFields(type).Select(MemberAccessor.Create));
            }
            if (IncludeProperties)
            {
                accessors = accessors.Concat(CollectProperties(type).Select(MemberAccessor.Create));
            }
            return accessors;
        }
    }
}