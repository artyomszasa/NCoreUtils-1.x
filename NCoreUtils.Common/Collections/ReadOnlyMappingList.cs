using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NCoreUtils.Collections
{
    /// <summary>
    /// Implements a read only list that maps element on retrieval. Mapping is applied each time element is being
    /// retrieved.
    /// </summary>
    public class ReadOnlyMappingList<TSource, TTarget> : IReadOnlyList<TTarget>
    {
        readonly IReadOnlyList<TSource> _source;
        readonly Func<TSource, TTarget> _mapping;
        /// <summary>
        /// Initiliazes new instance of <see cref="T:NCoreUtils.Collections.ReadOnlyMappingList{TSource,TTarget}" />
        /// from specified source and mapping.
        /// </summary>
        /// <param name="source">Source read only list.</param>
        /// <param name="mapping">Mapping to apply when retrieving elements.</param>
        public ReadOnlyMappingList(IReadOnlyList<TSource> source, Func<TSource, TTarget> mapping)
        {
            RuntimeAssert.ArgumentNotNull(source, nameof(source));
            RuntimeAssert.ArgumentNotNull(mapping, nameof(mapping));
            _source = source;
            _mapping = mapping;
        }

        /// <summary>
        /// Gets mapped element at the specified index. Mapping is applied each time element is being retrieved.
        /// </summary>
        public TTarget this[int index] => _mapping(_source[index]);

        /// <summary>
        /// Gets elements count of the read only list.
        /// </summary>
        public int Count => _source.Count;

        /// <summary>
        /// Returns enumerator which enumerates through the source collection and maps elements using the specified
        /// mapping.
        /// </summary>
        public IEnumerator<TTarget> GetEnumerator() => _source.Select(_mapping).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}