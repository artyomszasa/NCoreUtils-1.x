using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Simple metadata that consists of a single key and value.
    /// </summary>
    /// <typeparam name="T">Type of the metadata value.</typeparam>
    public abstract class MetadataValue<T> : IMetadata, IEquatable<MetadataValue<T>>
    {
        int IMetadata.Count => 1;
        /// <summary>
        /// The value.
        /// </summary>
        public T Value { get; private set; }
        /// <summary>
        /// The key.
        /// </summary>
        public CaseInsensitive Key { get; private set; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.MetadataValue{T}" />.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="dataKey">Key.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MetadataValue(T value, CaseInsensitive dataKey)
        {
            Value = value;
            Key = dataKey;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
            => SingletonEnumerator.Create(new KeyValuePair<CaseInsensitive, object>(Key, Value));
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        IEnumerator<KeyValuePair<CaseInsensitive, object>> IEnumerable<KeyValuePair<CaseInsensitive, object>>.GetEnumerator()
            => GetEnumerator();
        /// <summary>
        /// Returns value of the actual instance if the specified key equals to the one stored in the actual instance.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Variable to return the value to.</param>
        /// <returns>
        /// <c>true</c> if value has been found, <c>false</c> otherwise.
        /// </returns>
        public bool TryGetValue(CaseInsensitive key, out object value)
        {
            if (Key == key)
            {
                value = Value;
                return true;
            }
            value = null;
            return false;
        }
        #region equality
        /// <summary>
        /// Implements equality check so that two instances of <see cref="T:NCoreUtils.Metadata.MetadataValue{T}" />
        /// are considered equal if both data keys and values are equal.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(MetadataValue<T> other)
            => null != other && EqualityComparer<T>.Default.Equals(Value, other.Value);
        /// <summary>
        /// Overrides equality check so that two instances of <see cref="T:NCoreUtils.Metadata.MetadataValue{T}" />
        /// are considered equal if both data keys and values are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => obj is MetadataValue<T> metavalue && Equals(metavalue);
        /// <summary>
        /// Returns composite hash generated from data key and value.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode() => Key.GetHashCode() ^ EqualityComparer<T>.Default.GetHashCode(Value);
        #endregion
    }
}