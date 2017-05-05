namespace NCoreUtils.Collections
{
  /// <summary>
  /// Defines non-enumerable read-only buffer functionality. 
  /// </summary>
  public interface IReadOnlyBuffer<T>
  {
    /// <summary>
    /// Gets buffer size.
    /// </summary>    
    int Count { get; }
    /// <summary>
    /// Gets value at specified index.
    /// </summary>
    T this[int index] { get; }
  }
}