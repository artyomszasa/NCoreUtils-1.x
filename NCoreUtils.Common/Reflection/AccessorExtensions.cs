using System;
using System.Collections.Generic;
using System.Linq;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Extensions for <see cref="T:NCoreUtils.Reflection.IAccessor" />.
  /// </summary>
  public static class AccessorExtensions
  {
    /// <summary>
    /// Implements null-safe way to retrieve default value from the accessor, if any.
    /// </summary>
    /// <param name="accessor">The accessor to use.</param>
    /// <param name="value">Variable to store default value if present.</param>
    /// <returns>
    /// <c>true</c> if the accessor has default value and it has been stored in the specified variable, <c>false</c>
    /// otherwise.
    /// </returns>
    public static bool TryGetDefaultValue(this IAccessor accessor, out object value)
    {
      RuntimeAssert.ArgumentNotNull(accessor, nameof(accessor));
      if (accessor.HasDefaultValue)
      {
        value = accessor.DefaultValue;
        return true;
      }
      value = default(object);
      return false;
    }
    /// <summary>
    /// Retrieves a collection of custom attributes of a specified type that are applied to a specified accessor.
    /// </summary>
    /// <typeparam name="T">The type of attribute to search for.</typeparam>
    /// <param name="accessor">An accessor to inspect.</param>
    /// <returns>
    /// A collection of the custom attributes that are applied to element and that match <typeparamref name="T" />, or
    /// an empty collection if no such attributes exist.
    /// </returns>
    public static IEnumerable<T> GetCustomAttributes<T>(this IAccessor accessor) where T : Attribute
    {
      return accessor.GetCustomAttributes(typeof(T), true).Cast<T>();
    }
  }
}