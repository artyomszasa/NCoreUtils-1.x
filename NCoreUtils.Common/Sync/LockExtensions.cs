using System;
using System.Threading;
using System.Threading.Tasks;
using NCoreUtils.Async;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Contains extensions for the "lock" objects.
  /// </summary>
  public static class LockExtensions
  {
    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks until lock has been acquired
    /// and throws exception if lock cannot be acqured within specified amount of time.
    /// </summary>
    /// <param name="lock">Target lock.</param>
    /// <param name="timeout">Amount of time to acquire the lock within.</param>
    /// <returns>
    /// Task which is completed when lock has been acquired.
    /// </returns>
    public static async Task LockAsync(this ILockAsync @lock, TimeSpan timeout)
    {
      using (var cancellation = new CancellationTokenSource(timeout))
      {
        await @lock.LockAsync(cancellation.Token);
      }
    }

    /// <summary>
    /// Executes provided action as critical section, acquiring the specified lock, executing a statement, and then
    /// releasing the lock.
    /// </summary>
    /// <param name="lock">Synchronization object to use.</param>
    /// <param name="action">Action to execute.</param>
    public static void Synchronized(this ILock @lock, Action action)
    {
      @lock.Lock();
      try
      {
        action();
      }
      finally
      {
        @lock.Release();
      }
    }

    /// <summary>
    /// Executes provided function as critical section, acquiring the specified lock, executing a statement, and then
    /// releasing the lock.
    /// </summary>
    /// <param name="lock">Synchronization object to use.</param>
    /// <param name="func">Function to execute.</param>
    public static T Synchronized<T>(this ILock @lock, Func<T> func)
    {
      @lock.Lock();
      try
      {
        return func();
      }
      finally
      {
        @lock.Release();
      }
    }

    /// <summary>
    /// Executes provided function as critical section, acquiring the specified lock, executing a statement, and then
    /// releasing the lock.
    /// </summary>
    /// <param name="lock">Synchronization object to use.</param>
    /// <param name="action">Asynchronous action to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Task which is completed after the operation has been performed and the lock has been released.
    /// </returns>
    public static async Task SynchronizedAsync(this ILockAsync @lock, AsyncAction action, CancellationToken cancellationToken)
    {
      await @lock.LockAsync(cancellationToken);
      try
      {
        await action.InvokeAsync(cancellationToken);
      }
      finally
      {
        @lock.Release();
      }
    }

    /// <summary>
    /// Executes provided function as critical section, acquiring the specified lock, executing a statement, and then
    /// releasing the lock.
    /// </summary>
    /// <param name="lock">Synchronization object to use.</param>
    /// <param name="func">Asynchronous function to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Task which is completed after the operation has been performed and the lock has been released.
    /// </returns>
    public static async Task<T> SynchronizedAsync<T>(this ILockAsync @lock, AsyncFunc<T> func, CancellationToken cancellationToken)
    {
      await @lock.LockAsync(cancellationToken);
      try
      {
        return await func(cancellationToken);
      }
      finally
      {
        @lock.Release();
      }
    }
  }
}