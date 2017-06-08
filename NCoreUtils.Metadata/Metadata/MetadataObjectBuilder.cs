using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NCoreUtils.Collections;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Provides default implementation for typed composite metadata builder.
    /// </summary>
    public class MetadataObjectBuilder<T> : ICompositeMetdataBuilder<T> where T : MetadataObject
    {
        sealed class ObjectBeenBuilt : ICompositeMetadata
        {
            public ImmutableDictionary<Type, IMetadata>.Builder Builder { get; } = ImmutableDictionary.CreateBuilder<Type, IMetadata>();
            public int Count => Builder.Count;
            public IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
                => Builder.SelectMany(kv => kv.Value).GetEnumerator();
            public bool TryGetMetadata(Type metadataType, out IMetadata metadata)
                => Builder.TryGetValue(metadataType, out metadata);
            public bool TryGetValue(CaseInsensitive key, out object value)
            {
                foreach (var kv in Builder)
                {
                    var item = kv.Value;
                    if (item.TryGetValue(key, out var v))
                    {
                        value = v;
                        return true;
                    }
                }
                value = default(object);
                return false;
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        readonly Dictionary<Type, MetadataDescriptor> _metadata = new Dictionary<Type, MetadataDescriptor>();

        MetadataDescriptor IDictionary<Type, MetadataDescriptor>.this[Type key]
        {
            get => _metadata[key];
            set => _metadata[key] = value;
        }
        ICollection<Type> IDictionary<Type, MetadataDescriptor>.Keys => _metadata.Keys;
        ICollection<MetadataDescriptor> IDictionary<Type, MetadataDescriptor>.Values => _metadata.Values;
        bool ICollection<KeyValuePair<Type, MetadataDescriptor>>.IsReadOnly => false;
        /// <summary>
        /// Gets count of registered metadata items.
        /// </summary>
        public int Count => _metadata.Count;
        /// <summary>
        /// Initlizes new instance of <see cref="T:NCoreUtils.Metadata.MetadataObjectBuilder{T}" />.
        /// </summary>
        public MetadataObjectBuilder() { }
        void IDictionary<Type, MetadataDescriptor>.Add(Type key, MetadataDescriptor value)
            => _metadata.Add(key, value);
        void ICollection<KeyValuePair<Type, MetadataDescriptor>>.Add(KeyValuePair<Type, MetadataDescriptor> item)
            => _metadata.Add(item.Key, item.Value);
        void ICollection<KeyValuePair<Type, MetadataDescriptor>>.Clear()
            => _metadata.Clear();
        bool ICollection<KeyValuePair<Type, MetadataDescriptor>>.Contains(KeyValuePair<Type, MetadataDescriptor> item)
            => ((ICollection<KeyValuePair<Type, MetadataDescriptor>>)_metadata).Contains(item);
        bool IDictionary<Type, MetadataDescriptor>.ContainsKey(Type key)
            => _metadata.ContainsKey(key);
        void ICollection<KeyValuePair<Type, MetadataDescriptor>>.CopyTo(KeyValuePair<Type, MetadataDescriptor>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<Type, MetadataDescriptor>>)_metadata).CopyTo(array, arrayIndex);
        IEnumerator<KeyValuePair<Type, MetadataDescriptor>> IEnumerable<KeyValuePair<Type, MetadataDescriptor>>.GetEnumerator()
            => _metadata.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => _metadata.GetEnumerator();
        bool IDictionary<Type, MetadataDescriptor>.Remove(Type key)
            => _metadata.Remove(key);
        bool ICollection<KeyValuePair<Type, MetadataDescriptor>>.Remove(KeyValuePair<Type, MetadataDescriptor> item)
            => ((ICollection<KeyValuePair<Type, MetadataDescriptor>>)_metadata).Remove(item);
        bool IDictionary<Type, MetadataDescriptor>.TryGetValue(Type key, out MetadataDescriptor value)
            => _metadata.TryGetValue(key, out value);
        ICompositeMetadata ICompositeMetdataBuilder.Initialize(IServiceProvider serviceProvider) => Initialize(serviceProvider);
        /// <inheritdoc />
        public bool ContainsMetadata<TMetdata>() where TMetdata : IMetadata
            => _metadata.ContainsKey(typeof(TMetdata));
        /// <summary>
        /// Allows overriding object activation. By default object activated using service provider and explicit metadata as argument.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="explicitMetadata">Explicit metadata assigned to the builder.</param>
        /// <returns>Target object instance.</returns>
        protected virtual T Activate(IServiceProvider serviceProvider, ImmutableDictionary<Type, IMetadata> explicitMetadata)
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, explicitMetadata);
        }
        /// <inheritdoc />
        public T Initialize(IServiceProvider serviceProvider)
        {
            RuntimeAssert.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            var builder = new ObjectBeenBuilt();
            _metadata.MaybeChoose(kv => BoxedMatcher.TryGetValue(kv.Value).Select(md => KeyValuePair.Create(kv.Key, md)))
                .ForEach(kv => builder.Builder.Add(kv.Key, kv.Value));
            _metadata.MaybeChoose(kv => BoxedMatcher.TryInitialize(kv.Value, serviceProvider, builder).Select(md => KeyValuePair.Create(kv.Key, md)))
                .ForEach(kv => builder.Builder.Add(kv.Key, kv.Value));
            return Activate(serviceProvider, builder.Builder.ToImmutable());
        }
    }
}