using System;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Defines accessor extension which allowes paraméterized access.
  /// </summary>
  public interface IParameterizedAccessor : IAccessor
  {
    /// <summary>
    /// Returns value of a specified object using the actual accessor defined logic.
    /// </summary>
    /// <param name="instance">The object whose value will be returned.</param>
    /// <param name="serviceProvider">Service provider to use.</param>
    /// <returns> The value retrieved using the actual accessor defined logic.</returns>
    object GetValue(object instance, IServiceProvider serviceProvider);
    /// <summary>
    /// Sets value of a specified object using the actual accessor defined logic.
    /// </summary>
    /// <param name="instance">The object whose value will be set.</param>
    /// <param name="serviceProvider">Service provider to use.</param>
    /// <param name="value">The new value.</param>
    void SetValue(object instance, IServiceProvider serviceProvider, object value);
  }
}