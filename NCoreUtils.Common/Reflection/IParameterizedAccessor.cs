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
    /// <param name="state">User defined state to use.</param>
    /// <returns> The value retrieved using the actual accessor defined logic.</returns>
    object GetValue(object instance, object state);
    /// <summary>
    /// Sets value of a specified object using the actual accessor defined logic.
    /// </summary>
    /// <param name="instance">The object whose value will be set.</param>
    /// <param name="state">User defined state to use.</param>
    /// <param name="value">The new value.</param>
    void SetValue(object instance, object state, object value);
  }
}