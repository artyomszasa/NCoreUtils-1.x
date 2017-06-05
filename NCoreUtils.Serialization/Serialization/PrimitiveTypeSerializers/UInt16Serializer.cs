using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.UInt16" /> serializer.
  /// </summary>
  public sealed class UInt16Serializer : TypeSerializer<ushort>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.UInt16" /> serializer.
    /// </summary>
    public UInt16Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override ushort Read(IDataReader reader, IServiceProvider userState) => reader.ReadUInt16();
    /// <inheritdoc />
    public override void Write(ushort obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}