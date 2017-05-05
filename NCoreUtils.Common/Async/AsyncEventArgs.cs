using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NCoreUtils.Async
{
  /// <summary>
  /// Asynchronous event handler argument.
  /// </summary>
  public class AsyncEventArgs : EventArgs
  {
    /// <summary>
    /// Actual cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; private set; }

    /// <summary>
    /// Initializes new instance of <see cref="T:NCoreUtils.Async.AsyncEventArgs" /> with
    /// the specified cancellation token.
    /// </summary>
    /// <param name="cancellationToken">Operation cancellation.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public AsyncEventArgs(CancellationToken cancellationToken)
    {
      CancellationToken = cancellationToken;
    }
  }
}