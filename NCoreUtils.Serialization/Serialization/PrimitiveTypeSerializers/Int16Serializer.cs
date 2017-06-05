using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Int16" /> serializer.
  /// </summary>
  public sealed class Int16Serializer : TypeSerializer<short>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Int16" /> serializer.
    /// </summary>
    public Int16Serializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override short Read(IDataReader reader, IServiceProvider userState) => reader.ReadInt16();
    /// <inheritdoc />
    public override void Write(short obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}