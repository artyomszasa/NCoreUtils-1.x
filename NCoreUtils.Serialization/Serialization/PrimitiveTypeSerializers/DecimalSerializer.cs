using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Decimal" /> serializer.
  /// </summary>
  public sealed class DecimalSerializer : TypeSerializer<decimal>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Decimal" /> serializer.
    /// </summary>
    public DecimalSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override decimal Read(IDataReader reader, IServiceProvider userState) => reader.ReadDecimal();
    /// <inheritdoc />
    public override void Write(decimal obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}