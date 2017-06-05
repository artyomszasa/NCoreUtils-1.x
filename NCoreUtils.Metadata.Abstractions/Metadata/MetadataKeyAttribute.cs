using System;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Allows autoinjecting properties from derived classes to raw metadata export.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MetadataKeyAttribute : Attribute
    {
        /// <summary>
        /// Metadata key to be used in raw representation.
        /// </summary>
        /// <returns></returns>
        public CaseInsensitive Key { get; private set; }
        MetadataKeyAttribute(CaseInsensitive key) => Key = key;
        /// <summary>
        /// Marks field or property to be included in raw metadata representation specifing key to be used.
        /// </summary>
        /// <param name="key">Metadata key to be used.</param>
        public MetadataKeyAttribute(string key) : this(CaseInsensitive.Create(key)) { }
    }
}