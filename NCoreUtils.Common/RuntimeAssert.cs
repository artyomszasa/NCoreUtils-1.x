using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NCoreUtils
{
  /// <summary>
  /// Defines default runtime assertions.
  /// </summary>
  public static class RuntimeAssert
  {
    /// <summary>
    /// Throws if specified argument is null.
    /// </summary>
    /// <param name="obj">Argument value.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentNotNull(object obj, string name)
    {
      if (null == obj)
      {
        throw new ArgumentNullException(name);
      }
    }

    /// <summary>
    /// Throws if specified arguments are not equal.
    /// </summary>
    /// <param name="a">First argument value.</param>
    /// <param name="b">Second argument value.</param>
    /// <param name="aName">First argument name.</param>
    /// <param name="bName">Second argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Equals(int a, int b, string aName, string bName)
    {
      if (a != b)
      {
        throw new InvalidOperationException(string.Format("{0} must be equal to {1}", aName, bName));
      }
    }

    /// <summary>
    /// Throws if first argument is less than the second one.
    /// </summary>
    /// <param name="a">First argument value.</param>
    /// <param name="b">Second argument value.</param>
    /// <param name="aName">First argument name.</param>
    /// <param name="bName">Second argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GreaterOrEquals(int a, int b, string aName, string bName)
    {
      if (a < b)
      {
        throw new ArgumentException(string.Format("{0} must be greater than or equal to {1}", aName, bName));
      }
    }

    /// <summary>
    /// Throws if first argument is greater than or equals to the second one.
    /// </summary>
    /// <param name="a">First argument value.</param>
    /// <param name="b">Second argument value.</param>
    /// <param name="aName">First argument name.</param>
    /// <param name="bName">Second argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Less(int a, int b, string aName, string bName)
    {
      if (a >= b)
      {
        throw new ArgumentException(string.Format("{0} must be less than {1}", aName, bName));
      }
    }

    /// <summary>
    /// Throws if the specified argument is not within a range.
    /// </summary>
    /// <param name="value">Argument value.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IndexInRange(int value, int min, int max, string name)
    {
      if (value < min || value > max)
      {
        throw new IndexOutOfRangeException(string.Format("{0} must be in range [{1}, {2}] but is {3}", name, min, max, value));
      }
    }

    /// <summary>
    /// Throws if the specified argument is not within a range.
    /// </summary>
    /// <param name="value">Argument value.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentInRange(int value, int min, int max, string name)
    {
      if (value < min || value > max)
      {
        throw new ArgumentOutOfRangeException(string.Format("{0} must be in range [{1}, {2}] but is {3}", name, min, max, value));
      }
    }

    /// <summary>
    /// Throws if the specified argument is not within a range.
    /// </summary>
    /// <param name="value">Argument value.</param>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentInRange(long value, long min, long max, string name)
    {
      if (value < min || value > max)
      {
        throw new ArgumentOutOfRangeException(string.Format("{0} must be in range [{1}, {2}] but is {3}", name, min, max, value));
      }
    }
    /// <summary>
    /// Throws if the specified array is empty.
    /// </summary>
    /// <param name="array">Array to check.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentNotEmpty<T>(T[] array, string name)
    {
      if (0 == array.Length)
      {
        throw new ArgumentException($"{name} must not be empty.", name);
      }
    }
    /// <summary>
    /// Throws if the specified container is empty.
    /// </summary>
    /// <param name="collection">Container to check.</param>
    /// <param name="name">Argument name.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentNotEmpty<T>(IReadOnlyCollection<T> collection, string name)
    {
      if (0 == collection.Count)
      {
        throw new ArgumentException($"{name} must not be empty.", name);
      }
    }
  }
}