using System;
using System.IO;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Default implementation for serializing objects to and deserializing object from stream.
    /// </summary>
    public abstract class Serializer : ISerializer<Stream>, IDeserializer<Stream>
    {
        sealed class EmptyServiceProvider : IServiceProvider
        {
            public static EmptyServiceProvider SharedInstance { get; } = new EmptyServiceProvider();
            EmptyServiceProvider() { }
            public object GetService(Type serviceType) => null;
        }
        sealed class ExplicitSerializer : Serializer
        {
            public IDataReaderFactory DataReaderFactory { get; private set; }
            public IDataWriterFactory DataWriterFactory { get; private set; }

            public ExplicitSerializer(IServiceProvider serviceProvider, IDataReaderFactory dataReaderFactory, IDataWriterFactory dataWriterFactory, ITypeSerializerFactory typeSerializerFactory)
                : base(serviceProvider, typeSerializerFactory)
            {
                DataReaderFactory = dataReaderFactory;
                DataWriterFactory = dataWriterFactory;
            }

            protected override IDataReader CreateDataReader(Stream stream) => DataReaderFactory.Create(stream);

            protected override IDataWriter CreateDataWriter(Stream stream) => DataWriterFactory.Create(stream);

            public override Serializer Clone(ITypeSerializerFactory typeSerializerFactory) => new ExplicitSerializer(ServiceProvider, DataReaderFactory, DataWriterFactory, typeSerializerFactory);
        }
        /// <summary>
        /// Initializes new instance of default serializer with specified parameters.
        /// </summary>
        /// <param name="dataReaderFactory">Data reader factory.</param>
        /// <param name="dataWriterFactory">Data writer factory.</param>
        /// <param name="serviceProvider">Service provider. If not specified empty service provider is used.</param>
        /// <param name="typeSerializerFactory">Type serializer factory. If not specified new cached type serializer factory is used.</param>
        /// <returns>Serializer.</returns>
        public static Serializer Create(IDataReaderFactory dataReaderFactory, IDataWriterFactory dataWriterFactory, IServiceProvider serviceProvider = null, ITypeSerializerFactory typeSerializerFactory = null)
        {
            return new ExplicitSerializer(serviceProvider ?? EmptyServiceProvider.SharedInstance, dataReaderFactory, dataWriterFactory, typeSerializerFactory ?? new CachedTypeSerializerFactory());
        }

        /// <summary>
        /// Type serializer factory.
        /// </summary>
        public ITypeSerializerFactory TypeSerializerFactory { get; private set; }
        /// <summary>
        /// Internal service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.Serializer" /> with the specified
        /// parameters.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="typeSerializerFactory">Type serializer factory.</param>
        protected Serializer(IServiceProvider serviceProvider, ITypeSerializerFactory typeSerializerFactory)
        {
            ServiceProvider = serviceProvider;
            TypeSerializerFactory = typeSerializerFactory;
        }
        /// <summary>
        /// When overridden creates data reader from stream.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>Data reader.</returns>
        protected abstract IDataReader CreateDataReader(Stream stream);
        /// <summary>
        /// When overridden creates data writer from stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <returns>Data writer.</returns>
        protected abstract IDataWriter CreateDataWriter(Stream stream);
        /// <summary>
        /// Serializes object to the specified writer.
        /// </summary>
        /// <param name="writer">Data writer.</param>
        /// <param name="obj">Source object.</param>
        protected virtual void Serialize(IDataWriter writer, object obj)
        {
            if (null == obj)
            {
                writer.WriteNull();
            }
            else
            {
                TypeSerializerFactory.GetSerializer(obj.GetType(), ServiceProvider).WriteObject(obj, writer, ServiceProvider);
            }
        }
        /// <summary>
        /// Deserializes object from reader.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <param name="type">Target object type.</param>
        /// <returns>Deserialized object.</returns>
        protected virtual object Deserialize(IDataReader reader, Type type)
        {
            return TypeSerializerFactory.GetSerializer(type, ServiceProvider).ReadObject(reader, ServiceProvider);
        }
        /// <summary>
        /// Clones current instance replacing type serializer factory.
        /// </summary>
        /// <param name="typeSerializerFactory">Type serializer factory to replace.</param>
        /// <returns>Cloned instance of serializer.</returns>
        public abstract Serializer Clone(ITypeSerializerFactory typeSerializerFactory);
        /// <summary>
        /// Serializes object to stream.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="obj">Object to serialize.</param>
        public void Serialize(Stream stream, object obj)
        {
            using (var writer = CreateDataWriter(stream))
            {
                Serialize(writer, obj);
            }
            stream.Flush();
        }
        /// <summary>
        /// Deserializes object from stream.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="type">Object type.</param>
        /// <returns>Deserialized object.</returns>
        public object Deserialize(Stream stream, Type type)
        {
            using(var reader = CreateDataReader(stream))
            {
                return Deserialize(reader, type);
            }
        }
    }
}