using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Int64" /> serializer.
  /// </summary>
  public sealed class Int64Serializer : TypeSerializer<long>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Int64" /> serializer.
    /// </summary>
    public Int64Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override long Read(IDataReader reader, IServiceProvider userState) => reader.ReadInt64();
    /// <inheritdoc />
    public override void Write(long obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}