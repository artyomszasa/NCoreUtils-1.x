using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.DateTimeOffset" /> serializer.
  /// </summary>
  public sealed class DateTimeOffsetSerializer : TypeSerializer<DateTimeOffset>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.DateTimeOffset" /> serializer.
    /// </summary>
    public DateTimeOffsetSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override DateTimeOffset Read(IDataReader reader, IServiceProvider userState) => reader.ReadDateTimeOffset();
    /// <inheritdoc />
    public override void Write(DateTimeOffset obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}