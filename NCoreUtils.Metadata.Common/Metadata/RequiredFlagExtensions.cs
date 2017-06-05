namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions related to <see cref="T:NCoreUtils.Metadata.RequiredFlag" />.
    /// </summary>
    public static class RequiredFlagExtensions
    {
        /// <summary>
        /// Adds required flag to the builder.
        /// </summary>
        /// <param name="builder">Builder to add required flag to.</param>
        /// <param name="value">Value of the flag.</param>
        /// <returns>Target builder.</returns>
        public static TBuilder IsRequired<TBuilder>(this TBuilder builder, bool value = true) where TBuilder : ICompositeMetdataBuilder
        {
            builder.SetMetadata(new RequiredFlag(value));
            return builder;
        }
        /// <summary>
        /// Determines whether required flag has been set on the target composite data.
        /// </summary>
        /// <param name="compositeData">Target composite data.</param>
        /// <returns>
        /// <c>true</c> if required flag has been set, <c>false</c> otherwise.
        /// </returns>
        public static bool IsRequired(this ICompositeMetadata compositeData)
            => compositeData.TryGetMetadata(out RequiredFlag flag) && flag.Value;
    }
}