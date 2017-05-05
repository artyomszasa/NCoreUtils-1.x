using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using NCoreUtils.Sync;

namespace NCoreUtils
{
  /// <summary>
  /// Provides factory functionality for creation of <see cref="T:NCoreUtils.OnDemand`2"/> objects.
  /// </summary>
  public static class OnDemand
  {
    sealed class ThreadSafeOnDemand<TValue, TArg> : OnDemand<TValue, TArg>
    {
      readonly ILock _lock;
      int _isDisposed;

      public ThreadSafeOnDemand(Func<TArg, TValue> initializer, ILock @lock)
        : base(initializer)
      {
        _lock = @lock;
      }

      protected override void Dispose(bool disposing)
      {
        if (0 == Interlocked.CompareExchange(ref _isDisposed, 1, 0))
        {
          _lock.Dispose();
        }
        base.Dispose(disposing);
      }

      protected override bool InitValue(TArg arg, out TValue value)
      {
        _lock.Lock();
        try
        {
          if (null != _value)
          {
            value = _value.Value;
            return false;
          }
          var v = Initializer(arg);
          _value = ValueBox.Create(v);
          value = v;
          return true;
        }
        finally
        {
          _lock.Release();
        }
      }

      protected override bool ResetValue()
      {
        return _lock.Synchronized(() => null != Interlocked.Exchange(ref _value, null));
      }

      protected override bool SetValue(TValue value, bool replace)
      {
        return _lock.Synchronized(() => {
          if (replace || null == _value)
          {
            _value = ValueBox.Create(value);
            return true;
          }
          return false;
        });
      }
    }

    sealed class ConcurrentOnDemand<TValue, TArg> : OnDemand<TValue, TArg>
    {
      public ConcurrentOnDemand(Func<TArg, TValue> initializer) : base(initializer) { }

      protected override bool InitValue(TArg arg, out TValue value)
      {
        var newValue = ValueBox.Create(Initializer(arg));
        Interlocked.MemoryBarrier();
        if (null == Interlocked.CompareExchange(ref _value, newValue, null))
        {
          value = newValue.Value;
          return true;
        }
        value = _value.Value;
        return false;
      }

      protected override bool ResetValue()
      {
        ValueBox<TValue> value = null;
        var success = false;
        while (!success)
        {
          value = _value;
          Interlocked.MemoryBarrier();
          success = value == Interlocked.CompareExchange(ref _value, null, value);
        }
        return null == value;
      }

      protected override bool SetValue(TValue value, bool replace)
      {
        var success = false;
        if (replace)
        {
          while (!success)
          {
            var v = _value;
            success = v == Interlocked.CompareExchange(ref _value, ValueBox.Create(value), v);
          }
          return true;
        }
        var set = false;
        while (success)
        {
          var v = _value;
          if (null != v)
          {
            success = true;
            set = false;
          }
          else
          {
            success = v == Interlocked.CompareExchange(ref _value, ValueBox.Create(value), v);
            set = success;
          }
        }
        return set;
      }
    }

    sealed class NoCheckOnDemand<TValue, TArg> : OnDemand<TValue, TArg>
    {
      public NoCheckOnDemand(Func<TArg, TValue> initializer) : base(initializer) { }

      protected override bool InitValue(TArg arg, out TValue value)
      {
        var v = Initializer(arg);
        _value = ValueBox.Create(v);
        value = v;
        return true;
      }

      protected override bool ResetValue()
      {
        if (null == _value)
        {
          return false;
        }
        _value = null;
        return true;
      }

      protected override bool SetValue(TValue value, bool replace)
      {
        if (replace || null == _value)
        {
          _value = ValueBox.Create(value);
          return true;
        }
        return false;
      }
    }

    /// <summary>
    /// Represents operation performed on the objects generated on demand.
    /// </summary>
    public enum ChangeType
    {
      /// Value has been initialized.
      Created,
      /// Value has been explicitly set.
      Set,
      /// Value has been reset.
      Reset
    }

    /// <summary>
    /// Argument object passed to the appropriate event when operation performed on
    /// the objects generated on demand.
    /// </summary>
    public sealed class ChangedArgs : EventArgs
    {
      /// <summary>
      /// Singleton instance which passed as argument of the <see cref="E:NCoreUtils.OnDemand`2.Changed" />
      /// event when the value has been initialized.
      /// </summary>
      public static ChangedArgs Created { get; } = new ChangedArgs(ChangeType.Created);
      /// <summary>
      /// Singleton instance which passed as argument of the <see cref="E:NCoreUtils.OnDemand`2.Changed" />
      /// event when the value has been explicitly set.
      /// </summary>
      public static ChangedArgs Set { get; } = new ChangedArgs(ChangeType.Set);
      /// <summary>
      /// Singleton instance which passed as argument of the <see cref="E:NCoreUtils.OnDemand`2.Changed" />
      /// event when the value has been reset.
      /// </summary>
      public static ChangedArgs Reset { get; } = new ChangedArgs(ChangeType.Reset);
      /// <summary>
      /// Triggering operation.
      /// </summary>
      public ChangeType ChangeType { get; private set; }

      ChangedArgs(ChangeType changeType)
      {
        ChangeType = changeType;
      }
    }

    /// <summary>
    /// On-demand instances callback delegate.
    /// </summary>
    public delegate void ChangedDelegate(object sender, ChangedArgs e);

