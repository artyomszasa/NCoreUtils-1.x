using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NCoreUtils.Async
{
  /// <summary>
  /// Asynchronous action with no arguments.
  /// </summary>
  public delegate Task AsyncAction(CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous action with single argument.
  /// </summary>
  public delegate Task AsyncAction<T>(T arg, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous action with 2 arguments.
  /// </summary>
  public delegate Task AsyncAction<T1, T2>(T1 arg1, T2 arg2, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous action with 3 arguments.
  /// </summary>
  public delegate Task AsyncAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous action with 4 arguments.
  /// </summary>
  public delegate Task AsyncAction<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous function with no arguments.
  /// </summary>
  public delegate Task<T> AsyncFunc<T>(CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous function with single argument.
  /// </summary>
  public delegate Task<TRes> AsyncFunc<T, TRes>(T arg, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous function with 2 arguments.
  /// </summary>
  public delegate Task<TRes> AsyncFunc<T1, T2, TRes>(T1 arg1, T2 arg2, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous function with 3 arguments.
  /// </summary>
  public delegate Task<TRes> AsyncFunc<T1, T2, T3, TRes>(T1 arg1, T2 arg2, T3 arg3, CancellationToken cancellationToken);

  /// <summary>
  /// Asynchronous function with 4 arguments.
  /// </summary>
  public delegate Task<TRes> AsyncFunc<T1, T2, T3, T4, TRes>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken cancellationToken);

  /// <summary>
  /// Contains extensions for async delegates.
  /// </summary>
  public static class AsyncDelegateExtensions
  {
    internal static IEnumerable<Delegate> EnumerateInvokationList(Delegate @delegate, CancellationToken cancellationToken)
    {
      var handler = @delegate;
      if (null != handler)
      {
        var invokationList = handler.GetInvocationList();
        foreach (var item in invokationList)
        {
          cancellationToken.ThrowIfCancellationRequested();
          yield return item;
        }
      }
    }

    /// <summary>
    /// Sequentially invokes asynchronous action.
    /// </summary>
    public static async Task InvokeAsync(this AsyncAction action, CancellationToken cancellationToken)
    {
      foreach (var item in EnumerateInvokationList(action, cancellationToken).Cast<AsyncAction>())
      {
        await item.Invoke(cancellationToken);
      }
    }

    /// <summary>
    /// Sequentially invokes asynchronous action with specified argument.
    /// </summary>
    public static async Task InvokeAsync<T>(this AsyncAction<T> action, T arg, CancellationToken cancellationToken)
    {
      foreach (var item in EnumerateInvokationList(action, cancellationToken).Cast<AsyncAction<T>>())
      {
        await item.Invoke(arg, cancellationToken);
      }
    }

    /// <summary>
    /// Sequentially invokes asynchronous action with specified arguments.
    /// </summary>
    public static async Task InvokeAsync<T1, T2>(this AsyncAction<T1, T2> action, T1 arg1, T2 arg2, CancellationToken cancellationToken)
    {
      foreach (var item in EnumerateInvokationList(action, cancellationToken).Cast<AsyncAction<T1, T2>>())
      {
        await item.Invoke(arg1, arg2, cancellationToken);
      }
    }

    /// <summary>
    /// Sequentially invokes asynchronous action with specified arguments.
    /// </summary>
    public static async Task InvokeAsync<T1, T2, T3>(this AsyncAction<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3, CancellationToken cancellationToken)
    {
      foreach (var item in EnumerateInvokationList(action, cancellationToken).Cast<AsyncAction<T1, T2, T3>>())
      {
        await item.Invoke(arg1, arg2, arg3, cancellationToken);
      }
    }

    /// <summary>
    /// Sequentially invokes asynchronous action with specified arguments.
    /// </summary>
    public static async Task InvokeAsync<T1, T2, T3, T4>(this AsyncAction<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken cancellationToken)
    {
      foreach (var item in EnumerateInvokationList(action, cancellationToken).Cast<AsyncAction<T1, T2, T3, T4>>())
      {
        await item.Invoke(arg1, arg2, arg3, arg4, cancellationToken);
      }
    }
  }
}