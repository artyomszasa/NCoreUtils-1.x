using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

namespace NCoreUtils.Collections
{
  /// <summary>
  /// Provides lock free thread-safe implementation of <see cref="T:System.Collections.Generic.ICollection`1"/>.
  /// All operations are guaranteed to be performed yet their order is not necessarly preserved.
  /// </summary>
  public sealed class LockFreeCollection<T> : ICollection<T>
  {
    ImmutableHashSet<T> _data = ImmutableHashSet<T>.Empty;

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public int Count => _data.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The object to add.</param>
    public void Add(T item)
    {
      var done = false;
      while (!done)
      {
        var oldData = _data;
        var newData = _data.Add(item);
        done = oldData == Interlocked.CompareExchange(ref _data, newData, oldData);
      }
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    public void Clear()
    {
      var done = false;
      var newData = ImmutableHashSet<T>.Empty;
      while (!done)
      {
        var oldData = _data;
        done = oldData == Interlocked.CompareExchange(ref _data, newData, oldData);
      }
    }

    /// <summary>
    /// Determines whether the collection contains a specific value.
    /// </summary>
    /// <param name="item">The object to locate.</param>
    /// <returns>
    /// <c>true</c> if item is found in the collection; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(T item) => _data.Contains(item);

    /// <summary>
    /// Copies the elements of the collection to an Array, starting at a particular array index.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional Array that is the destination of the elements copied from collection. The Array must have
    /// zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
      var i = 0;
      foreach (var item in _data)
      {
        array[arrayIndex + i] = item;
        ++i;
      }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the collection.
    /// </returns>
    public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();

    /// <summary>
    /// Removes the specified object from the collection.
    /// </summary>
    /// <param name="item">The object to remove.</param>
    /// <returns>
    /// <c>true</c> if item was successfully removed from the collection; otherwise, <c>false</c>. This method also
    /// returns <c>false</c> if item is not found in the original collection.
    /// </returns>
    public bool Remove(T item)
    {
      var done = false;
      bool result = false;
      while (!done)
      {
        var oldData = _data;
        var newData = oldData.Remove(item);
        result = oldData.Count > newData.Count;
        done = oldData == Interlocked.CompareExchange(ref _data, newData, oldData);
      }
      return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}