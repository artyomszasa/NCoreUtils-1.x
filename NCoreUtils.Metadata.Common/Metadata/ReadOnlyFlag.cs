namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents read-only flag as part of some composite data.
    /// </summary>
    public sealed class ReadOnlyFlag : MetadataFlag
    {
        /// <summary>
        /// Key of the read-only flag within dictionary representation of composite data.
        /// </summary>
        public static CaseInsensitive KeyReadOnly { get; } = "ReadOnly";
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.ReadOnlyFlag" /> with the specified boolean value.
        /// </summary>
        /// <param name="value">The value of the flag.</param>
        public ReadOnlyFlag(bool value) : base(value, KeyReadOnly) { }
    }
}