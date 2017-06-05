using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.SByte" /> serializer.
  /// </summary>
  public sealed class SByteSerializer : TypeSerializer<sbyte>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.SByte" /> serializer.
    /// </summary>
    public SByteSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override sbyte Read(IDataReader reader, IServiceProvider userState) => reader.ReadSByte();
    /// <inheritdoc />
    public override void Write(sbyte obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}