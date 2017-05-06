using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace NCoreUtils.Collections
{
  //TODO: implement ICollection
  /// <summary>
  /// Represents a dictionary which may contain multiple values assigned to the same key.
  /// </summary>
  public class MultiDictionary<TKey, TValue> : IMultiDictionary<TKey, TValue>, IEquatable<MultiDictionary<TKey, TValue>>
  {
    struct Entry
    {
      /// <summary>
      /// Lower 31 bits of the hash code.
      /// </summary>
      public int HashCode;
      /// <summary>
      /// Index of the next entry.
      /// </summary>
      public int Next;
      /// <summary>
      /// The key of the entry;
      /// </summary>
      public TKey Key;
      /// <summary>
      /// The value of the entry.
      /// </summary>
      public TValue Value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public bool Equals(Entry that, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
      {
        return HashCode == that.HashCode && Next == that.Next && keyComparer.Equals(Key, that.Key) && valueComparer.Equals(Value, that.Value);
      }

      public override int GetHashCode()
      {
        return (HashCode << 8) ^ Next ^ Key.GetHashCode() ^ Value.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        if (null == obj || !(obj is Entry))
        {
          return false;
        }
        return Equals((Entry)obj, EqualityComparer<TKey>.Default, EqualityComparer<TValue>.Default);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool operator ==(Entry a, Entry b) => a.Equals(b);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool operator !=(Entry a, Entry b) => !a.Equals(b);
    }


    int[] _buckets;
    Entry[] _entries;
    int _version;
    int _freeIndex;
    int _freeCount;
    int _count;
    IEqualityComparer<TKey> _keyComparer;
    IEqualityComparer<TValue> _valueComparer;

    /// <summary>
    /// Allocated capacity of the actual instance.
    /// </summary>
    public int Capacity
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get { return null == _entries ? 0 : _entries.Length; }
    }

    private MultiDictionary(int[] buckets, Entry[] entries, int freeIndex, int freeCount, int count, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
    {
      if (null != buckets)
      {
        _buckets = new int[buckets.Length];
        for (var i = 0; i < buckets.Length; ++i)
        {
          _buckets[i] = buckets[i];
        }
      }
      if (null != entries)
      {
        _entries = new Entry[entries.Length];
        for (var i = 0; i < entries.Length; ++i)
        {
          _entries[i] = entries[i];
        }
      }
      _freeIndex = freeIndex;
      _freeCount = freeCount;
      _count = count;
      _keyComparer = keyComparer;
      _valueComparer = valueComparer;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:NFS.Collections.MultiDictionary`2"/> class.
    /// </summary>
    /// <param name="capacity">Capacity.</param>
    /// <param name="keyEqualityComparer">Key equality comparer.</param>
    /// <param name="valueEqualityComparer">Value equality comparer.</param>
    public MultiDictionary(int capacity = 0, IEqualityComparer<TKey> keyEqualityComparer = null, IEqualityComparer<TValue> valueEqualityComparer = null)
    {
      RuntimeAssert.ArgumentInRange(capacity, 0, int.MaxValue, nameof(capacity));
      if (capacity > 0)
      {
        Initialize(capacity);
      }
      _keyComparer = keyEqualityComparer ?? EqualityComparer<TKey>.Default;
      _valueComparer = valueEqualityComparer ?? EqualityComparer<TValue>.Default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:NFS.Collections.MultiDictionary`2"/> class with default inital
    /// capacity.
    /// </summary>
    /// <param name="keyEqualityComparer">Key equality comparer.</param>
    public MultiDictionary(IEqualityComparer<TKey> keyEqualityComparer) : this(0, keyEqualityComparer) { }

    void Initialize(int capacity)
    {
      int size = HashHelper.GetPrime(capacity);
      _buckets = new int[size];
      _buckets.Fill(-1);
      _entries = new Entry[size];
      _freeIndex = -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int GetKeyHasCode(TKey key) => _keyComparer.GetHashCode(key) & 0x7FFFFFFF;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void EnsureInitialized()
    {
      if (null == _buckets)
      {
        Initialize(0);
      }
    }

    void Resize(int newSize, bool forceNewHashCodes)
    {
      RuntimeAssert.GreaterOrEquals(newSize, _entries.Length, nameof(newSize), "entry count");
      int[] newBuckets = new int[newSize];
      for (int i = 0; i < newBuckets.Length; i++) newBuckets[i] = -1;
      Entry[] newEntries = new Entry[newSize];
      Array.Copy(_entries, 0, newEntries, 0, _count);
      if (forceNewHashCodes)
      {
        for (int i = 0; i < _count; i++)
        {
          if (newEntries[i].HashCode != -1)
          {
            newEntries[i].HashCode = GetKeyHasCode(newEntries[i].Key);
          }
        }
      }
      for (int i = 0; i < _count; i++)
      {
        if (newEntries[i].HashCode >= 0)
        {
          int bucket = newEntries[i].HashCode % newSize;
          newEntries[i].Next = newBuckets[bucket];
          newBuckets[bucket] = i;
        }
      }
      _buckets = newBuckets;
      _entries = newEntries;
    }

    void Resize() => Resize(HashHelper.ExpandPrime(_count), false);

    bool FindEntries(TKey key, ICollection<int> entries)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      var found = false;
      if (null != _buckets)
      {
        var hashCode = GetKeyHasCode(key);
        for (int i = _buckets[hashCode % _buckets.Length]; i >= 0; i = _entries[i].Next)
        {
          if (_entries[i].HashCode == hashCode && _keyComparer.Equals(_entries[i].Key, key))
          {
            found = true;
            entries.Add(i);
          }
        }
      }
      return found;
    }

    int FindEntry(TKey key)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      if (null != _buckets)
      {
        var hashCode = GetKeyHasCode(key);
        for (int i = _buckets[hashCode % _buckets.Length]; i >= 0; i = _entries[i].Next)
        {
          if (_entries[i].HashCode == hashCode && _keyComparer.Equals(_entries[i].Key, key))
          {
            return i;
          }
        }
      }
      return -1;
    }

    int Insert(TKey key, TValue value, int hashCode, int targetBucket0)
    {
      var targetBucket = targetBucket0;
      int index;
      if (_freeCount > 0)
      {
        index = _freeIndex;
        _freeIndex = _entries[index].Next;
        --_freeCount;
      }
      else
      {
        if (_count == _entries.Length)
        {
          Resize();
          targetBucket = hashCode % _buckets.Length;
        }
        index = _count;
        ++_count;
      }

      _entries[index].HashCode = hashCode;
      _entries[index].Next = _buckets[targetBucket];
      _entries[index].Key = key;
      _entries[index].Value = value;
      _buckets[targetBucket] = index;
      unchecked
      {
        ++_version;
      }
      return targetBucket;
    }

    int Remove(TKey key, Func<int, bool> predicate)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      if (null != _buckets)
      {
        var hashCode = GetKeyHasCode(key);
        var bucket = hashCode % _buckets.Length;
        var last = -1;
        var i = _buckets[bucket];
        var removed = 0;
        while (i >= 0)
        {
          if (_entries[i].HashCode == hashCode && predicate(i))
          {
            int j = _entries[i].Next;
            if (0 > last)
            {
              // first item
              _buckets[bucket] = j;
            }
            else
            {
              _entries[last].Next = j;
            }
            _entries[i].HashCode = -1;
            _entries[i].Next = _freeIndex;
            _entries[i].Key = default(TKey);
            _entries[i].Value = default(TValue);
            _freeIndex = i;
            ++_freeCount;
            ++_version;
            ++removed;
            i = j;
          }
          else
          {
            last = i;
            i = _entries[i].Next;
          }
        }
        return removed;
      }
      return 0;
    }

    bool Contains(TKey key, Func<int, bool> predicate)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      var found = false;
      if (null != _buckets)
      {
        var hashCode = GetKeyHasCode(key);
        for (int i = _buckets[hashCode % _buckets.Length]; !found && i >= 0; i = _entries[i].Next)
        {
          found = _entries[i].HashCode == hashCode && predicate(i);
        }
      }
      return found;
    }

    #region IMultDictionary

    /// <summary>
    /// Assignes the specified value to the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void Add(TKey key, TValue value)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      EnsureInitialized();
      var hashCode = GetKeyHasCode(key);
      var targetBucket = hashCode % _buckets.Length;
      Insert(key, value, hashCode, targetBucket);
    }

    /// <summary>
    /// Assignes the specified values to the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="values">Values to assign.</param>
    public void Add(TKey key, IEnumerable<TValue> values)
    {
      RuntimeAssert.ArgumentNotNull(key, nameof(key));
      EnsureInitialized();
      var hashCode = GetKeyHasCode(key);
      var targetBucket = hashCode % _buckets.Length;
      foreach (var value in values)
      {
        targetBucket = Insert(key, value, hashCode, targetBucket);
      }
    }

    /// <summary>
    /// Assignes the specified values to the specified key.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="values">Values to assign.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(TKey key, params TValue[] values)
    {
      Add(key, (IEnumerable<TValue>)values);
    }

    /// <summary>
    /// Clones the dictionary. Items are not cloned but copied by reference.
    /// </summary>
    /// <returns></returns>
    public MultiDictionary<TKey, TValue> Clone()
      => new MultiDictionary<TKey, TValue>(_buckets, _entries, _freeIndex, _freeCount, _count, _keyComparer, _valueComparer);


    /// <summary>
    /// Removes all values assigned to the specified key.
    /// </summary>
    /// <returns>Amount of values has been removed.</returns>
    /// <param name="key">Key.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Remove(TKey key)
      => Remove(key, index => _keyComparer.Equals(_entries[index].Key, key));

    /// <summary>
    /// Assignes the specified value to the specified key. Replaces any previous assignments.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Values to assign.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(TKey key, TValue value)
    {
      Remove(key);
      Add(key, value);
    }

    /// <summary>
    /// Assignes the specified values to the specified key. Replaces any previous assignments.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="values">Values to assign.</param>

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(TKey key, IEnumerable<TValue> values)
    {
      Remove(key);
      Add(key, values);
    }

    /// <summary>
    /// Assignes the specified values to the specified key. Replaces any previous assignments.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="values">Values to assign.</param>

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(TKey key, params TValue[] values) => Set(key, (IEnumerable<TValue>)values);

    #endregion

    #region IReadOnlyMultiDictionary

    /// <summary>
    /// Gets the sequence containing all keys which are assigned at least one value.
    /// </summary>
    /// <value>The keys.</value>

    public IEnumerable<TKey> Keys
    {
      get
      {
        if (null != _entries)
        {
          for (var i = 0; i < _count; ++i)
          {
            if (_entries[i].HashCode >= 0)
            {
              yield return _entries[i].Key;
            }
          }
        }
      }
    }

    /// <summary>
    /// Gets whether sepcified key has assigned value.
    /// </summary>
    /// <returns><c>true</c>, if key has value assigned, <c>false</c> otherwise.</returns>
    /// <param name="key">Key.</param>

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ContainsKey(TKey key)
      => Contains(key, index => _keyComparer.Equals(_entries[index].Key, key));

    /// <summary>
    /// Adds all values assigned to the specified key to the specified collection.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if at least one value has been assigned the the specified key, <c>false</c> otherwise.
    /// </returns>
    /// <param name="key">Key.</param>
    /// <param name="values">Collection to add assigned values to.</param>
    public bool TryGetValues(TKey key, ICollection<TValue> values)
    {
      var buffer = new List<int>();
      var found = FindEntries(key, buffer);
      if (found)
      {
        foreach (var index in buffer)
        {
          values.Add(_entries[index].Value);
        }
      }
      return found;
    }

    /// <summary>
    /// Gets all values assigned to the specified key.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if at least one value has been assigned the the specified key, <c>false</c> otherwise.
    /// </returns>
    /// <param name="key">Key.</param>
    /// <param name="values">
    /// Variable to store assigned values. After calling this function either references array containing the assigned
    /// values or null.
    /// </param>
    public bool TryGetValues(TKey key, out TValue[] values)
    {
      var buffer = new List<int>();
      var found = FindEntries(key, buffer);
      if (found)
      {
        var l = buffer.Count;
        var vs = new TValue[l];
        for (var i = 0; i < l; ++i)
        {
          vs[i] = _entries[buffer[i]].Value;
        }
        values = vs;
      }
      else
      {
        values = null;
      }
      return found;
    }

    /// <summary>
    /// Tries to get first value assigned to the specified key.
    /// </summary>
    /// <returns><c>true</c>, if get at least one value has beed assigned, <c>false</c> otherwise.</returns>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public bool TryGetValue(TKey key, out TValue value)
    {
      var i = FindEntry(key);
      if (-1 == i)
      {
        value = default(TValue);
        return false;
      }
      value = _entries[i].Value;
      return true;
    }

    #endregion

    #region ICollection

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    /// <summary>
    /// Clears all assignment contained by the current instance.
    /// </summary>
    public void Clear()
    {
      if (_count > 0)
      {
        _buckets.Fill(-1);
        Array.Clear(_entries, 0, _count);
        _freeIndex = -1;
        _count = 0;
        _freeCount = 0;
        ++_version;
      }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
      var key = item.Key;
      var value = item.Value;
      return Contains(key, index => _keyComparer.Equals(_entries[index].Key, key) && EqualityComparer<TValue>.Default.Equals(_entries[index].Value, value));
    }

    /// <summary>
    /// Copies all assignments to the specified array at the specified index.
    /// </summary>
    /// <param name="array">Array.</param>
    /// <param name="arrayIndex">Array index.</param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      RuntimeAssert.ArgumentNotNull(array, nameof(array));
      RuntimeAssert.IndexInRange(arrayIndex, 0, array.Length - 1, nameof(arrayIndex));
      RuntimeAssert.GreaterOrEquals(array.Length - arrayIndex, _count, "target capacity", "entry count");
      if (null != _entries)
      {
        for (var i = 0; i < _count; ++i)
        {
          if (_entries[i].HashCode >= 0)
          {
            array[i + arrayIndex] = new KeyValuePair<TKey, TValue>(_entries[i].Key, _entries[i].Value);
          }
        }
      }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
      var key = item.Key;
      var value = item.Value;
      return Remove(key, index => _keyComparer.Equals(_entries[index].Key, key) && EqualityComparer<TValue>.Default.Equals(_entries[index].Value, value)) > 0;
    }

    #endregion

    #region IReadOnlyCollection

    /// <summary>
    /// Gets the assgned values' count.
    /// </summary>
    public int Count
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      [DebuggerStepThrough]
      get => _count - _freeCount;
    }

    #endregion

    #region IEnumerable

    sealed class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
      MultiDictionary<TKey, TValue> _source;
      int _index;
      int _version;
      KeyValuePair<TKey, TValue> _current;

      public KeyValuePair<TKey, TValue> Current => _current;

      object IEnumerator.Current => _current;

      public Enumerator(MultiDictionary<TKey, TValue> source)
      {
        _source = source;
        Reset();
      }

      public void Dispose()
      {
        _source = null;
        _current = default(KeyValuePair<TKey, TValue>);
      }

      public bool MoveNext()
      {
        if (_version != _source._version)
        {
          throw new InvalidOperationException("MultiDictinary has changed during enumeration");
        }
        bool success = false;
        ++_index;
        while (!success && _index < _source._count)
        {
          if (_source._entries[_index].HashCode >= 0)
          {
            _current = new KeyValuePair<TKey, TValue>(_source._entries[_index].Key, _source._entries[_index].Value);
            success = true;
          }
          else
          {
            ++_index;
          }
        }
        return success;
      }

      public void Reset()
      {
        _version = _source._version;
        _index = -1;
      }
    }

    /// <summary>
    /// Gets the assignment enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region equality

    /// <summary>
    /// Determines whether the specified <see cref="T:NFS.Collections.MultiDictionary`2"/> is equal to the
    /// current <see cref="T:NFS.Collections.MultiDictionary`2"/>.
    /// </summary>
    /// <param name="that">The <see cref="T:NFS.Collections.MultiDictionary`2"/> to compare with the current <see cref="T:NFS.Collections.MultiDictionary`2"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="T:NFS.Collections.MultiDictionary`2"/> is equal to the current
    /// <see cref="T:NFS.Collections.MultiDictionary`2"/>; otherwise, <c>false</c>.</returns>
    public bool Equals(MultiDictionary<TKey, TValue> that)
    {
      if (_freeCount != that._freeCount)
      {
        return false;
      }
      if (_freeIndex != that._freeIndex)
      {
        return false;
      }
      if (_count != that._count)
      {
        return false;
      }
      if (!_keyComparer.Equals(that._keyComparer))
      {
        return false;
      }
      if (!_valueComparer.Equals(that._valueComparer))
      {
        return false;
      }
      if (_buckets.Length != that._buckets.Length)
      {
        return false;
      }
      if (_entries.Length != that._entries.Length)
      {
        return false;
      }
      for (var i = 0; i < _buckets.Length; ++i)
      {
        if (_buckets[i] != that._buckets[i])
        {
          return false;
        }
      }
      for (var i = 0; i < _entries.Length; ++i)
      {
        if (_entries[i].Equals(that._entries[i], _keyComparer, _valueComparer))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:NFS.Collections.MultiDictionary`2"/>.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:NFS.Collections.MultiDictionary`2"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
    /// <see cref="T:NFS.Collections.MultiDictionary`2"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
      var that = obj as MultiDictionary<TKey, TValue>;
      if (null == that)
      {
        return false;
      }
      return Equals(that);
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="T:NFS.Collections.MultiDictionary`2"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
      var result = 0;
      foreach (var bucket in _buckets)
      {
        result = (result << 8) ^ bucket;
      }
      return result;
    }

    #endregion
  }
}

