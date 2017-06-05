using System;
using System.Text;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Commonly used functionality.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// UTF-8 encoding without BOM.
        /// </summary>
        public static readonly Encoding Utf8 = new UTF8Encoding(false);
    }
}