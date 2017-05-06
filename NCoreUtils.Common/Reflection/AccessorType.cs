using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NCoreUtils.Reflection
{
  /// <summary>
  /// Strongly typed enumeration representing type of the <see cref="T:NCoreUtils.Reflection.IAccessor" /> instance.
  /// </summary>
  public abstract class AccessorType : IEquatable<AccessorType>
  {
    sealed class PropertyAccessor : AccessorType
    {
      internal static readonly AccessorType Instance = new PropertyAccessor();
      internal override int Hash => 1;
      PropertyAccessor() { }
      internal override void Accept(IAccessorTypeVisitor visitor) => visitor.VisitProperty();
      internal override T Accept<T>(IAccessorTypeVisitor<T> visitor) => visitor.VisitProperty();
    }

    sealed class FieldAccessor : AccessorType
    {
      internal static readonly AccessorType Instance = new FieldAccessor();
      internal override int Hash => 2;
      FieldAccessor() { }
      internal override void Accept(IAccessorTypeVisitor visitor) => visitor.VisitField();
      internal override T Accept<T>(IAccessorTypeVisitor<T> visitor) => visitor.VisitField();
    }

    sealed class CustomAccessor : AccessorType
    {
      internal static readonly AccessorType Instance = new CustomAccessor();
      internal override int Hash => 3;
      CustomAccessor() { }
      internal override void Accept(IAccessorTypeVisitor visitor) => visitor.VisitCustom();
      internal override T Accept<T>(IAccessorTypeVisitor<T> visitor) => visitor.VisitCustom();
    }

    sealed class MatchVisitor : IAccessorTypeVisitor
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      [DebuggerStepThrough]
      static void ThrowIfNull(Action action, string name)
      {
        if (null == action)
        {
          throw new ArgumentException($"Either {name} or otherwise must not be null.", name);
        }
      }
      readonly Action _onProperty;
      readonly Action _onField;
      readonly Action  _onCustom;
      readonly Action _otherwise;
      internal MatchVisitor(Action onProperty, Action onField, Action onCustom, Action otherwise)
      {
        if (null == otherwise)
        {
          ThrowIfNull(onProperty, nameof(onProperty));
          ThrowIfNull(onField, nameof(onField));
          ThrowIfNull(onCustom, nameof(onCustom));
        }
        _onProperty = onProperty;
        _onField = onField;
        _onCustom = onCustom;
        _otherwise = otherwise;
      }
      void IAccessorTypeVisitor.VisitCustom() => (_onCustom ?? _otherwise).Invoke();
      void IAccessorTypeVisitor.VisitField() => (_onField ?? _otherwise).Invoke();
      void IAccessorTypeVisitor.VisitProperty() => (_onProperty ?? _otherwise).Invoke();
    }

    sealed class MatchVisitor<T> : IAccessorTypeVisitor<T>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      [DebuggerStepThrough]
      static void ThrowIfNull(Func<T> action, string name)
      {
        if (null == action)
        {
          throw new ArgumentException($"Either {name} or otherwise must not be null.", name);
        }
      }
      readonly Func<T> _onProperty;
      readonly Func<T> _onField;
      readonly Func<T>  _onCustom;
      readonly Func<T> _otherwise;
      internal MatchVisitor(Func<T> onProperty, Func<T> onField, Func<T> onCustom, Func<T> otherwise)
      {
        if (null == otherwise)
        {
          ThrowIfNull(onProperty, nameof(onProperty));
          ThrowIfNull(onField, nameof(onField));
          ThrowIfNull(onCustom, nameof(onCustom));
        }
        _onProperty = onProperty;
        _onField = onField;
        _onCustom = onCustom;
        _otherwise = otherwise;
      }
      T IAccessorTypeVisitor<T>.VisitCustom() => (_onCustom ?? _otherwise).Invoke();
      T IAccessorTypeVisitor<T>.VisitField() => (_onField ?? _otherwise).Invoke();
      T IAccessorTypeVisitor<T>.VisitProperty() => (_onProperty ?? _otherwise).Invoke();
    }
    /// <summary>
    /// Represents property option of the strongly typed enum.
    /// </summary>
    public static AccessorType Property { get; } = PropertyAccessor.Instance;
    /// <summary>
    /// Represents field option of the strongly typed enum.
    /// </summary>
    public static AccessorType Field { get; } = FieldAccessor.Instance;
    /// <summary>
    /// Represents custom option of the strongly typed enum.
    /// </summary>
    public static AccessorType Custom { get; } = CustomAccessor.Instance;
    internal abstract int Hash { get; }
    internal abstract void Accept(IAccessorTypeVisitor visitor);
    internal abstract T Accept<T>(IAccessorTypeVisitor<T> visitor);
    /// <summary>
    /// Determines whether two instances of <see cref="T:NCoreUtils.Reflection.AccessorType" /> are equal.
    /// </summary>
    /// <param name="that">Instance to compare to the actual.</param>
    /// <returns>
    /// <c>true</c> if both instances represent the same stronged enum option, <c>false</c> otherwise.
    /// </returns>
    public bool Equals(AccessorType that) => null != that && Hash == that.Hash;
    /// <summary>
    /// Determines whether the specified object and the actual instance of
    /// <see cref="T:NCoreUtils.Reflection.AccessorType" /> are equal.
    /// </summary>
    /// <param name="other">Object to compare to the actual.</param>
    /// <returns>
    /// <c>true</c> if both instances represent the same stronged enum option, <c>false</c> otherwise.
    /// </returns>
    public override bool Equals(object other)
    {
      var that = other as AccessorType;
      return that != null && Equals(that);
    }
    /// <summary>
    /// Returns the hash code for the current <see cref="T:NCoreUtils.Reflection.AccessorType" /> instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Hash;
    /// <summary>
    /// Performs pattern matching on the actual instance of <see cref="T:NCoreUtils.Reflection.AccessorType" />.
    /// </summary>
    /// <param name="onProperty">Action to perform if the actual instance represents property option.</param>
    /// <param name="onField">Action to perform if the actual instance represents field option.</param>
    /// <param name="onCustom">Action to perform if the actual instance represents custom option.</param>
    /// <param name="otherwise">Action to perform if any other handler is not specified.</param>
    public void Match(Action onProperty = null, Action onField = null, Action onCustom = null, Action otherwise = null)
      => Accept(new MatchVisitor(onProperty, onField, onCustom, otherwise));
    /// <summary>
    /// Performs pattern matching on the actual instance of <see cref="T:NCoreUtils.Reflection.AccessorType" />.
    /// </summary>
    /// <param name="onProperty">Value factory to invoke if the actual instance represents property option.</param>
    /// <param name="onField">Value factory to invoke if the actual instance represents field option.</param>
    /// <param name="onCustom">Value factory to invoke if the actual instance represents custom option.</param>
    /// <param name="otherwise">Value factory to invoke if any other handler is not specified.</param>
    public T Match<T>(Func<T> onProperty = null, Func<T> onField = null, Func<T> onCustom = null, Func<T> otherwise = null)
      => Accept(new MatchVisitor<T>(onProperty, onField, onCustom, otherwise));
  }
}