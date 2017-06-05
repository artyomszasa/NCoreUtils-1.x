using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.UInt64" /> serializer.
  /// </summary>
  public sealed class UInt64Serializer : TypeSerializer<ulong>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.UInt64" /> serializer.
    /// </summary>
    public UInt64Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override ulong Read(IDataReader reader, IServiceProvider userState) => reader.ReadUInt64();
    /// <inheritdoc />
    public override void Write(ulong obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}