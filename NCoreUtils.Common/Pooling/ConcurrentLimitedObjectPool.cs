using System;

namespace NCoreUtils.Pooling
{
  /// <summary>
  /// Simple thread-safe object pool. Instances are created on demand without any limitation, only the amount of
  /// preserved (i.e. stored in pool) items is limited. If the limit is reached any call to return method will
  /// destroy the returned object.
  /// </summary>
  public class ConcurrentLimitedObjectPool<T> : ConcurrentObjectPool<T>
  {
    /// <summary>
    /// Gets or sets maximum amount of preserved objects. Setting value does not affect already created objects,
    /// this value is only checked when returning objects to the pool.
    /// </summary>
    public int MaxItemsPreserved { get; set; }
    /// <summary>
    /// Initializes new instance of <see cref="T:NCoreUtils.Pooling.ConcurrentLimitedObjectPool{T}" /> with
    /// specified maximum amount of preserved objects.
    /// </summary>
    /// <param name="maxItemsPreserved">Maximum amount of preserved objects</param>
    public ConcurrentLimitedObjectPool(int maxItemsPreserved)
    {
      MaxItemsPreserved = maxItemsPreserved;
    }
    /// <summary>
    /// Destroys the specified object. This method is called when the pool is "full" (i.e. maximum preserved objects
    /// limit has been hit). By default disposes objects that implement <see cref="T:System.IDisposable" />
    /// interface and does nothing for other objects. Method can be overridden to adjust this functionality.
    /// </summary>
    protected virtual void Dispose(T item)
    {
      if (item is IDisposable disposable)
      {
        disposable.Dispose();
      }
    }
    /// <summary>
    /// Return an instance to the pool.
    /// </summary>
    /// <param name="item">Item to return.</param>
    public override void Return(T item)
    {
      if (Pool.Count >= MaxItemsPreserved)
      {
        RuntimeAssert.ArgumentNotNull(item, nameof(item));
        Dispose(item);
      }
      else
      {
        base.ReturnNoCheck(item);
      }
    }
  }
}