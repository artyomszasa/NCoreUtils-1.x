using System.Runtime.CompilerServices;

namespace NCoreUtils
{
  /// <summary>
  /// Extensions of the <see cref="NCoreUtils.CaseInsensitive" />.
  /// </summary>
  public static class CaseInsensitiveExtensions
  {
    /// <summary>
    /// Retrieves a substring from this instance. The substring starts at a specified character position and continues
    /// to the end of the string.
    /// </summary>
    /// <param name="source">Source string.</param>
    /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
    /// <returns>
    /// A string that is equivalent to the substring that begins at <paramref name="startIndex" /> in this instance,
    /// or <c>Empty</c> if <paramref name="startIndex" /> is equal to the length of this instance.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CaseInsensitive Substring(this CaseInsensitive source, int startIndex) => CaseInsensitive.Create(source.Value.Substring(startIndex));
    /// <summary>
    /// Retrieves a substring from this instance. The substring starts at a specified character position and has a
    /// specified length.
    /// </summary>
    /// <param name="source">Source string.</param>
    /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <returns>
    /// A string that is equivalent to the substring of length length that begins at <paramref name="startIndex" /> in
    /// this instance, or <c>Empty</c> if <paramref name="startIndex" /> is equal to the length of this instance and
    /// length is zero.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CaseInsensitive Substring(this CaseInsensitive source, int startIndex, int length) => CaseInsensitive.Create(source.Value.Substring(startIndex, length));
  }
}