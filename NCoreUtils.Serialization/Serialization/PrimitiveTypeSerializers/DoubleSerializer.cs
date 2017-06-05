using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Double" /> serializer.
  /// </summary>
  public sealed class DoubleSerializer : TypeSerializer<double>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Double" /> serializer.
    /// </summary>
    public DoubleSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override double Read(IDataReader reader, IServiceProvider userState) => reader.ReadDouble();
    /// <inheritdoc />
    public override void Write(double obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}