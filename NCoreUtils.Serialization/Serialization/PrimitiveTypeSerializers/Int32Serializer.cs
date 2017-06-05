using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Int32" /> serializer.
  /// </summary>
  public sealed class Int32Serializer : TypeSerializer<int>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Int32" /> serializer.
    /// </summary>
    public Int32Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override int Read(IDataReader reader, IServiceProvider userState) => reader.ReadInt32();
    /// <inheritdoc />
    public override void Write(int obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}