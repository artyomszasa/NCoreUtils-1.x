using System.Collections;
using System.Collections.Generic;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents floating numbers precision as part of some composite data.
    /// </summary>
    public class Precision : IMetadata
    {
        /// <summary>
        /// Key of the floating numbers precision.
        /// </summary>
        public static CaseInsensitive KeyPrecision { get; } = "Precision";
        /// <summary>
        /// Key of the floating numbers scale.
        /// </summary>
        public static CaseInsensitive KeyScale { get; } = "Scale";
        int IMetadata.Count => 2;
        /// <summary>
        /// Gets floating numbers precision.
        /// </summary>
        public byte PrecisionValue { get; private set; }
        /// <summary>
        /// Gets floating numbers scale.
        /// </summary>
        public byte Scale { get; private set; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.Precision" /> with the specified values.
        /// </summary>
        /// <param name="precisionValue">Precision value.</param>
        /// <param name="scale">Scale value.</param>
        public Precision(byte precisionValue, byte scale)
        {
            PrecisionValue = precisionValue;
            Scale = scale;
        }
        IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
        {
            var items = new [] {
                new KeyValuePair<CaseInsensitive, object>(KeyPrecision, PrecisionValue),
                new KeyValuePair<CaseInsensitive, object>(KeyScale, Scale),
            };
            return ((IEnumerable<KeyValuePair<CaseInsensitive, object>>)items).GetEnumerator();
        }
        bool IMetadata.TryGetValue(CaseInsensitive key, out object value)
        {
            if (KeyPrecision == key)
            {
                value = PrecisionValue;
                return true;
            }
            if (KeyScale == key)
            {
                value = Scale;
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