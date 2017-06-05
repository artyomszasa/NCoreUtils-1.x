using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Boolean" /> serializer.
  /// </summary>
  public sealed class BooleanSerializer : TypeSerializer<bool>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Boolean" /> serializer.
    /// </summary>
    public BooleanSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override bool Read(IDataReader reader, IServiceProvider serviceProvider) => reader.ReadBoolean();
    /// <inheritdoc />
    public override void Write(bool obj, IDataWriter writer, IServiceProvider serviceProvider) => writer.WriteValue(obj);
  }
}