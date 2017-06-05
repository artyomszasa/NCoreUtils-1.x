using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace NCoreUtils.Serialization
{
    /// <summary>
    /// Type serializer implementation which caches created type serializers so that type serializers are created once
    /// for type.
    /// </summary>
    public class CachedTypeSerializerFactory : TypeSerializerFactory
    {
        readonly ConcurrentDictionary<Type, TypeSerializer> _cache = new ConcurrentDictionary<Type, TypeSerializer>();
        readonly Func<Type, IServiceProvider, TypeSerializer> _typeSerializerFactory;
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.CachedTypeSerializerFactory" /> with the
        /// specified accessor selector.
        /// </summary>
        /// <param name="defaultAccessorSelector">Accessor selector to use.</param>
        public CachedTypeSerializerFactory(IAccessorSelector defaultAccessorSelector)
            : base(defaultAccessorSelector)
        {
            _typeSerializerFactory = base.GetSerializer;
        }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Serialization.CachedTypeSerializerFactory" /> with
        /// default accessor selector.
        /// </summary>
        public CachedTypeSerializerFactory() : this(new DefaultAccessorSelector()) { }
        /// <summary>
        /// Overrides default type serializer population so that type serializers are created once for type.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="serviceProvider">Serivice provider.</param>
        /// <returns>Type serializer.</returns>
        public override TypeSerializer GetSerializer(Type type, IServiceProvider serviceProvider)
        {
            if (_cache.TryGetValue(type, out var typeSerializer))
            {
                return typeSerializer;
            }
            lock (_cache)
            {
                return _cache.GetOrAdd(type, ty => _typeSerializerFactory(ty, serviceProvider));
            }
        }
    }
}