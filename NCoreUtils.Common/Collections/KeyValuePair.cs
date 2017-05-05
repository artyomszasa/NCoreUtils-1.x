using System.Collections.Generic;

namespace NCoreUtils.Collections
{
  /// <summary>
  /// Contains helpers for <see cref="T:System.Collection.Generic.KeyValuePair{TKey,TValue}" />.
  /// </summary>
  public static class KeyValuePair
  {
    /// <summary>
    /// Equality comparer, which compares the pairs through comparing thier keys using specified equality comparer.
    /// </summary>
    public sealed class ByKeyComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
      /// <summary>
      /// Gets the equality comparer used to compare keys.
      /// </summary>
      public IEqualityComparer<TKey> KeyEqualityComparer { get; private set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="T:NFS.Misc.KeyValuePair.ByKeyComparer{TKey,TValue}"/> class, which uses
      /// equality comparer specified by <paramref name="keyEqualityComparer"/> to compare keys.
      /// </summary>
      /// <param name="keyEqualityComparer">Equality comparer to use for key comparison.</param>
      public ByKeyComparer(IEqualityComparer<TKey> keyEqualityComparer)
      {
        KeyEqualityComparer = keyEqualityComparer;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="T:NFS.Misc.KeyValuePair.ByKeyComparer{TKey,TValue}"/> class, which uses
      /// the default eqaulity comparer for key comparison.
      /// </summary>
      public ByKeyComparer() : this(EqualityComparer<TKey>.Default) { }

      /// <summary>
      /// Determines whether two instance is equal by comparing their keys using specified key equality comparer.
      /// </summary>
      /// <param name="x">First instance.</param>
      /// <param name="y">Second instance.</param>
      public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y) => KeyEqualityComparer.Equals(x.Key, y.Key);

      /// <summary>
      /// Gets the hash code ot the instance by using specified key equality comparer on key value of the instance.
      /// </summary>
      /// <param name="obj">Instance to get the has code for.</param>
      /// <returns>The hash code.</returns>
      public int GetHashCode(KeyValuePair<TKey, TValue> obj) => KeyEqualityComparer.GetHashCode(obj.Key);
    }

    /// <summary>
    /// Creates and initializes new instance of <see cref="T:System.Collections.Generic.KeyValuePair{TKey,TValue}" />.
    /// </summary>
    /// <param name="key">Value of the key member.</param>
    /// <param name="value">Value of the Value member.</param>
    /// <returns>Newly created instance.</returns>
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) => new KeyValuePair<TKey, TValue>(key, value);
  }
}