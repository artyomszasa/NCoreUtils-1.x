namespace NCoreUtils.Pooling
{
  /// <summary>
  /// Common object pool interface.
  /// </summary>
  public interface IObjectPool<T>
  {
    /// <summary>
    /// Take an instance from the pool.
    /// </summary>
    T Take();
    /// <summary>
    /// Returns an instance to the pool.
    /// </summary>
    /// <param name="item">Instance to reuse.</param>
    void Return(T item);
  }
}