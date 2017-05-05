using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NCoreUtils
{
  /// <summary>
  /// Provides implementation for full comparison (both order and equality) of an object.
  /// <para>
  /// <see cref="M:ObjectComparer{T}.Compare" /> must return zero only for equal objects as the comparer consider two
  /// objects equal if this method returns zero.
  /// </para>
  /// </summary>
  public abstract class ObjectComparer<T> : IObjectComparer<T>
  {
    sealed class ExplicitObjectComparer : ObjectComparer<T>
    {
      readonly IComparer<T> _comparer;
      readonly Func<T, int> _getHashCode;

      public ExplicitObjectComparer(IComparer<T> comparer, Func<T, int> getHashCode)
      {
        _comparer = comparer;
        _getHashCode = getHashCode;
      }

      public override int Compare(T x, T y) => _comparer.Compare(x, y);

      public override int GetHashCode(T x) => _getHashCode(x);
    }

    /// <summary>
    /// Gets default object comparer. Both order and eqaulity comparison are performed using
    /// <see cref="P:System.Collections.Generic.Comparer{T}.Default" /> and hash code generated using
    /// <see cref="P:System.Collections.Generic.EqualityComparer{T}.Default" />.
    /// </summary>
    public static ObjectComparer<T> Default { get; } = new ExplicitObjectComparer(Comparer<T>.Default, EqualityComparer<T>.Default.GetHashCode);

    /// <summary>
    /// Creates new object comparer. Both order and eqaulity comparison are performed using
    /// <paramref name="comparer" /> and hash code generated using <paramref name="getHashCode" />.
    /// </summary>
    /// <param name="comparer">Source comparer.</param>
    /// <param name="getHashCode">Source get hash code function.</param>
    /// <returns>Newly created object comparer.</returns>
    public static ObjectComparer<T> Create(IComparer<T> comparer, Func<T, int> getHashCode)
    {
      RuntimeAssert.ArgumentNotNull(comparer, nameof(comparer));
      RuntimeAssert.ArgumentNotNull(getHashCode, nameof(getHashCode));
      return new ExplicitObjectComparer(comparer, getHashCode);
    }

    /// <summary>
    /// When overridden in a derived class, performs a comparison of two objects of the same type and returns a value
    /// indicating whether one object is less than, equal to, or greater than the other.
    /// </summary>
    public abstract int Compare(T x, T y);

    /// <summary>
    /// When overridden in a derived class, serves as a hash function for the specified object for hashing algorithms
    /// and data structures, such as a hash table.
    /// </summary>
    public abstract int GetHashCode(T x);

    /// <summary>
    /// When overridden in a derived class, determines whether two specified objects are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(T x, T y) => 0 == Compare(x, y);
  }
}