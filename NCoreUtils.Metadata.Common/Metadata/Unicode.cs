namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents unicode flag as part of some composite data.
    /// </summary>
    public sealed class Unicode : MetadataFlag
    {
        /// <summary>
        /// Key of the unicode flag within dictionary representation of composite data.
        /// </summary>
        public static CaseInsensitive KeyUnicode { get; } = "Unicode";
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.Unicode" /> with the specified boolean value.
        /// </summary>
        /// <param name="value">The value of the flag.</param>
        public Unicode(bool value) : base(value, KeyUnicode) { }
    }
}