namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents required flag as part of some composite data.
    /// </summary>
    public sealed class RequiredFlag : MetadataFlag
    {
        /// <summary>
        /// Key of the required flag within dictionary representation of composite data.
        /// </summary>
        public static CaseInsensitive KeyRequired { get; } = "Required";
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Metadata.RequiredFlag" /> with the specified boolean value.
        /// </summary>
        /// <param name="value">The value of the flag.</param>
        public RequiredFlag(bool value) : base(value, KeyRequired) { }
    }
}