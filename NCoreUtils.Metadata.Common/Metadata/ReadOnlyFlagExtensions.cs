namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions related to <see cref="T:NCoreUtils.Metadata.ReadOnlyFlag" />.
    /// </summary>
    public static class ReadOnlyFlagExtensions
    {
        /// <summary>
        /// Adds read-only flag to the builder.
        /// </summary>
        /// <param name="builder">Builder to add read-only flag to.</param>
        /// <param name="value">Value of the flag.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder IsReadOnly<TBuilder>(this TBuilder builder, bool value = true) where TBuilder : ICompositeMetdataBuilder
        {
            builder.SetMetadata(new ReadOnlyFlag(value));
            return builder;
        }
        /// <summary>
        /// Determines whether read-only flag has been set on the target composite data.
        /// </summary>
        /// <param name="compositeData">Target composite data.</param>
        /// <returns>
        /// <c>true</c> if read-only flag has been set, <c>false</c> otherwise.
        /// </returns>
        public static bool IsReadOnly(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata(out ReadOnlyFlag flag) && flag.Value;
    }
}