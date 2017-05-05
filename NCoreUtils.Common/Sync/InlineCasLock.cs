using System;
using System.Threading;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Lightweight CAS (compare-and-swap) based lock. Implies no thread management.
  /// <para>
  /// NOTE: InlineCasLock is a value type, for performance reasons. For this reason, you must be very careful not to
  /// accidentally copy an InlineCasLock instance, as the two instances (the original and the copy) would then be
  /// completely independent of one another, which would likely lead to erroneous behavior of the application. If a
  /// InlineCasLock instance must be passed around, it should be passed by reference rather than by value.
  /// </para>
  /// </summary>
  public struct InlineCasLock
  {
    /// Preferred way to create instances of <see cref="T:NCoreUtils.Sync.InlineCasLock" />.
    public static InlineCasLock Create()
    {
      InlineCasLock l;
      l._value = Free;
      return l;
    }

    const int ThreadBlockCycles = 1000;
    const int Locked = 1;
    const int Free = 0;

    int _value;

    /// <summary>
    /// Initializes the specified inline CAS lock.
    /// </summary>
    /// <param name="lock">Target lock.</param>
    public static void Init(ref InlineCasLock @lock) => @lock._value = Free;

    /// <summary>
    /// Determines whether inline CAS lock is in <c>Locked</c> state.
    /// </summary>
    /// <param name="lock">Target lock.</param>
    /// <returns>
    /// <c>true</c> if inline CAS lock is in <c>Locked</c> state, <c>false</c> otherwise.
    /// </returns>
    public static bool IsLocked(ref InlineCasLock @lock) => Locked == @lock._value;

    // /// <summary>
    // /// Tries to acquire lock implemented by the synchronization object. This method should return as soon as possible.
    // /// </summary>
    // /// <param name="cancellationToken">Cancellation token</param>
    // /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    // public Task<bool> TryLockAsync(CancellationToken cancellationToken) => Task.FromResult(TryLock());
    //
    // /// <summary>
    // /// Acquires lock implemented by the synchronization object. This method blocks until lock has been acquired
    // /// and throws exception if lock cannot be acqured.
    // /// </summary>
    // /// <param name="cancellationToken">Cancellation token</param>
    // public async Task LockAsync(CancellationToken cancellationToken)
    // {
    //   cancellationToken.ThrowIfCancellationRequested();
    //   for (var tryCount = 0; tryCount < ThreadBlockCycles; ++tryCount)
    //   {
    //     if (TryLock())
    //     {
    //       return;
    //     }
    //   }
    //   await LockAsync(cancellationToken).ConfigureAwait(false);
    // }

    /// <summary>
    /// Tries to acquire lock implemented by the synchronization object. This method returns immidiately.
    /// </summary>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    /// <exception cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException">Thrown if the instance contains invalid state.</exception>
    public static bool TryLock(ref InlineCasLock @lock)
    {
      switch (Interlocked.CompareExchange(ref @lock._value, Locked, Free))
      {
        case Free:
          return true;
        case Locked:
          return false;
        default:
          throw new CasLock.OutOfSyncException();
      }
    }

    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks uses busy waiting until the lock has been acuired.
    /// and throws exception if lock cannot be acqured.
    /// </summary>
    public static void Lock(ref InlineCasLock @lock)
    {
      while (!TryLock(ref @lock));
    }

    /// <summary>
    /// Releases lock implemented by the synchronization object. This method returns immidiately.
    /// </summary>
    /// <exception cref="T:NCoreUtils.Sync.CaseLock.OutOfSyncException">Thrown if the instance contains invalid state.</exception>
    /// <exception cref="T:System.InvalidOperationException">Thrown if the instance is not currently locked.</exception>
    public static void Release(ref InlineCasLock @lock)
    {
      switch (Interlocked.CompareExchange(ref @lock._value, Free, Locked))
      {
        case Free:
          throw new InvalidOperationException("Not locked");
        case Locked:
          return;
        default:
          throw new CasLock.OutOfSyncException();
      }
    }
  }
}