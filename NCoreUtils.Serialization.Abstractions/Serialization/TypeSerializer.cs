using System;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Base type that contains overridable methods to perform type serialization.
    /// </summary>
    public abstract class TypeSerializer
    {
        sealed class TypedSerializer<T> : TypeSerializer<T>
        {
            readonly TypeSerializer _nonTypedSerializer;
            public TypedSerializer(TypeSerializer nonTypedSerializer)
                : base(nonTypedSerializer.Factory)
            {
                RuntimeAssert.ArgumentNotNull(nonTypedSerializer, nameof(nonTypedSerializer));
                _nonTypedSerializer = nonTypedSerializer;
            }
            public override void Write(T obj, IDataWriter writer, IServiceProvider serviceProvider) => _nonTypedSerializer.WriteObject(obj, writer, serviceProvider);
            public override T Read(IDataReader reader, IServiceProvider serviceProvider) => (T)_nonTypedSerializer.ReadObject(reader, serviceProvider);
        }
        /// <summary>
        /// Gets type serializer factory this instance was created by.
        /// </summary>
        public ITypeSerializerFactory Factory { get; private set; }
        /// <summary>
        /// Gets the type handled by the actual type serializer.
        /// </summary>
        public Type TargetType { get; private set; }
        /// <summary>
        /// Initializes new instance of type serializer.
        /// </summary>
        /// <param name="factory">Type serializer factory this instance was created by</param>
        /// <param name="targetType">Target type handled by the actual instance.</param>
        protected TypeSerializer(ITypeSerializerFactory factory, Type targetType)
        {
            RuntimeAssert.ArgumentNotNull(factory, nameof(factory));
            RuntimeAssert.ArgumentNotNull(targetType, nameof(targetType));
            Factory = factory;
            TargetType = targetType;
        }
        /// <summary>
        /// When overridden writes object to generic data writer.
        /// </summary>
        /// <param name="object">Object to serialize.</param>
        /// <param name="writer">Low level data writer to use.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        public abstract void WriteObject(object @object, IDataWriter writer, IServiceProvider serviceProvider);
        /// <summary>
        /// When overridden reads object from generic data reader.
        /// </summary>
        /// <param name="reader">Low level data reader to use.</param>
        /// <param name="serviceProvider">ServiceProvider to use.</param>
        /// <returns>Deserialized object.</returns>
        public abstract object ReadObject(IDataReader reader, IServiceProvider serviceProvider);
        /// <summary>
        /// Returns typed instance of the type serializer.
        /// </summary>
        /// <returns>Typed instance of the type serializer</returns>
        public virtual TypeSerializer<T> AsTyped<T>() => new TypedSerializer<T>(this);
    }

    /// <summary>
    /// Base type that contains overridable methods to perform type serialization for the type specified by generic parameter.
    /// </summary>
    /// <typeparam name="T">Type handled by the type serializer.</typeparam>
    public abstract class TypeSerializer<T> : TypeSerializer
    {
        /// <summary>
        /// Initializes new instance of type serializer.
        /// </summary>
        /// <param name="factory">Type serializer factory this instance was created by</param>
        protected TypeSerializer(ITypeSerializerFactory factory) : base(factory, typeof(T)) { }
        /// <summary>
        /// When overridden writes object to generic data writer.
        /// </summary>
        /// <param name="object">Object to serialize.</param>
        /// <param name="writer">Low level data writer to use.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        public abstract void Write(T @object, IDataWriter writer, IServiceProvider serviceProvider);
        /// <summary>
        /// When overridden reads object from generic data reader.
        /// </summary>
        /// <param name="reader">Low level data reader to use.</param>
        /// <param name="serviceProvider">ServiceProvider to use.</param>
        /// <returns>Deserialized object.</returns>
        public abstract T Read(IDataReader reader, IServiceProvider serviceProvider);
        /// <inheritdoc />
        public override void WriteObject(object obj, IDataWriter writer, IServiceProvider serviceProvider) => Write((T)obj, writer, serviceProvider);
        /// <inheritdoc />
        public override object ReadObject(IDataReader reader, IServiceProvider serviceProvider) => Read(reader, serviceProvider);
        /// <inheritdoc />
        public override TypeSerializer<U> AsTyped<U>()
        {
            if (typeof(U) != typeof(T))
            {
                throw new InvalidCastException();
            }
            return (TypeSerializer<U>)(object)this;
        }
    }
}