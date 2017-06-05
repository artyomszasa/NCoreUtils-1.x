using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Char" /> serializer.
  /// </summary>
  public sealed class CharSerializer : TypeSerializer<char>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Char" /> serializer.
    /// </summary>
    public CharSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override char Read(IDataReader reader, IServiceProvider userState) => reader.ReadChar();
    /// <inheritdoc />
    public override void Write(char obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}