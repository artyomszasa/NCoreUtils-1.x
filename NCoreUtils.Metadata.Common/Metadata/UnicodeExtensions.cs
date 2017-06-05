namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions related to <see cref="T:NCoreUtils.Metadata.Unicode" />.
    /// </summary>
    public static class UnicodeExtensions
    {
        /// <summary>
        /// Adds unicode flag to the builder.
        /// </summary>
        /// <param name="builder">Builder to add unicode flag to.</param>
        /// <param name="isUnicode">Value of the flag.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder IsUnicode<TBuilder>(this TBuilder builder, bool isUnicode = true)
            where TBuilder : ICompositeMetdataBuilder
            => builder.SetMetadata(new Unicode(isUnicode));
        /// <summary>
        /// Determines whether unicode flag has been set on the target composite data.
        /// </summary>
        /// <param name="compositeData">Target composite data.</param>
        /// <returns>
        /// <c>true</c> if unicode flag has been set, <c>false</c> otherwise.
        /// </returns>
        public static bool? IsUnicode(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata<Unicode>(out var u) ? (bool?)u.Value : null;
    }
}