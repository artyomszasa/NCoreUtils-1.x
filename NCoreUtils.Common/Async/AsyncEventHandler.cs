using System.Linq;
using System.Threading.Tasks;

namespace NCoreUtils.Async
{
  /// <summary>
  /// Asynchronous event handler.
  /// </summary>
  public delegate Task AsyncEventHandler(object sender, AsyncEventArgs e);

  /// <summary>
  /// Asynchronous event handler.
  /// </summary>
  public delegate Task AsyncEventHandler<T>(object sender, T e) where T : AsyncEventArgs;

  /// <summary>
  /// Contains extensions for async event handling.
  /// </summary>
  public static class AsyncEventHandlerExtensions
  {
    /// <summary>
    /// Sequentially invokes asyncrounous multicast delegate.
    /// </summary>
    /// <param name="handler">Multicast delegate to invoke.</param>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">
    /// Instance of <see cref="T:NCoreUtils.Async.AsyncEventArgs" /> handling the cancellation.
    /// </param>
    /// <returns>
    /// Task which is completed when all callbacks has been processed.
    /// </returns>
    public static async Task InvokeAsync(this AsyncEventHandler handler, object sender, AsyncEventArgs e)
    {
      foreach (var item in AsyncDelegateExtensions.EnumerateInvokationList(handler, e.CancellationToken).Cast<AsyncEventHandler>())
      {
        await item.Invoke(sender, e);
      }
    }

    /// <summary>
    /// Sequentially invokes asyncrounous multicast delegate.
    /// </summary>
    /// <param name="handler">Multicast delegate to invoke.</param>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">
    /// Instance of <see cref="T:NCoreUtils.Async.AsyncEventArgs" /> handling the cancellation.
    /// </param>
    /// <returns>
    /// Task which is completed when all callbacks has been processed.
    /// </returns>
    public static async Task InvokeAsync<T>(this AsyncEventHandler<T> handler, object sender, T e) where T : AsyncEventArgs
    {
      foreach (var item in AsyncDelegateExtensions.EnumerateInvokationList(handler, e.CancellationToken).Cast<AsyncEventHandler<T>>())
      {
        await item.Invoke(sender, e);
      }
    }
  }
}