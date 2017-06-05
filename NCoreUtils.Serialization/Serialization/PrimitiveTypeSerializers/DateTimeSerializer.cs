using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.DateTime" /> serializer.
  /// </summary>
  public sealed class DateTimeSerializer : TypeSerializer<DateTime>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.DateTime" /> serializer.
    /// </summary>
    public DateTimeSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override DateTime Read(IDataReader reader, IServiceProvider userState) => reader.ReadDateTime();
    /// <inheritdoc />
    public override void Write(DateTime obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}