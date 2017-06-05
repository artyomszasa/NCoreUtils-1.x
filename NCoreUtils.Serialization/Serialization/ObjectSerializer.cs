using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Reflection;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Default object serializer.
    /// </summary>
    public class ObjectSerializer : TypeSerializer
    {
        /// <summary>
        /// Object factory used to instantiate objects.
        /// </summary>
        public IObjectFactory ObjectFactory { get; private set; }
        /// <summary>
        /// Members involved in serialization.
        /// </summary>
        public ImmutableDictionary<string, IAccessor> Accessors { get; private set; }
        /// <summary>
        /// Initilizes new instance of <see cref="T:NCoreUtils.Serialization.ObjectSerializer" /> with the specified
        /// parameters.
        /// </summary>
        /// <param name="factory">Type serializer factory.</param>
        /// <param name="targetType">Handled type.</param>
        /// <param name="accessors">All serializable accessors.</param>
        /// <param name="objectFactory">Object factory to use.</param>
        public ObjectSerializer(TypeSerializerFactory factory, Type targetType, ImmutableArray<IAccessor> accessors, IObjectFactory objectFactory)
            : base(factory, targetType)
        {
            //FIXME: Handle members hiding inherited members...
            Accessors = ImmutableDictionary.CreateRange(accessors.Select(a => new KeyValuePair<string, IAccessor>(a.Name, a)));
            ObjectFactory = objectFactory;
        }

        /// <summary>
        /// Controls value retreival from the object during serialization. Can be overridden to adjust value retreival.
        /// <para>Default implementation ignores <paramref name="serviceProvider" /></para>
        /// </summary>
        /// <param name="accessor">Accessor to use</param>
        /// <param name="instance">Instance of the object</param>
        /// <param name="serviceProvider">User defined state</param>
        protected virtual object GetAccessorValue(IAccessor accessor, object instance, IServiceProvider serviceProvider)
        {
            return accessor.GetValue(instance);
        }

        /// <summary>
        /// Controls value application to the object during deserialization. Can be overridden to adjust value application.
        /// <para>Default implementation ignores <paramref name="serviceProvider" /></para>
        /// </summary>
        /// <param name="accessor">Accessor to use</param>
        /// <param name="instance">Instance of the object</param>
        /// <param name="value">Value to apply</param>
        /// <param name="serviceProvider">User defined state</param>
        protected virtual void SetAccessorValue(IAccessor accessor, object instance, object value, IServiceProvider serviceProvider)
        {
            accessor.SetValue(instance, value);
        }

        /// <summary>
        /// Serializes object whose dynamic type matches the target type of the current object serializer instance.
        /// </summary>
        protected virtual void WriteExactObject(object obj, IDataWriter writer, IServiceProvider serviceProvider)
        {
            writer.WriteObjectStart(Factory.GetTypeName(TargetType));
            foreach (var accessor in Accessors.Values)
            {
                var value = GetAccessorValue(accessor, obj, serviceProvider);
                writer.WriteMemberName(accessor.Name);
                Factory.GetSerializer(accessor.TargetType, serviceProvider).WriteObject(value, writer, serviceProvider);
            }
            writer.WriteObjectEnd();
        }
        /// <summary>
        /// Creates object when no object factory has been specified.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>Created object.</returns>
        protected virtual object CreateInstance(IServiceProvider serviceProvider)
            => ActivatorUtilities.CreateInstance(serviceProvider, TargetType);
        /// <summary>
        /// Deserializes object whose dynamic type matches the target type of the current object serializer instance.
        /// </summary>
        protected virtual object ReadExactObject(IObjectDataReader reader, IServiceProvider serviceProvider)
        {
            var inputBuilder = ImmutableDictionary.CreateBuilder<IAccessor, object>();
            while (reader.MoveNext())
            {
                var memberName = reader.CurrentMemberName;
                IAccessor accessor;
                if (Accessors.TryGetValue(memberName, out accessor))
                {
                    var value = Factory.GetSerializer(accessor.TargetType, serviceProvider).ReadObject(reader, serviceProvider);
                    inputBuilder.Add(accessor, value);
                }
            }
            var input = inputBuilder.ToImmutable();
            if (null != ObjectFactory)
            {
                return ObjectFactory.CreateObject(serviceProvider, input);
            }
            var obj = CreateInstance(serviceProvider);
            foreach (var accessor in Accessors.Values)
            {
                object value;
                if (input.TryGetValue(accessor, out value) || accessor.TryGetDefaultValue(out value))
                {
                    SetAccessorValue(accessor, obj, value, serviceProvider);
                }
                else
                {
                    throw new MissingValueException(accessor, $"Missing value for {accessor} while deserializing {TargetType.FullName}");
                }
            }
            return obj;
        }
        /// <inheritdoc />
        public override void WriteObject(object obj, IDataWriter writer, IServiceProvider serviceProvider)
        {
            if (null == obj)
            {
                writer.WriteNull();
            }
            else
            {
                var dynamicType = obj.GetType();
                if (dynamicType == TargetType)
                {
                    WriteExactObject(obj, writer, serviceProvider);
                }
                else
                {
                    var serializer = Factory.GetSerializer(dynamicType, serviceProvider);
                    var objectSerializer = serializer as ObjectSerializer;
                    if (null == objectSerializer)
                    {
                        serializer.WriteObject(obj, writer, serviceProvider);
                    }
                    else
                    {
                        objectSerializer.WriteExactObject(obj, writer, serviceProvider);
                    }
                }
            }
        }
        /// <inheritdoc />
        public override object ReadObject(IDataReader reader, IServiceProvider serviceProvider)
        {
            var objectReader = reader.ReadObject();
            if (objectReader.IsNull)
            {
                return null;
            }
            var dynamicType = Factory.ResolveType(objectReader.TypeName);
            if (TargetType == dynamicType)
            {
                return ReadExactObject(objectReader, serviceProvider);
            }
            var serializer = Factory.GetSerializer(dynamicType, serviceProvider);
            var objectSerializer = serializer as ObjectSerializer;
            if (null == objectSerializer)
            {
                throw new SerializationException($"{serializer.GetType().FullName} must derive from {typeof(ObjectSerializer).FullName}");
            }
            return objectSerializer.ReadExactObject(objectReader, serviceProvider);
        }
    }
}