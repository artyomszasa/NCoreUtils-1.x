using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Reflection;
using NCoreUtils.Serialization.Meta;
using NCoreUtils.Serialization.PrimitiveTypeSerializers;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Default type serializer factory.
    /// </summary>
    public class TypeSerializerFactory : ITypeSerializerFactory
    {
        readonly ImmutableDictionary<Type, TypeSerializer> _primitiveSerializers;
        readonly IAccessorSelector _defaultAccessorSelector;
        int _isDisposed;

        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.TypeSerializerFactory" /> with the
        /// specified accessor selector.
        /// </summary>
        /// <param name="defaultAccessorSelector">Accessor selector to be used as default.</param>
        public TypeSerializerFactory(IAccessorSelector defaultAccessorSelector)
        {
            _defaultAccessorSelector = defaultAccessorSelector;
            _primitiveSerializers = ImmutableDictionary.CreateRange(new KeyValuePair<Type, TypeSerializer>[] {
                new KeyValuePair<Type, TypeSerializer>(typeof(bool), new BooleanSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(sbyte), new SByteSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(short), new Int16Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(int), new Int32Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(long), new Int64Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(byte), new ByteSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(ushort), new UInt16Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(uint), new UInt32Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(ulong), new UInt64Serializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(float), new SingleSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(double), new DoubleSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(decimal), new DecimalSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(char), new CharSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(string), new StringSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(DateTime), new DateTimeSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(DateTimeOffset), new DateTimeOffsetSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(Guid), new GuidSerializer(this)),
                new KeyValuePair<Type, TypeSerializer>(typeof(CaseInsensitive), new CaseInsensitiveSerializer(this))
            });
        }

        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.TypeSerializerFactory" /> with
        /// default accessor selector.
        /// </summary>
        public TypeSerializerFactory() : this(new DefaultAccessorSelector()) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        void ThrowIfDisposed()
        {
            if (0 != _isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// When overridden should be used to release all used resources.
        /// </summary>
        /// <param name="disposing">Whether disposing or finalizing.</param>
        protected virtual void Dispose(bool disposing)
        {
            _isDisposed = 1;
        }

        /// <summary>
        /// Returns accessor selector for the specified type.
        /// </summary>
        /// <remarks>
        /// Accessor selector retrieved using following scheme:
        /// <list type="number">
        /// <item>
        /// <description>
        /// If <see cref="T:NCoreUtils.Serialization.Meta.AccessorSelectorAttriute" /> is defined for the type, then
        /// accessor selector factory defined by this attribute is used.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If service provider contains implementation for <see cref="T:NCoreUtils.Serialization.IAccessorSelector{T}" />
        /// specialization for <paramref name="type" />, then this implementation is used.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If service provider contains implementation for <see cref="T:NCoreUtils.Serialization.IAccessorSelectorFactory{T}" />
        /// specialization for <paramref name="type" />, then this implementation is to create accessor selector.
        /// </description>
        /// </item>
        /// <item>
        /// <description>Default accessor selector is used.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <param name="type">Target type.</param>
        /// <param name="serviceProvider">Service provider to use.</param>
        /// <returns>Accessor selector for the specified type.</returns>
        protected virtual IAccessorSelector GetAccessorSelectorFor(Type type, IServiceProvider serviceProvider)
        {
            RuntimeAssert.ArgumentNotNull(type, nameof(type));
            IAccessorSelector accessorSelector;
            if (type.TryGetAttribute<AccessorSelectorAttribute>(out var attr))
            {
                var factory = (IAccessorSelectorFactory)ActivatorUtilities.CreateInstance(serviceProvider, attr.SelectorFactoryType);
                accessorSelector = factory.Create(serviceProvider);
            }
            else if (serviceProvider.TryGetService(typeof(IAccessorSelector<>).MakeGenericType(type), out var boxedSelector))
            {
                accessorSelector = (IAccessorSelector)boxedSelector;
            }
            else if (serviceProvider.TryGetService(typeof(IAccessorSelectorFactory<>).MakeGenericType(type), out var boxedFactory))
            {
                var factory = (IAccessorSelectorFactory)boxedFactory;
                accessorSelector = factory.Create(serviceProvider);
            }
            else
            {
                accessorSelector = _defaultAccessorSelector;
            }
            if (accessorSelector is DerivedAccessorSelector derived)
            {
                derived.BaseAccessorSelector = _defaultAccessorSelector;
                return derived;
            }
            return accessorSelector;
        }

        /// <summary>
        /// Gets member inheritance for type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Member inheritance to be used with type.</returns>
        protected virtual MemberInheritance GetMemberInheritanceFor(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                // type is struct type
                return MemberInheritance.None;
            }
            MemberInheritanceAttribute attr;
            if (type.TryGetAttribute(out attr))
            {
                return attr.MemberInheritance;
            }
            return MemberInheritance.Default;
        }

        /// <summary>
        /// Gets default member inheritance for type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Default member inheritance to be used with type.</returns>
        protected virtual MemberInheritance GetDefaultMemberInheritanceFor(Type type) => MemberInheritance.Inherited;
        /// <summary>
        /// Returns object factory for the specified type. Default object factory may be overriden by attribute, or through singleton service.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>Object factory for the specified type.</returns>
        protected virtual IObjectFactory GetObjectFactoryFor(Type type, IServiceProvider serviceProvider)
        {
            if (type.TryGetAttribute(out ObjectFactoryAttribute attr))
            {
                return (IObjectFactory)ActivatorUtilities.CreateInstance(serviceProvider, attr.ObjectFactoryType);
            }
            if (serviceProvider.TryGetService(typeof(IObjectFactory<>).MakeGenericType(type), out var boxedFactory))
            {
                return (IObjectFactory)boxedFactory;
            }
            return null;
        }
        /// <summary>
        /// Only called for objects and structs;
        /// </summary>
        protected virtual ImmutableArray<IAccessor> GetSerializableAccessorsFor(Type type, IServiceProvider serviceProvider)
        {
            var accessorSelector = GetAccessorSelectorFor(type, serviceProvider);
            var memberInheritance = GetMemberInheritanceFor(type);
            if (memberInheritance == MemberInheritance.Default)
            {
                memberInheritance = GetDefaultMemberInheritanceFor(type);
            }
            if (memberInheritance == MemberInheritance.None)
            {
                return ImmutableArray.CreateRange(accessorSelector.GetSerializableAccessors(type));
            }
            // memberInheritance == MemberInheritance.Inherited
            var baseType = type.GetTypeInfo().BaseType;
            if (null == baseType || typeof(object) == baseType)
            {
                return ImmutableArray.CreateRange(accessorSelector.GetSerializableAccessors(type));
            }
            var baseSerializer = GetSerializer(baseType, serviceProvider) as ObjectSerializer;
            if (null == baseSerializer)
            {
                throw new InvalidOperationException($"{baseType.FullName} is not an object");
            }
            return ImmutableArray.CreateRange(accessorSelector.GetSerializableAccessors(type).Union(baseSerializer.Accessors.Values));
        }
        /// <summary>
        /// Creates default object serializer for the specified type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="accessors">All of the serializable accessors.</param>
        /// <param name="objectFactory">Optional object factory.</param>
        /// <returns>Object serializer for the specified type.</returns>
        protected virtual ObjectSerializer CreateObjectSerializer(Type type, ImmutableArray<IAccessor> accessors, IObjectFactory objectFactory)
        {
            return new ObjectSerializer(this, type, accessors, objectFactory);
        }
        /// <summary>
        /// Creates type serializer for the specified type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>Type serializer for the specified type.</returns>
        protected virtual TypeSerializer CreateTypeSerializerFor(Type type, IServiceProvider serviceProvider)
        {
            // var accessorSelector = GetAccessorSelector(type);
            TypeSerializer typeSerializer;
            if (_primitiveSerializers.TryGetValue(type, out typeSerializer))
            {
                return typeSerializer;
            }
            if (type.IsArray)
            {
                return ArraySerializerFactory.Create(this, type.GetElementType(), serviceProvider);
            }
            if (type.TryGetAttribute(out TypeSerializerAttribute attr))
            {
                return (TypeSerializer)ActivatorUtilities.CreateInstance(serviceProvider, attr.TypeSerializerType);
            }
            return CreateObjectSerializer(type, GetSerializableAccessorsFor(type, serviceProvider), GetObjectFactoryFor(type, serviceProvider));
        }
        /// <summary>
        /// Returns type serializer for the specified type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>Type serializer for the specified type.</returns>
        public virtual TypeSerializer GetSerializer(Type type, IServiceProvider serviceProvider)
        {
            if (serviceProvider.TryGetService(typeof(TypeSerializer<>).MakeGenericType(type), out var boxedTypeSerializer))
            {
                return (TypeSerializer)boxedTypeSerializer;
            }
            return CreateTypeSerializerFor(type, serviceProvider);
        }
        /// <summary>
        /// Returns assembly qualified type name of the specified type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>Assembly qualified name.</returns>
        public virtual string GetTypeName(Type type) => type.AssemblyQualifiedName;
        /// <summary>
        /// Resolves type by its assembly qualified name.
        /// </summary>
        /// <param name="typeName">Assembly qualified type name.</param>
        /// <returns>Resolved type.</returns>
        public virtual Type ResolveType(string typeName) => Type.GetType(typeName);
        /// <summary>
        /// Returns typed type serializer for the specified type.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>Typed type serializer for thze specified type.</returns>
        public virtual TypeSerializer<T> GetSerializer<T>(IServiceProvider serviceProvider)
        {
            ThrowIfDisposed();
            return GetSerializer(typeof(T), serviceProvider).AsTyped<T>();
        }
        /// <summary>
        /// Releases all resources used by the current instance.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
    }
}