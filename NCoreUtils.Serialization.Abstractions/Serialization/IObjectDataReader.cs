namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to read nested object from the input.
    /// </summary>
    public interface IObjectDataReader : IDataReader
    {
        /// <summary>
        /// Checks whether the input contains null value.
        /// </summary>
        bool IsNull { get; }
        /// <summary>
        /// Reads type name of the object.
        /// </summary>
        string TypeName { get; }
        /// <summary>
        /// Gets the actual member name.
        /// </summary>
        string CurrentMemberName { get; }
        /// <summary>
        /// Advances the reader position to the next member.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the input contains hext member, <c>false</c> otherwise.
        /// </returns>
        bool MoveNext();
    }
}