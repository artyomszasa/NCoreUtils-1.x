using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Wrapper around <see cref="T:System.Threading.SemaphoreSlim" /> object implmenting
  /// <see cref="T:NCoreUtils.ILock" /> and <see cref="T:NCoreUtils.ILockAsync" /> interfaces.
  /// </summary>
  public sealed class SemaphoreLock : ILockAsync
  {
    readonly SemaphoreSlim _sem = new SemaphoreSlim(1, 1);
    int _isDisposed;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowIfDisposed()
    {
      if (0 != _isDisposed)
      {
        throw new ObjectDisposedException(nameof(SemaphoreLock));
      }
    }

    /// <summary>
    /// Tries to enter the underlying semaphore. This method returns immediately.
    /// </summary>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    public bool TryLock()
    {
      ThrowIfDisposed();
      return _sem.Wait(0);
    }

    /// <summary>
    /// Blocks the current thread until it can enter the underlying semaphore can be entered and throws exception if
    /// lock cannot be acqured.
    /// </summary>
    public void Lock()
    {
      ThrowIfDisposed();
      _sem.Wait();
    }

    /// <summary>
    /// Asynchronously tries to enter the underlying semaphore.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    public async Task<bool> TryLockAsync(CancellationToken cancellationToken)
    {
      ThrowIfDisposed();
      return await _sem.WaitAsync(0, cancellationToken);
    }

    /// <summary>
    /// Returns task which is completed when the underlying semaphore has been entered.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task LockAsync(CancellationToken cancellationToken)
    {
      ThrowIfDisposed();
      await _sem.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Releases the underlying semaphore.
    /// </summary>
    public void Release()
    {
      ThrowIfDisposed();
      _sem.Release();
    }

    /// <summary>
    /// Disposes the underlying semaphore.
    /// </summary>
    public void Dispose()
    {
      if (0 == Interlocked.CompareExchange(ref _isDisposed, 1, 0))
      {
        _sem.Dispose();
      }
    }
  }

}