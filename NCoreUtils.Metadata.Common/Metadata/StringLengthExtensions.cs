namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions related to <see cref="T:NCoreUtils.Metadata.StringLength" />.
    /// </summary>
    public static class StringLengthExtensions
    {
        /// <summary>
        /// Adds string length constraints to the builder.
        /// </summary>
        /// <param name="builder">Builder to add string constaints to.</param>
        /// <param name="maxLength">Maximum string length value.</param>
        /// <param name="minLength">Minimum string length value.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder HasStringLength<TBuilder>(this TBuilder builder, int maxLength, int? minLength = null)
            where TBuilder : ICompositeMetdataBuilder
            => builder.SetMetadata(new StringLength(maxLength, minLength));
        /// <summary>
        /// Retrieves value of maximum string length constraint if any.
        /// </summary>
        /// <param name="compositeData">Composite metadata source.</param>
        /// <returns>Either maximum string length constraint or <c>null</c></returns>
        public static int? MaxLength(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata<StringLength>(out var stringLength) ? (int?)stringLength.MaxLength : null;
        /// <summary>
        /// Retrieves value of minimum string length constraint if any.
        /// </summary>
        /// <param name="compositeData">Composite metadata source.</param>
        /// <returns>Either minimum string length constraint or <c>null</c></returns>
        public static int? MinLength(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata<StringLength>(out var stringLength) ? stringLength.MinLength : null;
    }
}