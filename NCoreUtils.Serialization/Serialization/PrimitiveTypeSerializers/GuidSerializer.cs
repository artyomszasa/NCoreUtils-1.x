using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Guid" /> serializer.
  /// </summary>
  public sealed class GuidSerializer : TypeSerializer<Guid>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Guid" /> serializer.
    /// </summary>
    public GuidSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override Guid Read(IDataReader reader, IServiceProvider userState) => reader.ReadGuid();
    /// <inheritdoc />
    public override void Write(Guid obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}