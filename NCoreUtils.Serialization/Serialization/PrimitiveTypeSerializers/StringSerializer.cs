using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.String" /> serializer.
  /// </summary>
  public sealed class StringSerializer : TypeSerializer<string>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.String" /> serializer.
    /// </summary>
    public StringSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override string Read(IDataReader reader, IServiceProvider userState) => reader.ReadString();
    /// <inheritdoc />
    public override void Write(string obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}