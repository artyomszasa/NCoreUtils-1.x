namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Contains extensions for <see cref="T:NCoreUtils.Metadata.ICompositeMetadata" />.
    /// </summary>
    public static class CompositeDataExtensions
    {
        /// <summary>
        /// Returns typed partial data if present.
        /// </summary>
        /// <param name="dataSource">Composite metadata source.</param>
        /// <param name="data">Variable to return metadata.</param>
        /// <returns>
        /// <c>true</c> if metadata of the specified type was present, <c>false</c> otherwise.
        /// </returns>
        public static bool TryGetMetadata<TMetadata>(this ICompositeMetadata dataSource, out TMetadata data) where TMetadata : IMetadata
        {
            RuntimeAssert.ArgumentNotNull(dataSource, nameof(dataSource));
            if (dataSource.TryGetMetadata(typeof(TMetadata), out var obj))
            {
                data = (TMetadata)obj;
                return true;
            }
            data = default(TMetadata);
            return false;
        }
        /// <summary>
        /// Returns either value of single key/value partial data defined by <typeparamref name="T" /> or default
        /// value.
        /// </summary>
        /// <param name="data">Composite data source.</param>
        /// <param name="defaultValue">Optional default value.</param>
        /// <returns>
        /// Either value of single key/value partial data defined by <typeparamref name="T" /> or default value.
        /// </returns>
        public static bool GetBooleanOrDefault<T>(this ICompositeMetadata data, bool defaultValue = default(bool))
            where T : MetadataValue<bool>
            => data.TryGetMetadata<T>(out var value) ? value.Value : defaultValue;
        /// <summary>
        /// Returns either value of single key/value partial data defined by <typeparamref name="T" /> or default
        /// value.
        /// </summary>
        /// <param name="data">Composite data source.</param>
        /// <param name="defaultValue">Optional default value.</param>
        /// <returns>
        /// Either value of single key/value partial data defined by <typeparamref name="T" /> or default value.
        /// </returns>
        public static string GetStringOrDefault<T>(this ICompositeMetadata data, string defaultValue = default(string))
            where T : MetadataValue<string>
            => data.TryGetMetadata<T>(out var value) ? value.Value : defaultValue;
        /// <summary>
        /// Returns either value of single key/value partial data defined by <typeparamref name="T" /> or default
        /// value.
        /// </summary>
        /// <param name="data">Composite data source.</param>
        /// <param name="defaultValue">Optional default value.</param>
        /// <returns>
        /// Either value of single key/value partial data defined by <typeparamref name="T" /> or default value.
        /// </returns>
        public static int GetInt32OrDefault<T>(this ICompositeMetadata data, int defaultValue = default(int))
            where T : MetadataValue<int>
            => data.TryGetMetadata<T>(out var value) ? value.Value : defaultValue;
    }
}