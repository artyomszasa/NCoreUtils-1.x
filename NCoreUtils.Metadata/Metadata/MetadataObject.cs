using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using NCoreUtils.Collections;
using NCoreUtils.Reflection;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Represents simple composite metadata which is initialized on construction.
    /// </summary>
    public class MetadataObject : ICompositeMetadata
    {
        static Lazy<T> L<T>(Func<T> valueFactory) => new Lazy<T>(valueFactory);
        readonly ImmutableDictionary<Type, IMetadata> _explicitMetadata;
        readonly Lazy<ImmutableDictionary<CaseInsensitive, object>> _rawMetadata;
        /// <summary>
        /// Gets lazily initialized raw metdata representation.
        /// </summary>
        protected ImmutableDictionary<CaseInsensitive, object> RawMetadata => _rawMetadata.Value;
        /// <summary>
        /// Gets lazily initialized raw metdata representation item count.
        /// </summary>
        public int Count => RawMetadata.Count;
        /// <summary>
        /// Initializes new composite metadata object from initialized metadata objects appending any property marked
        /// as metadata to the raw representation.
        /// </summary>
        /// <param name="explicitMetadata"></param>
        protected internal MetadataObject(ImmutableDictionary<Type, IMetadata> explicitMetadata)
        {
            RuntimeAssert.ArgumentNotNull(explicitMetadata, nameof(explicitMetadata));
            _explicitMetadata = explicitMetadata;
            _rawMetadata = L(() => {
                var builder = ImmutableDictionary.CreateBuilder<CaseInsensitive, object>();
                // compose explicit metadata
                _explicitMetadata.Values
                    .ForEach(data => data.ForEach(raw => builder[raw.Key] = raw.Value));
                // compose implicit metadata
                this.GetType()
                    .GetAccessors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .ForEach(accessor => {
                        if (accessor.TryGetAttribute<MetadataKeyAttribute>(out var keyAttr))
                        {
                            builder[keyAttr.Key] = accessor.GetValue(this);
                        }
                    });
                return builder.ToImmutable();
            });
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        /// <inheritdoc />
        public bool TryGetMetadata(Type metadataType, out IMetadata metadata)
            => _explicitMetadata.TryGetValue(metadataType, out metadata);
        /// <inheritdoc />
        public bool TryGetValue(CaseInsensitive key, out object value)
            => RawMetadata.TryGetValue(key, out value);
        /// <inheritdoc />
        public IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
            => RawMetadata.GetEnumerator();
    }
}