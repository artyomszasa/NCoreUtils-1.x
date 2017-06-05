using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.UInt32" /> serializer.
  /// </summary>
  public sealed class UInt32Serializer : TypeSerializer<uint>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.UInt32" /> serializer.
    /// </summary>
    public UInt32Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override uint Read(IDataReader reader, IServiceProvider userState) => reader.ReadUInt32();
    /// <inheritdoc />
    public override void Write(uint obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}