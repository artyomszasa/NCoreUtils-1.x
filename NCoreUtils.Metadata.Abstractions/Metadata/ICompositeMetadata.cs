using System;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Provides composite metadata functionality.
    /// </summary>
    public interface ICompositeMetadata : IMetadata
    {
        /// <summary>
        /// Returns typed metadata if present.
        /// </summary>
        /// <param name="metadataType">Metadata type.</param>
        /// <param name="metadata">Variable to return metadata.</param>
        /// <returns>
        /// <c>true</c> if metadata of the specified type was present, <c>false</c> otherwise.
        /// </returns>
        bool TryGetMetadata(Type metadataType, out IMetadata metadata);
    }
}