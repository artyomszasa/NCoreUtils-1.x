using System;

namespace NCoreUtils.Serialization.PrimitiveTypeSerializers
{
  /// <summary>
  /// Built-in <see cref="T:System.Single" /> serializer.
  /// </summary>
  public sealed class SingleSerializer : TypeSerializer<float>
  {
    /// <summary>
    /// Initializes new instance of built-in <see cref="T:System.Single" /> serializer.
    /// </summary>
    public SingleSerializer(TypeSerializerFactory factory) : base(factory) { }
    /// <inheritdoc />
    public override float Read(IDataReader reader, IServiceProvider userState) => reader.ReadSingle();
    /// <inheritdoc />
    public override void Write(float obj, IDataWriter writer, IServiceProvider userState) => writer.WriteValue(obj);
  }
}