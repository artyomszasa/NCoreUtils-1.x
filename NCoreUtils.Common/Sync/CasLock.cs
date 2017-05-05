using System;
using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Lightweight CAS (compare-and-swap) based lock. Implies no thread management.
  /// </summary>
  public sealed class CasLock : ILockAsync
  {
    /// <summary>
    /// Thrown when the target CAS lock contains invalid state. May not happen until some unexpected
    /// direct manipulation occures on the internal state of the lock.
    /// </summary>
    public class OutOfSyncException : Exception
    {
      /// <summary>
      /// Initializes new instance of <see cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException"/> without explicit error description.
      /// </summary>
      public OutOfSyncException() : base() { }

      /// <summary>
      /// Initializes new instance of <see cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException"/> with explicit error description.
      /// </summary>
      /// <param name="message">Error description.</param>
      public OutOfSyncException(string message) : base(message) { }
    }

    const int Locked = 1;
    const int Free = 0;

    int _value = Free;

    /// <summary>
    /// Tries to acquire lock implemented by the synchronization object. This method should return as soon as possible.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    public Task<bool> TryLockAsync(CancellationToken cancellationToken) => Task.Run(() => TryLock(), cancellationToken);

    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks until lock has been acquired
    /// and throws exception if lock cannot be acqured.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task LockAsync(CancellationToken cancellationToken)
    {
      return Task.Run(delegate () {
        while (!TryLock())
        {
          cancellationToken.ThrowIfCancellationRequested();
        }
      }, cancellationToken);
    }

    /// <summary>
    /// Tries to acquire lock implemented by the synchronization object. This method returns immidiately.
    /// </summary>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    /// <exception cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException">Thrown if the instance contains invalid state.</exception>
    public bool TryLock()
    {
      switch (Interlocked.CompareExchange(ref _value, Locked, Free))
      {
        case Free:
          return true;
        case Locked:
          return false;
        default:
          throw new OutOfSyncException();
      }
    }

    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks uses busy waiting until the lock has been acuired.
    /// and throws exception if lock cannot be acqured.
    /// </summary>
    public void Lock()
    {
      while (TryLock());
    }

    /// <summary>
    /// Releases lock implemented by the synchronization object. This method returns immidiately.
    /// </summary>
    /// <exception cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException">Thrown if the instance contains invalid state.</exception>
    /// <exception cref="T:System.InvalidOperationException">Thrown if the instance is not currently locked.</exception>
    public void Release()
    {
      switch (Interlocked.CompareExchange(ref _value, Free, Locked))
      {
        case Free:
          throw new InvalidOperationException("Not locked");
        case Locked:
          return;
        default:
          throw new OutOfSyncException();
      }
    }

    void IDisposable.Dispose() { }
  }
}