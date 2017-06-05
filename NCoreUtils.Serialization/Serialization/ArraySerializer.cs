using System;
using System.Collections.Generic;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Default implementation of array serializer.
    /// </summary>
    public class ArraySerializer<T> : TypeSerializer<T[]>
    {
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.ArraySerializer{T}" />.
        /// </summary>
        /// <param name="factory">Type serializer factory to use.</param>
        public ArraySerializer(ITypeSerializerFactory factory) : base(factory) { }

        T[] ReadWithLength(IArrayDataReader reader, int arrayLength, IServiceProvider serviceProvider)
        {
            var itemSerializer = Factory.GetSerializer<T>(serviceProvider);
            var array = new T[arrayLength];
            for (var i = 0; i < arrayLength; ++i)
            {
                if (!reader.MoveNext())
                {
                    throw new SerializationException("Unexpected end of array");
                }
                array[i] = itemSerializer.Read(reader, serviceProvider);
            }
            return array;
        }

        T[] ReadWithoutLength(IArrayDataReader reader, IServiceProvider serviceProvider)
        {
            var itemSerializer = Factory.GetSerializer<T>(serviceProvider);
            var buffer = new List<T>();
            while (reader.MoveNext())
            {
                buffer.Add(itemSerializer.Read(reader, serviceProvider));
            }
            return buffer.ToArray();
        }
        /// <inheritdoc />
        public override void Write(T[] array, IDataWriter writer, IServiceProvider serviceProvider)
        {
            var itemSerializer = Factory.GetSerializer<T>(serviceProvider);
            writer.WriteArrayStart();
            var arrayLength = array.Length;
            for (var i = 0; i < arrayLength; ++i)
            {
                itemSerializer.WriteObject(array[i], writer, serviceProvider);
            }
            writer.WriteArrayEnd();
        }
        /// <inheritdoc />
        public override T[] Read(IDataReader dataReader, IServiceProvider serviceProvider)
        {
            var reader = dataReader.ReadArray();
            int arrayLength;
            return reader.TryGetLength(out arrayLength) ? ReadWithLength(reader, arrayLength, serviceProvider) : ReadWithoutLength(reader, serviceProvider);
        }
    }

    /// <summary>
    /// Provides functionality to instantiate default array serializers.
    /// </summary>
    // FIXME: DI array serializers?
    public static class ArraySerializerFactory
    {
        /// <summary>
        /// Instantiates default array serializer for the specified element type.
        /// </summary>
        /// <param name="factory">Type serializer factory.</param>
        /// <param name="elementType">Array element type.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Initialized default array serializer.</returns>
        public static TypeSerializer Create(TypeSerializerFactory factory, Type elementType, IServiceProvider serviceProvider)
        {
            var serializerType = typeof(ArraySerializer<>).MakeGenericType(elementType);
            return (TypeSerializer)Activator.CreateInstance(serializerType, factory);
        }
    }
}