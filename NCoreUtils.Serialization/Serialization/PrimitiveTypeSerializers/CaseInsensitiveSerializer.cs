using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:NCoreUtils.CaseInsensitive" /> serializer.
  /// </summary>
  public sealed class CaseInsensitiveSerializer : TypeSerializer<CaseInsensitive>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:NCoreUtils.CaseInsensitive" /> serializer.
    /// </summary>
    public CaseInsensitiveSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override CaseInsensitive Read(IDataReader reader, IServiceProvider userState) => reader.ReadString();
    /// <inheritdoc />
    public override void Write(CaseInsensitive obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj.Value);
  }
}