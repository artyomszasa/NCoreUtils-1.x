using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NCoreUtils.Pooling
{
  /// <summary>
  /// Simple thread-safe object pool. Instances are created on demand without any limitation.
  /// </summary>
  public class ConcurrentObjectPool<T> : IObjectPool<T>
  {
    readonly ConcurrentQueue<T> _pool = new ConcurrentQueue<T>();
    /// <summary>
    /// Underlying insance of <see cref="T:System.Collections.Concurrent.ConcurrentQueue{T}"/>.
    /// </summary>
    protected ConcurrentQueue<T> Pool
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      [DebuggerStepThrough]
      get => _pool;
    }
    /// <summary>
    /// When overridden allows redefining how the instances are created.
    /// </summary>
    /// <returns>The instance.</returns>
    protected virtual T CreateInstance() => Activator.CreateInstance<T>();
    /// <summary>
    /// When overridden allows item clean up before returning to the pool. By default does nothing.
    /// </summary>
    /// <param name="item">Item to clean up.</param>
    /// <returns>Cleaned up item.</returns>
    protected virtual T CleanUp(T item) => item;
    /// <summary>
    /// Return an instance to the pool without checking parameters. All checks must be performed by caller.
    /// </summary>
    /// <param name="item">Item to return.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ReturnNoCheck(T item)
      => _pool.Enqueue(CleanUp(item));
    /// <summary>
    /// Either retrieves an instance from the pool or creates a new one.
    /// </summary>
    public virtual T Take()
      => _pool.TryDequeue(out var item) ? item : CreateInstance();
    /// <summary>
    /// Return an instance to the pool.
    /// </summary>
    /// <param name="item">Item to return.</param>
    public virtual void Return(T item)
    {
      RuntimeAssert.ArgumentNotNull(item, nameof(item));
      ReturnNoCheck(item);
    }
  }
}