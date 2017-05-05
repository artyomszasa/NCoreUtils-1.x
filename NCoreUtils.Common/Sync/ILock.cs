using System;

namespace NCoreUtils.Sync
{
  /// <summary>
  /// Interface of synchronization objects. 
  /// </summary>
  public interface ILock : IDisposable
  {
    /// <summary>
    /// Tries to acquire lock implemented by the synchronization object. This method should return as soon as possible.
    /// </summary>
    /// <returns><c>true</c> if lock has been successfully acquired; <c>false</c> otherwise.</returns>
    bool TryLock();
    
    /// <summary>
    /// Acquires lock implemented by the synchronization object. This method blocks until lock has been acquired
    /// and throws exception if lock cannot be acqured.
    /// </summary>
    void Lock();

    /// <summary>
    /// Releases lock implemented by the synchronization object. This method should return as soon as possible.
    /// </summary>
    void Release();
  }
}