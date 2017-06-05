using System;

namespace NCoreUtils.Serialization.Meta
{
    /// <summary>
    /// Excludes type from serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class NotSerializableAttribute : Attribute { }
}
