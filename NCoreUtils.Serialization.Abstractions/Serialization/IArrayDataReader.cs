namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines low level interface to deserialize array like objects.
    /// <para>
    /// Each implementation provides a way to iterate through serialized
    /// items using <see cref="M:NCoreUtils.Serialization.IArrayDataReader.MoveNext" />.
    /// Iteration is irreversible.
    /// </para>
    /// <para>
    /// Implementations should provide final item count retreival through
    /// <see cref="M:NCoreUtils.Serialization.IArrayDataReader.TryGetLength" />
    /// when possible to achieve best performance on deserialization.
    /// </para>
    /// </summary>
    public interface IArrayDataReader : IDataReader
    {
        /// <summary>
        /// Determines whether serialized value is <c>null</c>.
        /// <para>
        /// If the serialized value is <c>null</c> no other method of the
        /// instance must be called.
        /// </para>
        /// </summary>
        bool IsNull { get; }
        /// <summary>
        /// Gets the final collection size if specified.
        /// </summary>
        /// <param name="length">Variable to return the length to.</param>
        /// <returns>
        /// <c>true</c> if the length could be retrieved and has been stored into the specified variable, <c>false</c>
        /// otherwise.
        /// </returns>
        bool TryGetLength(out int length);
        /// <summary>
        /// Advances the reader position within the collection.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the collection has next element, <c>false</c> otherwise.
        /// </returns>
        bool MoveNext();
    }
}