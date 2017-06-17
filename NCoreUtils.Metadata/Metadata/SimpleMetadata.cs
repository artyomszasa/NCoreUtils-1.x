using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NCoreUtils.Collections;
using NCoreUtils.Reflection;

namespace NCoreUtils.Metadata
{
    /// <summary>
    /// Base class for metadata represented by the properties of the derived object.
    /// </summary>
    public abstract class SimpleMetadata : IMetadata
    {
        ImmutableDictionary<CaseInsensitive, PropertyInfo> _metdataProperties;
        bool _isInitialized;
        int IMetadata.Count => MetadataProperties.Count;
        /// <summary>
        /// Gets metadata key/property association. The result value is generated on the first use.
        /// </summary>
        protected IReadOnlyDictionary<CaseInsensitive, PropertyInfo> MetadataProperties
        {
            get
            {
                if (!_isInitialized)
                {
                    _metdataProperties = CollectMetdataProperties().ToImmutableDictionary();
                    _isInitialized = true;
                }
                return _metdataProperties;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<KeyValuePair<CaseInsensitive, object>> GetEnumerator()
        {
            foreach (var kv in MetadataProperties)
            {
                yield return KeyValuePair.Create(kv.Key, kv.Value.GetValue(this, null));
            }
        }

        /// <summary>
        /// Invoked once to collect metadata properties for the actual instance.
        /// </summary>
        /// <returns>
        /// Metdata key/property collection that contains all metadata properties for the actual instance.
        /// </returns>
        protected virtual IEnumerable<KeyValuePair<CaseInsensitive, PropertyInfo>> CollectMetdataProperties()
            => GetType()
                .GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .MaybeChoose(property => {
                    if (property.CanRead && property.TryGetAttribute(out MetadataKeyAttribute keyAttr))
                    {
                        return KeyValuePair.Create(keyAttr.Key, property).AsMaybe();
                    }
                    return Maybe.Empty;
                });

        IEnumerator<KeyValuePair<CaseInsensitive, object>> IEnumerable<KeyValuePair<CaseInsensitive, object>>.GetEnumerator()
            => GetEnumerator();

        bool IMetadata.TryGetValue(CaseInsensitive key, out object value)
        {
            if (MetadataProperties.TryGetValue(key, out var property))
            {
                value = property.GetValue(this, null);
                return true;
            }
            value = default(object);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}