using System;
using System.Reflection;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Defines generalized accessor representation, which can represent both field and property accessors and allows
  /// implementing custom accessors.
  /// </summary>
  public interface IAccessor : ICustomAttributeProvider
  {
    /// <summary>
    /// Gets a value indicating whether the accessor can be written to.
    /// </summary>
    bool CanWrite { get; }
    /// <summary>
    /// Gets a value indicating whether the accessor can be read.
    /// </summary>
    bool CanRead { get; }
    /// <summary>
    /// Gets the name of the represented member.
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Gets the class that declares the represented member, if any.
    /// </summary>
    Type DeclaringType { get; }
    /// <summary>
    /// Gets the type of the represented member.
    /// </summary>
    Type TargetType { get; }
    /// <summary>
    /// Gets the accessor type of the actual accessor.
    /// </summary>
    AccessorType AccessorType { get; }
    /// <summary>
    /// Gets whether the represented member has default value.
    /// </summary>
    bool HasDefaultValue { get; }
    /// <summary>
    /// Gets the default value for the represented member, if any.
    /// </summary>
    object DefaultValue { get; }
    /// <summary>
    /// Returns value of a specified object using the actual accessor defined logic.
    /// </summary>
    /// <param name="instance">The object whose value will be returned.</param>
    /// <returns> The value retrieved using the actual accessor defined logic.</returns>
    object GetValue(object instance);
    /// <summary>
    /// Sets value of a specified object using the actual accessor defined logic.
    /// </summary>
    /// <param name="instance">The object whose value will be set.</param>
    /// <param name="value">The new value.</param>
    void SetValue(object instance, object value);
  }
}