using System.Collections;
using System.Collections.Generic;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents length constraints as part of some composite data.
    /// </summary>
    public class StringLength : IMetadata
    {
        /// <summary>
        /// Key of the maximum string length.
        /// </summary>
        public static CaseInsensitive KeyMaxLength { get; } = "MaxLength";
        /// <summary>
        /// Key of the minimum string length.
        /// </summary>
        public static CaseInsensitive KeyMinLength { get; } = "MinLength";
        int IMetadata.Count => MinLength.HasValue ? 2 : 1;
        /// <summary>
        /// Maximum string length value.
        /// </summary>
        public int MaxLength { get; private set; }
        /// <summary>
        /// Optional minimum string length value.
        /// </summary>
        public int? MinLength { get; private set; }
        /// <summary>
        /// Gets whether maximum and minimum lengths are equal.
        /// </summary>
        public bool FixedLength => MinLength.HasValue && MinLength.Value == MaxLength;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.StringLength" /> with the specified values.
        /// </summary>
        /// <param name="maxLength">Maximum length.</param>
        /// <param name="minLength">Minimum length.</param>
        public StringLength(int maxLength, int? minLength = null)
        {
            MaxLength = maxLength;
            MinLength = minLength;
        }
        IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
        {
            yield return new KeyValuePair<CaseInsensitive, object>(KeyMaxLength, MaxLength);
            if (MinLength.HasValue)
            {
                yield return new KeyValuePair<CaseInsensitive, object>(KeyMinLength, MinLength.Value);
            }
        }
        bool IMetadata.TryGetValue(CaseInsensitive key, out object value)
        {
            if (KeyMaxLength == key)
            {
                value = MaxLength;
                return true;
            }
            if (KeyMinLength == key && MinLength.HasValue)
            {
                value = MinLength.Value;
                return true;
            }
            value = default(object);
            return false;
        }
        IEnumerator<KeyValuePair<CaseInsensitive, object>> IEnumerable<KeyValuePair<CaseInsensitive, object>>.GetEnumerator()
            => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}