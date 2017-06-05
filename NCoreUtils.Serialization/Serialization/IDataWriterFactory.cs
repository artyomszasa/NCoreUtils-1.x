using System.IO;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines functionality to create data writers from streams.
    /// </summary>
    public interface IDataWriterFactory
    {
        /// <summary>
        /// Creates data writer from stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <returns>Data writer.</returns>
        IDataWriter Create(Stream stream);
    }
}