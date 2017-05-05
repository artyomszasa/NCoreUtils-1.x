using System;
using System.Threading.Tasks;

namespace NCoreUtils.Async
{
  /// <summary>
  /// Provides a mechanism for releasing unmanaged resources.
  /// <para>
  /// Possible replacement: https://github.com/dotnet/roslyn/issues/114.
  /// </para>
  /// </summary>
  public interface IAsyncDisposable : IDisposable
  {
    /// <summary>
    /// Asynchronously performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <returns>
    /// Task which is completed when the operation has been performed.
    /// </returns>
    Task DisposeAsync();
  }
}