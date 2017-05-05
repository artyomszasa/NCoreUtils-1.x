using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Interface of synchronization objects. 
  /// </summary>
  public interface ILockAsync : ILock
  {
    /// <summary>
    /// Tries to acquire lock implemented by the synchronization object. This method should return as soon as possible.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    Task<bool> TryLockAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks until lock has been acquired
    /// and throws exception if lock cannot be acqured.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task LockAsync(CancellationToken cancellationToken);
  }
}