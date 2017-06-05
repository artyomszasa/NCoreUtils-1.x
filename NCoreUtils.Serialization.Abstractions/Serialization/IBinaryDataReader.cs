namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to read binary data from the input.
    /// </summary>
    public interface IBinaryDataReader
    {
        /// <summary>
        /// Gets the length of the binary data.
        /// </summary>
        int Length { get; }
        /// <summary>
        /// Copies part of the binary data to the specified buffer at the specified position.
        /// </summary>
        /// <param name="sourceOffset">Offset to start copying from.</param>
        /// <param name="target">Target buffer.</param>
        /// <param name="targetOffset">Offset to start emplacing data to.</param>
        /// <param name="count">Maximum count of bytes to copy.</param>
        void CopyTo(int sourceOffset, byte[] target, int targetOffset, int count = -1);
    }
}