namespace NCoreUtils.Collections
{
  /// <summary>
  /// Defines non-enumerable buffer functionality. 
  /// </summary>
  public interface IBuffer<T> : IReadOnlyBuffer<T>
  {
    /// <summary>
    /// Sets value at specified index.
    /// </summary>
    new T this[int index] { set; }
  }
}