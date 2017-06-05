using System;
using System.Collections.Generic;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Provides easy method to create derived accessor selectors.
    /// </summary>
    public abstract class DerivedAccessorSelector : IAccessorSelector
    {
        /// <summary>
        /// Gets reference to the inherited accessor selector.
        /// </summary>
        /// <returns></returns>
        public IAccessorSelector BaseAccessorSelector { get; internal set; }
        /// <summary>
        /// Initilizes new instance of <see cref="T:NCoreUtils.Serialization.DerivedAccessorSelector" />.
        /// </summary>
        protected DerivedAccessorSelector() { }
        /// <summary>
        /// Can be overridden to adjust output of the inherited accessor selector.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Collection of accessors that should be serialized.</returns>
        public virtual IEnumerable<IAccessor> GetSerializableAccessors(Type type) => BaseAccessorSelector.GetSerializableAccessors(type);
    }
}