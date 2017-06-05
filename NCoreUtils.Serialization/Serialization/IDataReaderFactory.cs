using System.IO;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to create data readers from streams.
    /// </summary>
    public interface IDataReaderFactory
    {
        /// <summary>
        /// Creates data reader from stream.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>Data reader.</returns>
        IDataReader Create(Stream stream);
    }
}