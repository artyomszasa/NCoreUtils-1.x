using System;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Extensions for <see cref="T:NCoreUtils.Reflection.AccessorType" />.
  /// </summary>
  public static class AccessorTypeExtensions
  {
    /// <summary>
    /// Performs pattern matching on the actual instance of <see cref="T:NCoreUtils.Reflection.AccessType" />.
    /// </summary>
    /// <param name="accessorType">Instance to use.</param>
    /// <param name="defaultValue">Value to return if any other handler is not specified.</param>
    /// <param name="onProperty">Value factory to invoke if the actual instance represents property option.</param>
    /// <param name="onField">Value factory to invoke if the actual instance represents field option.</param>
    /// <param name="onCustom">Value factory to invoke if the actual instance represents custom option.</param>
    public static T Match<T>(this AccessorType accessorType, T defaultValue, Func<T> onProperty = null, Func<T> onField = null, Func<T> onCustom = null)
      => accessorType.Match(onProperty, onField, onCustom, () => defaultValue);
  }
}