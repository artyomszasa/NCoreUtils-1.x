using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Byte" /> serializer.
  /// </summary>
  public sealed class ByteSerializer : TypeSerializer<byte>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Byte" /> serializer.
    /// </summary>
    public ByteSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override byte Read(IDataReader reader, IServiceProvider userState) => reader.ReadByte();
    /// <inheritdoc />
    public override void Write(byte obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}