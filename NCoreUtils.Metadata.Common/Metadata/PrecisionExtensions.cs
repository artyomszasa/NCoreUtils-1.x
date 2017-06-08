namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions related to <see cref="T:NCoreUtils.Metadata.Precision" />.
    /// </summary>
    public static class PrecisionExtensions
    {
        /// <summary>
        /// Adds precision to the builder.
        /// </summary>
        /// <param name="builder">Builder to add precision to.</param>
        /// <param name="precision">Precision value.</param>
        /// <param name="scale">Scale value.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder HasPrecision<TBuilder>(this TBuilder builder, byte precision, byte scale)
            where TBuilder : ICompositeMetdataBuilder
            => builder.SetMetadata(new Precision(precision, scale));
        /// <summary>
        /// Retrieves precision and scale values if set.
        /// </summary>
        /// <param name="compositeData">Composite metadata source.</param>
        /// <returns>Either precision and scale or <c>null</c></returns>
        public static (byte, byte)? Precision(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata<Precision>(out var precision) ? ((byte, byte)?)(precision.PrecisionValue, precision.Scale) : null;
    }
}