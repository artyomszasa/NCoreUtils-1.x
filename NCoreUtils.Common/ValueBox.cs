using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NCoreUtils
{
  /// <summary>
  /// Contains helper methods for <see cref="T:NCoreUtils.ValueBox{T}" />.
  /// </summary>
  public static class ValueBox
  {
    /// <summary>
    /// Creates new instance of <see cref="T:NCoreUtils.ValueBox{T}" />.
    /// </summary>
    /// <param name="value">Value to store initially.</param>
    /// <returns>The created instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static ValueBox<T> Create<T>(T value) => new ValueBox<T>(value);

    /// <summary>
    /// Stpres value of the provided value box into the specified variable if value box is not <c>null</c>.
    /// </summary>
    /// <param name="valueBox">Value box to use.</param>
    /// <param name="value">Variable to return the value.</param>
    /// <returns>
    /// <c>true</c> if value box was not <c>null</c> and <paramref name="value" /> has been updated, <c>false</c>
    /// otherwise.
    /// </returns>
    [DebuggerStepThrough]
    public static bool TryGetValue<T>(this ValueBox<T> valueBox, out T value)
    {
      if (null == valueBox)
      {
        value = default(T);
        return false;
      }
      value = valueBox.Value;
      return true;
    }
  }
  /// <summary>
  /// Creates explicit reference to the specified value similarly to the F# reference cells, see
  /// https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/reference-cells.
  /// Useful to represent absence of the object when <c>null</c> is valid object.
  /// </summary>
  /// <typeparam name="T">Type of the underlying object.</typeparam>
  public sealed class ValueBox<T>
  {
    readonly T _value;

    /// <summary>
    /// Value stored in the value box.
    /// </summary>
    public T Value
    {
      [DebuggerStepThrough]
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return _value; }
    }

    /// <summary>
    /// Initialzes new instance of <see cref="T:NCoreUtils.ValueBox{T}" />.
    /// </summary>
    /// <param name="value">Value to store.</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueBox(T value)
    {
      _value = value;
    }
  }
}