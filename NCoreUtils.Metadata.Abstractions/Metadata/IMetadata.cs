using System;
using System.Collections.Generic;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Provides metadata object functionality.
    /// </summary>
    public interface IMetadata : IEnumerable<KeyValuePair<CaseInsensitive, object>>
    {
        /// <summary>
        /// Gets stored objects count.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Searches for the specified data key and if found returns object associated.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Variable to return the object to.</param>
        /// <returns>
        /// <c>true</c> if object has been found, <c>false</c> otherwise.
        /// </returns>
        bool TryGetValue(CaseInsensitive key, out object value);
    }
}
