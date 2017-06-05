using System.Runtime.CompilerServices;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Simple metadata that consists of a single key and boolean value.
    /// </summary>
    public abstract class MetadataFlag : MetadataValue<bool>
    {
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.MetadataFlag" />.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="dataKey">Key.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MetadataFlag(bool value, CaseInsensitive dataKey) : base(value, dataKey) { }
    }
}