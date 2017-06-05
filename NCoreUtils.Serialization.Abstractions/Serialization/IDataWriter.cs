using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines low level interface for writing output.
    /// </summary>
    public interface IDataWriter : IDisposable
    {
        /// <summary>
        /// Writes <c>null</c> to the output.
        /// </summary>
        void WriteNull();
        /// <summary>
        /// Writes signed byte to the output.
        /// </summary>
        void WriteValue(sbyte value);
        /// <summary>
        /// Writes short to the output.
        /// </summary>
        void WriteValue(short value);
        /// <summary>
        /// Writes int to the output.
        /// </summary>
        void WriteValue(int value);
        /// <summary>
        /// Writes long to the output.
        /// </summary>
        void WriteValue(long value);
        /// <summary>
        /// Writes unsigned byte to the output.
        /// </summary>
        void WriteValue(byte value);
        /// <summary>
        /// Writes unsigned short to the output.
        /// </summary>
        void WriteValue(ushort value);
        /// <summary>
        /// Writes unsigned int to the output.
        /// </summary>
        void WriteValue(uint value);
        /// <summary>
        /// Writes unsigned long to the output.
        /// </summary>
        void WriteValue(ulong value);
        /// <summary>
        /// Writes float to the output.
        /// </summary>
        void WriteValue(float value);
        /// <summary>
        /// Writes double to the output.
        /// </summary>
        void WriteValue(double value);
        /// <summary>
        /// Writes boolean to the output.
        /// </summary>
        void WriteValue(bool value);
        /// <summary>
        /// Writes decimal to the output.
        /// </summary>
        void WriteValue(decimal value);
        /// <summary>
        /// Writes char to the output.
        /// </summary>
        void WriteValue(char value);
        /// <summary>
        /// Writes string to the output.
        /// </summary>
        void WriteValue(string value);
        /// <summary>
        /// Writes timestamp to the output.
        /// </summary>
        void WriteValue(DateTime value);
        /// <summary>
        /// Writes timestamp with timezone to the output.
        /// </summary>
        void WriteValue(DateTimeOffset value);
        /// <summary>
        /// Writes Guid to the output.
        /// </summary>
        void WriteValue(Guid value);
        /// <summary>
        /// Writes binary data to the output.
        /// </summary>
        void WriteBinary(byte[] data);
        /// <summary>
        /// Writes array start to the output.
        /// </summary>
        void WriteArrayStart();
        /// <summary>
        /// Writes array end to the output.
        /// </summary>
        void WriteArrayEnd();
        /// <summary>
        /// Writes object start to the output.
        /// </summary>
        void WriteObjectStart(string typeName);
        /// <summary>
        /// Writes member name to the output.
        /// </summary>
        void WriteMemberName(string propertyName);
        /// <summary>
        /// Writes object end to the output.
        /// </summary>
        void WriteObjectEnd();
    }
}