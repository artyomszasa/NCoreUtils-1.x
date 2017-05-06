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
      if (accessor.HasDefaultValue)
      {
        value = accessor.DefaultValue;
        return true;
      }
      value = default(object);
      return false;
    }
  }
}