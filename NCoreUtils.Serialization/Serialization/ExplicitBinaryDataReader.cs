using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Implements bonary data reader that reads form explicit byte array.
    /// </summary>
    public sealed class ExplicitBinaryDataReader : IBinaryDataReader
    {
        readonly byte[] _data;
        /// <inheritdoc />
        public int Length => _data.Length;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.ExplicitBinaryDataReader" /> from the
        /// specified byte array.
        /// </summary>
        /// <param name="data">Explicit byte array.</param>
        public ExplicitBinaryDataReader(byte[] data)
        {
            RuntimeAssert.ArgumentNotNull(data, nameof(data));
            _data = data;
        }
        /// <inheritdoc />
        public void CopyTo(int sourceOffset, byte[] target, int targetOffset, int count = -1)
        {
            int c;
            if (-1 == count)
            {
                if (sourceOffset + count > Length)
                {
                    throw new InvalidOperationException("Insufficient data");
                }
                c = count;
            }
            else
            {
                c = Length - sourceOffset;
            }
            if (target.Length - targetOffset > c)
            {
                throw new ArgumentException(nameof(target), "Insufficient buffer");
            }
            Buffer.BlockCopy(_data, sourceOffset, target, targetOffset, c);
        }
    }
}