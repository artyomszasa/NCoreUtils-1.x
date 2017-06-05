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
        /// Applies the given function to each element of the sequence and returns the list comprised of the results
        /// for each element where the function returns non-empty value.
        /// </summary>
        /// <param name="source">The input sequence.</param>
        /// <param name="chooser">A function to transform items.</param>
        /// <returns>The result sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// Thrown if <paramref name="source" /> or <paramref name="chooser" /> is <c>null</c>.
        /// </exception>
        public static IEnumerable<TResult> MaybeChoose<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Maybe<TResult>> chooser)
        {
            RuntimeAssert.ArgumentNotNull(source, nameof(source));
            RuntimeAssert.ArgumentNotNull(chooser, nameof(chooser));
            foreach (var item in source)
            {
                if (chooser(item).TryGetValue(out var result))
                {
                    yield return result;
                }
            }
        }
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