    /// <summary>
    /// Initializes new instance of the on-demand instance.
    /// </summary>
    /// <param name="initializer">Value initializer.</param>
    /// <param name="mode">Thread-safe mode.</param>
    /// <param name="lock">Synchronization object to use if needed.</param>
    /// <returns>Initialized instance.</returns>
    public static OnDemand<TValue, TArg> Init<TValue, TArg>(Func<TArg, TValue> initializer, LazyThreadSafetyMode mode = LazyThreadSafetyMode.ExecutionAndPublication, ILock @lock = null)
    {
      if (LazyThreadSafetyMode.None == mode)
      {
        return new NoCheckOnDemand<TValue, TArg>(initializer);
      }
      if (LazyThreadSafetyMode.PublicationOnly == mode)
      {
        return new ConcurrentOnDemand<TValue, TArg>(initializer);
      }
      return new ThreadSafeOnDemand<TValue, TArg>(initializer, @lock ?? new CasLock());
    }

    /// <summary>
    /// Initializes new instance of the on-demand instance with thread-safe initialization.
    /// </summary>
    /// <param name="initializer">Value initializer.</param>
    /// <param name="lock">Synchronization object to use.</param>
    /// <returns>Initialized instance.</returns>
    public static OnDemand<TValue, TArg> Init<TValue, TArg>(Func<TArg, TValue> initializer, ILock @lock)
      => Init(initializer, LazyThreadSafetyMode.ExecutionAndPublication, @lock);
  }

  /// <summary>
  /// Provides support for on-demand value initialization.
  /// </summary>
  public abstract class OnDemand<TValue, TArg> : IDisposable
  {
    /// <summary>
    /// Stores value initializer.
    /// </summary>
    readonly Func<TArg, TValue> _initializer;
    /// <summary>
    /// Either stores boxed initialized value or <c>null</c>.
    /// </summary>
    [CLSCompliant(false)]
    protected ValueBox<TValue> _value = null;
    int _isDisposed;

    /// <summary>
    /// Read-only access to the initializer function.
    /// </summary>
    protected Func<TArg, TValue> Initializer
    {
      [DebuggerStepThrough]
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return _initializer; }
    }

    /// <summary>
    /// Triggered whenever any value change occures.
    /// </summary>
    public event OnDemand.ChangedDelegate Changed;

    /// <summary>
    /// On-demand object initializer.
    /// </summary>
    /// <param name="initializer">Initialization function.</param>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected OnDemand(Func<TArg, TValue> initializer)
    {
      _initializer = initializer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ThrowIfDisposed()
    {
      if (0 != _isDisposed)
      {
        throw new ObjectDisposedException(nameof(OnDemand));
      }
    }

    /// <summary>
    /// Implements explicit on-demand value setting in the derived class.
    /// </summary>
    /// <param name="value">Value to set.</param>
    /// <param name="replace">Whether to replace the value if already initialized.</param>
    /// <returns>
    /// <c>true</c> if value has been set, <c>false</c> otherwise.
    /// </returns>
    protected abstract bool SetValue(TValue value, bool replace);

    /// <summary>
    /// Implements explicit on-demand value reset in the derived class.
    /// </summary>
    /// <returns>
    /// <c>true</c> if value has been reset, <c>false</c> otherwise.
    /// </returns>
    protected abstract bool ResetValue();

    /// <summary>
    /// Implements value initialization in the derived class.
    /// </summary>
    /// <param name="arg">Initialization argument.</param>
    /// <param name="value">Variable to return the initialized value.</param>
    /// <returns>
    /// <c>true</c> if value has been initialized, <c>false</c> otherwise.
    /// </returns>
    protected abstract bool InitValue(TArg arg, out TValue value);

    /// <summary>
    /// Overridable dispose method.
    /// </summary>
    /// <param name="disposing"><c>true</c> if disposing, <c>false</c> if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
      _isDisposed = 1;
    }

    /// <summary>
    /// Explicitely sets the underlying value.
    /// </summary>
    /// <param name="value">Value to set.</param>
    /// <param name="replace">Whether to replace the value if it has been already initialized.</param>
    public void Set(TValue value, bool replace = true)
    {
      ThrowIfDisposed();
      if (SetValue(value, replace))
      {
        Changed?.Invoke(this, OnDemand.ChangedArgs.Set);
      }
    }

    /// <summary>
    /// Resets the underlying value is any.
    /// </summary>
    public void Reset()
    {
      ThrowIfDisposed();
      if (ResetValue())
      {
        Changed?.Invoke(this, OnDemand.ChangedArgs.Reset);
      }
    }

    /// <summary>
    /// Returns the on-demand value. Initializes the value if it has not been initialized already.
    /// </summary>
    /// <param name="arg">Initialization argument.</param>
    /// <returns>The on-demand value.</returns>
    public TValue GetValue(TArg arg)
    {
      ThrowIfDisposed();
      var vbox = _value;
      if (null != vbox) {
        return vbox.Value;
      }
      TValue value;
      if (InitValue(arg, out value))
      {
        Changed?.Invoke(this, OnDemand.ChangedArgs.Created);
      }
      return value;
    }

    /// <summary>
    /// Implements resource releasing in the derived class.
    /// </summary>
    public virtual void Dispose() => Dispose(true);
  }
}