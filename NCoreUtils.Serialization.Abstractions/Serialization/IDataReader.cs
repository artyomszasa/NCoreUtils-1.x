using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Defines low level interface for reading input.
    /// </summary>
    public interface IDataReader : IDisposable
    {
        /// <summary>
        /// Reads signed byte from the input.
        /// </summary>
        sbyte ReadSByte();
        /// <summary>
        /// Reads short from the input.
        /// </summary>
        short ReadInt16();
        /// <summary>
        /// Reads int from the input.
        /// </summary>
        int ReadInt32();
        /// <summary>
        /// Reads long from the input.
        /// </summary>
        long ReadInt64();
        /// <summary>
        /// Reads unsigned byte from the input.
        /// </summary>
        byte ReadByte();
        /// <summary>
        /// Reads unsigned short from the input.
        /// </summary>
        ushort ReadUInt16();
        /// <summary>
        /// Reads unsigned int from the input.
        /// </summary>
        uint ReadUInt32();
        /// <summary>
        /// Reads unsigned long from the input.
        /// </summary>
        ulong ReadUInt64();
        /// <summary>
        /// Reads float from the input.
        /// </summary>
        float ReadSingle();
        /// <summary>
        /// Reads double from the input.
        /// </summary>
        double ReadDouble();
        /// <summary>
        /// Reads boolean from the input.
        /// </summary>
        bool ReadBoolean();
        /// <summary>
        /// Reads decimal from the input.
        /// </summary>
        decimal ReadDecimal();
        /// <summary>
        /// Reads char from the input.
        /// </summary>
        char ReadChar();
        /// <summary>
        /// Reads serialized string. The returned value may be <c>null</c>.
        /// </summary>
        string ReadString();
        /// <summary>
        /// Reads datetime from the input.
        /// </summary>
        DateTime ReadDateTime();
        /// <summary>
        /// Reads datetime with timezone from the input.
        /// </summary>
        DateTimeOffset ReadDateTimeOffset();
        /// <summary>
        /// Reads Guid from the input.
        /// </summary>
        Guid ReadGuid();
        /// <summary>
        /// Initiates binary read on the input.
        /// </summary>
        IBinaryDataReader ReadBinary();
        /// <summary>
        /// Initiates collection read on the input.
        /// </summary>
        IArrayDataReader ReadArray();
        /// <summary>
        /// Initiates nested object read on the input.
        /// </summary>
        IObjectDataReader ReadObject();
    }
}