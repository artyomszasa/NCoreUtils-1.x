using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NCoreUtils.Collections
{
  /// <summary>
  /// Various collection extensions.
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Creates new array of specified size and initializes the array by invoking <paramref name="generator" /> for
    /// each item.
    /// </summary>
    /// <param name="size">Size of array to be created.</param>
    /// <param name="generator">Initialization function.</param>
    /// <returns>Initialized array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T[] InitArray<T>(int size, Func<int, T> generator)
    {
      RuntimeAssert.ArgumentNotNull(generator, nameof(generator));
      var result = new T[size];
      for (var i = 0; i < result.Length; ++i)
      {
        result[i] = generator(i);
      }
      return result;
    }

    /// <summary>
    /// Sets <paramref name="array" /> as the value for all the elements in the array object.
    /// </summary>
    /// <param name="array">Array to fill.</param>
    /// <param name="value">Value to fill array with.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static void Fill<T>(this T[] array, T value = default(T))
    {
      RuntimeAssert.ArgumentNotNull(array, nameof(array));
      for (int i = 0; i < array.Length; ++i)
      {
        array[i] = value;
      }
    }

    /// <summary>
    /// Creates new array and copies specified slice of the array into the newly created array.
    /// </summary>
    /// <param name="array">Source array.</param>
    /// <param name="offset">Index of the first copied item.</param>
    /// <param name="count">Copied items count.</param>
    /// <returns>Newly created array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T[] Slice<T>(this T[] array, int offset, int count = -1)
    {
      RuntimeAssert.ArgumentNotNull(array, nameof(array));
      if (-1 == count)
      {
        count = array.Length - offset;
      }
      var result = new T[count];
      Array.Copy(array, offset, result, 0, count);
      return result;
    }

    /// <summary>
    /// Projects each element of a sequence into a new form. Provides array-optimized version of
    /// <see cref="M:System.Linq.Enumerable.Select" />.
    /// </summary>
    /// <param name="source">An array of values to invoke a transform function on.</param>
    /// <param name="conversion">A transform function to apply to each element.</param>
    /// <returns>
    /// An array whose elements are the result of invoking the transform function on each element of
    /// <paramref name="source" />.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static TRes[] Map<TItem, TRes>(this TItem[] source, Func<TItem, TRes> conversion)
      => InitArray(source.Length, i => conversion(source[i]));

    /// <summary>
    /// Projects each element of a sequence into a new form. Provides array-optimized version of
    /// <see cref="M:System.Linq.Enumerable.Select" />.
    /// </summary>
    /// <param name="source">An array of values to invoke a transform function on.</param>
    /// <param name="conversion">A transform function to apply to each element.</param>
    /// <returns>
    /// An array whose elements are the result of invoking the transform function on each element of
    /// <paramref name="source" />.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static TRes[] Map<TItem, TRes>(this TItem[] source, Func<TItem, int, TRes> conversion)
      => InitArray(source.Length, i => conversion(source[i], i));

    /// <summary>
    /// Executes a provided action once for each array element.
    /// </summary>
    /// <param name="source">Source array.</param>
    /// <param name="action">Action to execute.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (var item in source)
      {
        action(item);
      }
    }

    /// <summary>
    /// Executes a provided action once for each array element.
    /// </summary>
    /// <param name="source">Source array.</param>
    /// <param name="action">Action to execute.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
      var i = 0;
      foreach (var item in source)
      {
        action(item, i++);
      }
    }

    /// <summary>
    /// Returns the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </summary>
    /// <param name="source">Source dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">Optional value to return as default value.</param>
    /// <returns>
    /// Either the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue))
    {
      TValue value;
      if (!source.TryGetValue(key, out value))
      {
        return defaultValue;
      }
      return value;
    }

    /// <summary>
    /// Returns the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </summary>
    /// <param name="source">Source dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">Optional value to return as default value.</param>
    /// <returns>
    /// Either the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyMultiDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue))
    {
      TValue value;
      if (!source.TryGetValue(key, out value))
      {
        return defaultValue;
      }
      return value;
    }

    /// <summary>
    /// Returns the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </summary>
    /// <param name="source">Source dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="valueFactory">Value factory invoked to provide default value.</param>
    /// <returns>
    /// Either the value associated with the specified key, or a default value if the dictionary has no value
    /// assiciated to the key.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static TValue GetOrSupply<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<TValue> valueFactory)
    {
      TValue value;
      if (!source.TryGetValue(key, out value))
      {
        return valueFactory();
      }
      return value;
    }

    /// <summary>
    /// Removes and returns the last item of the specified list.
    /// </summary>
    /// <param name="list">Source list.</param>
    /// <returns>Former last item of the list.</returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if list is empty.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T Pop<T>(this List<T> list)
    {
      RuntimeAssert.ArgumentNotNull(list, nameof(list));
      if (0 == list.Count)
      {
        throw new InvalidOperationException("List is empty.");
      }
      var index = list.Count - 1;
      var result = list[index];
      list.RemoveAt(index);
      return result;
    }
  }
}