using System;
using System.Collections.Generic;
using System.Linq;

namespace NCoreUtils
{
    /// <summary>
    /// Maybe related sequence extensions.
    /// </summary>
    public static class MaybeEnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence if any or empty container.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// Either empty <see cref="T:NCoreUtils.Maybe{TResult}" /> or
        /// <see cref="T:NCoreUtils.Maybe{TResult}" /> containing the first element.
        /// </returns>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source)
        {
            RuntimeAssert.ArgumentNotNull(source, nameof(source));
            return source.Select(Maybe.AsMaybe).FirstOrDefault();
        }
        /// <summary>
        /// Returns the first element of a sequence that setisfies specified predicate if any or empty container.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <param name="predicate">Predicate to apply.</param>
        /// <returns>
        /// Either empty <see cref="T:NCoreUtils.Maybe{TResult}" /> or
        /// <see cref="T:NCoreUtils.Maybe{TResult}" /> containing the first element.
        /// </returns>
        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            RuntimeAssert.ArgumentNotNull(source, nameof(source));
            return source.Where(predicate).Select(Maybe.AsMaybe).FirstOrDefault();
        }
    }
}