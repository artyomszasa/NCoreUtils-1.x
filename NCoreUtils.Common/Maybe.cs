using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NCoreUtils
{
    /// <summary>
    /// Contains enumerable like extensions for <see cref="T:NCoreUtils.Maybe{T}" />.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Represents objects thatare implicitly convertible to <see cref="T:NCoreUtils.Maybe{T}" /> of any type.
        /// </summary>
        public sealed class NoValueClass
        {
            NoValueClass()
            {
                throw new InvalidOperationException("NoValueClass is not intended to be instantiable.");
            }
        }
        /// <summary>
        /// Represents no value. Implicitly convertible to <see cref="T:NCoreUtils.Maybe{T}" /> of any type.
        /// </summary>
        public static NoValueClass Empty { get; } = null;
        /// <summary>
        /// Determines whether maybe element satisfies a condition.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if no element is present.
        /// </remarks>
        /// <param name="maybe">An <see cref="T:NCoreUtils.Maybe{T}" /> that may contains the element to apply the predicate to.</param>
        /// <param name="predictate">A function to test element for a condition.</param>
        /// <returns>
        /// <c>true</c> if every element is present and passes the test in the specified predicate, or if no elements
        /// are present; otherwise, <c>false</c>.
        /// </returns>
        public static bool All<T>(this Maybe<T> maybe, Func<T, bool> predictate)
        {
            if (maybe.HasValue)
            {
                return predictate(maybe.Value);
            }
            return true;
        }
        /// <summary>
        /// Determines whether a <see cref="T:NCoreUtils.Maybe{T}" /> contains an element. Same as
        /// <see cref="P:NCoreUtils.Maybe{T}.HasValue" />.
        /// </summary>
        /// <param name="maybe">The <see cref="T:NCoreUtils.Maybe{T}" /> to check for emptiness.</param>
        /// <returns>
        /// <c>true</c> if source instance of <see cref="T:NCoreUtils.Maybe{T}" /> contains element, otherwise
        /// <c>false</c>.
        /// </returns>
        public static bool Any<T>(this Maybe<T> maybe) => maybe.HasValue;
        /// <summary>
        /// Determines whether a <see cref="T:NCoreUtils.Maybe{T}" /> contains an element that safisfies the specified
        /// predicate.
        /// </summary>
        /// <param name="maybe">
        /// The <see cref="T:NCoreUtils.Maybe{T}" /> whose elements to apply the predicate to.
        /// </param>
        /// <param name="predictate">A function to test an element for a condition.</param>
        /// <returns>
        /// <c>true</c> if source instance of <see cref="T:NCoreUtils.Maybe{T}" /> contains element that safisfies the
        /// specified predicate, otherwise <c>false</c>.
        /// </returns>
        public static bool Any<T>(this Maybe<T> maybe, Func<T, bool> predictate)
            => maybe.HasValue && predictate(maybe.Value);
        /// <summary>
        /// Returns the input as <see cref="T:System.Collections.Enumerable{T}" />.
        /// </summary>
        /// <param name="maybe">
        /// The <see cref="T:NCoreUtils.Maybe{T}" /> to convert to <see cref="T:System.Collections.Enumerable{T}" />.
        /// </param>
        /// <returns>
        /// Either single valued or empty sequence.
        /// </returns>
        public static IEnumerable<T> AsEnumerable<T>(this Maybe<T> maybe)
            => maybe.HasValue ? new [] { maybe.Value } : Enumerable.Empty<T>();
        /// <summary>
        /// Explicitly converts arbitrary value to <see cref="T:NCoreUtils.Maybe{T}" />.
        /// </summary>
        /// <param name="value">Value to store.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> containing the source value.
        /// </returns>
        public static Maybe<T> AsMaybe<T>(this T value) => new Maybe<T>(value);
        /// <summary>
        /// Determines whether a container contains a specified element by using the default equality comparer.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="value">The value to locate in the container.</param>
        /// <returns>
        /// <c>true</c> if the source container contains an element that has the specified value; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool Contains<T>(this Maybe<T> maybe, T value)
            => maybe.Contains(value, Maybe<T>._comparer);
        /// <summary>
        /// Determines whether a container contains a specified element by using a specified
        /// <see cref="T:System.Collections.Generic.IEqualityComparer{T}" />.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="value">The value to locate in the container.</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>
        /// <c>true</c> if the source container contains an element that has the specified value; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool Contains<T>(this Maybe<T> maybe, T value, IEqualityComparer<T> comparer)
            => maybe.HasValue && comparer.Equals(maybe.Value, value);
        /// <summary>
        /// Returns the number of elements in a container.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <returns><c>0</c> if container is empty, <c>1</c> otherwise.</returns>
        public static int Count<T>(this Maybe<T> maybe)
            => maybe.HasValue ? 1 : 0;
        /// <summary>
        /// Returns the number of elements in a container that satisfies the perdicate.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="predictate">A function to test element for a condition.</param>
        /// <returns>
        /// <c>1</c> if container is not empty and element stored satisfies the predicate, <c>0</c> otherwise.
        /// </returns>
        public static int Count<T>(this Maybe<T> maybe, Func<T, bool> predictate)
            => maybe.HasValue && predictate(maybe.Value) ? 1 : 0;
        /// <summary>
        /// Retrieves the stored element.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <returns>The stored element.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if container is empty.
        /// </exception>
        public static T Get<T>(this Maybe<T> maybe)
            => maybe.HasValue ? maybe.Value : throw new InvalidOperationException("No value.");
        /// <summary>
        /// Retrieves the stored element or the default value.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="defaultValue">Default value to use.</param>
        /// <returns>Either the element stored or the default value.</returns>
        public static T GetOrDefault<T>(this Maybe<T> maybe, T defaultValue = default(T))
            => maybe.HasValue ? maybe.Value : defaultValue;
        /// <summary>
        /// Returns the number of elements in a container.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <returns><c>0</c> if container is empty, <c>1</c> otherwise.</returns>
        public static long LongCount<T>(this Maybe<T> maybe)
            => maybe.HasValue ? 1 : 0;
        /// <summary>
        /// Returns the number of elements in a container that satisfies the perdicate.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="predictate">A function to test element for a condition.</param>
        /// <returns>
        /// <c>1</c> if container is not empty and element stored satisfies the predicate, <c>0</c> otherwise.
        /// </returns>
        public static long LongCount<T>(this Maybe<T> maybe, Func<T, bool> predictate)
            => maybe.HasValue && predictate(maybe.Value) ? 1 : 0;
        /// <summary>
        /// Projects any stored elements of a sequence into a new form.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="selector">A transform function to apply to the element.</param>
        /// <returns>
        /// Empty <see cref="T:NCoreUtils.Maybe{TResult}" /> if the source is empty, a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> whose element is the result of invoking the transform function on the
        /// source element otherwise.
        /// </returns>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, TResult> selector)
        {
            if (maybe.HasValue)
            {
                return selector(maybe.Value);
            }
            return Empty;
        }
        /// <summary>
        /// Projects any stored elements of a sequence into a new form by incorporating the element's index.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="selector">
        /// A transform function to apply to the element; the second parameter of the function is necessarily <c>0</c>.
        /// </param>
        /// <returns>
        /// Empty <see cref="T:NCoreUtils.Maybe{TResult}" /> if the source is empty, a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> whose element is the result of invoking the transform function on the
        /// source element otherwise.
        /// </returns>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, int, TResult> selector)
        {
            if (maybe.HasValue)
            {
                return selector(maybe.Value, 0);
            }
            return Empty;
        }
        /// <summary>
        /// Projects any stored elements of the container to an
        /// <see cref="T:System.Collections.Generic.IEnumerable{T}" />, and invokes a result selector function on each
        /// element therein.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="collectionSelector">
        /// A transform function to apply to any elements of the input container.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable{T}" /> whose elements are the result of invoking the
        /// one-to-many transform function collectionSelector on element of source and then mapping each of those
        /// sequence elements and their corresponding source element to a result element.
        /// </returns>
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this Maybe<TSource> maybe, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TCollection, TResult> resultSelector)
            => maybe.SelectMany(collectionSelector).Select(resultSelector);
        /// <summary>
        /// Projects any stored elements of the container to an
        /// <see cref="T:System.Collections.Generic.IEnumerable{T}" />, and invokes a result selector function on each
        /// element therein.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="collectionSelector">
        /// A transform function to apply to any elements of the input container; the second parameter of the function
        /// is necessarily <c>0</c>.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable{T}" /> whose elements are the result of invoking the
        /// one-to-many transform function collectionSelector on element of source and then mapping each of those
        /// sequence elements and their corresponding source element to a result element.
        /// </returns>
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this Maybe<TSource> maybe, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TCollection, TResult> resultSelector)
            => maybe.SelectMany(collectionSelector).Select(resultSelector);
        /// <summary>
        /// Projects any elements of the container to an <see cref="T:System.Collections.Generic.IEnumerable{T}" />.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="selector">A transform function to apply to an element.</param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable{T}" /> whose elements are the result of invoking the
        /// one-to-many transform function.
        /// </returns>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, IEnumerable<TResult>> selector)
            => maybe.HasValue ? selector(maybe.Value) : Enumerable.Empty<TResult>();
        /// <summary>
        /// Projects any elements of the container to an <see cref="T:System.Collections.Generic.IEnumerable{T}" />.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="selector">
        /// A transform function to apply to an element; the second parameter of the function is necessarily <c>0</c>.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable{T}" /> whose elements are the result of invoking the
        /// one-to-many transform function.
        /// </returns>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> maybe, Func<TSource, int, IEnumerable<TResult>> selector)
            => maybe.HasValue ? selector(maybe.Value, 0) : Enumerable.Empty<TResult>();
        /// <summary>
        /// Creates an array from a <see cref="T:NCoreUtils.Maybe{T}" />.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <returns>
        /// Either single valued or empty array.
        /// </returns>
        public static T[] ToArray<T>(this Maybe<T> maybe)
            => maybe.HasValue ? new T[] { } : new [] { maybe.Value };
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> from a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> according to a specified key selector function.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> that contains keys and values.
        /// </returns>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this Maybe<TSource> maybe, Func<TSource, TKey> keySelector)
        {
            var dictionary = new Dictionary<TKey, TSource>();
            if (maybe.TryGetValue(out var value))
            {
                dictionary.Add(keySelector(value), value);
            }
            return dictionary;
        }
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> from a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> according to a specified key selector function and key comparer.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="comparer">
        /// An <see cref="T:System.Collections.Generic.IEqualityComparer{T}" /> to compare keys.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> that contains keys and values.
        /// </returns>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this Maybe<TSource> maybe, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var dictionary = new Dictionary<TKey, TSource>(comparer);
            if (maybe.TryGetValue(out var value))
            {
                dictionary.Add(keySelector(value), value);
            }
            return dictionary;
        }
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> from a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> according to a specified key selector and element selector functions.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="valueSelector">
        /// A transform function to produce a result element value from each element.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> that contains keys and values.
        /// </returns>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this Maybe<TSource> maybe, Func<TSource, TKey> keySelector, Func<TSource, TElement> valueSelector)
        {
            var dictionary = new Dictionary<TKey, TElement>();
            if (maybe.TryGetValue(out var value))
            {
                dictionary.Add(keySelector(value), valueSelector(value));
            }
            return dictionary;
        }
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> from a
        /// <see cref="T:NCoreUtils.Maybe{T}" /> according to a specified key selector function, element selector
        /// function, and key comparer.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="valueSelector">
        /// A transform function to produce a result element value from each element.
        /// </param>
        /// <param name="comparer">
        /// An <see cref="T:System.Collections.Generic.IEqualityComparer{T}" /> to compare keys.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.Dictionary{TKey,TValue}" /> that contains keys and values.
        /// </returns>
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this Maybe<TSource> maybe, Func<TSource, TKey> keySelector, Func<TSource, TElement> valueSelector, IEqualityComparer<TKey> comparer)
        {
            var dictionary = new Dictionary<TKey, TElement>(comparer);
            if (maybe.TryGetValue(out var value))
            {
                dictionary.Add(keySelector(value), valueSelector(value));
            }
            return dictionary;
        }
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.List{T}" /> from a <see cref="T:NCoreUtils.Maybe{T}" />.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <returns>
        /// Either single valued or empty list.
        /// </returns>
        public static List<T> ToList<T>(this Maybe<T> maybe)
        {
            var list = new List<T>();
            if (maybe.TryGetValue(out var value))
            {
                list.Add(value);
            }
            return list;
        }
        /// <summary>
        /// Filters a value container based on a predicate.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="predictate">A function to test each source element for a condition.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which either empty or contains element that satisfies the predicate.
        /// </returns>
        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predictate)
        {
            if (maybe.HasValue && predictate(maybe.Value))
            {
                return maybe;
            }
            return Empty;
        }
        /// <summary>
        /// Filters a value container based on a predicate.
        /// </summary>
        /// <param name="maybe">Source container.</param>
        /// <param name="predictate">A function to test each source element for a condition.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which either empty or contains element that satisfies the predicate.
        /// </returns>
        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, int, bool> predictate)
        {
            if (maybe.HasValue && predictate(maybe.Value, 0))
            {
                return maybe;
            }
            return Empty;
        }
        /// <summary>
        /// Applies a specified function to the corresponding elements of two containers, producing a container of the result.
        /// </summary>
        /// <param name="first">The first container to merge.</param>
        /// <param name="second">The second container to merge.</param>
        /// <param name="resultSelector">A function that specifies how to merge the elements.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which either empty or contains the merged element.
        /// </returns>
        public static Maybe<TResult> Zip<TFirst, TSecond, TResult>(this Maybe<TFirst> first, Maybe<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first.HasValue && second.HasValue)
            {
                return resultSelector(first.Value, second.Value);
            }
            return Empty;
        }
        /// <summary>
        /// Zips the corresponding elements of two containers, producing a container with tupled result.
        /// </summary>
        /// <param name="first">The first container to merge.</param>
        /// <param name="second">The second container to merge.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which either empty or contains the tupled element.
        /// </returns>
        public static Maybe<ValueTuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this Maybe<TFirst> first, Maybe<TSecond> second)
        {
            if (first.HasValue && second.HasValue)
            {
                return (first.Value, second.Value);
            }
            return Empty;
        }
    }
    /// <summary>
    /// Represents container that may contain an element. Can be considered extensions of the
    /// <see cref="System.Nullable{T}" /> for non value types.
    /// </summary>
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        internal static readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;
        /// <summary>
        /// Implicit conversion from spcified value.
        /// </summary>
        /// <param name="value">Value to store.</param>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which contains the specfied value.
        /// </returns>
        public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);
        /// <summary>
        /// Hack to support <c>null</c> as <see cref="T:NCoreUtils.Maybe{T}" />.
        /// </summary>
        /// <param name="dummy">Not used.</param>
        /// <returns>
        /// An empty <see cref="T:NCoreUtils.Maybe{T}" />.
        /// </returns>
        public static implicit operator Maybe<T>(Maybe.NoValueClass dummy) => new Maybe<T>();
        /// <summary>
        /// Overrides default equality comparison.
        /// </summary>
        /// <param name="a">First container to compare.</param>
        /// <param name="b">Second container to compare.</param>
        /// <returns>
        /// <c>true</c> either if both containers are empty or both contains equal value, <c>false</c> otherwise.
        /// </returns>
        public static bool operator==(Maybe<T> a, Maybe<T> b) => a.Equals(b);
        /// <summary>
        /// Overrides default inequality comparison.
        /// </summary>
        /// <param name="a">First container to compare.</param>
        /// <param name="b">Second container to compare.</param>
        /// <returns>
        /// <c>false</c> either if both containers are empty or both contains equal value, <c>true</c> otherwise.
        /// </returns>
        public static bool operator!=(Maybe<T> a, Maybe<T> b) => !a.Equals(b);
        readonly T _value;
        readonly bool _hasValue;
        /// <summary>
        /// Stored value.
        /// </summary>
        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get => _value;
        }
        /// <summary>
        /// Determines whether the container is not empty.
        /// </summary>
        public bool HasValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [DebuggerStepThrough]
            get => _hasValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        internal Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }
        /// <summary>
        /// Casts the stored value to the specified type.
        /// </summary>
        /// <returns>
        /// A <see cref="T:NCoreUtils.Maybe{T}" /> which contains the casted value.
        /// </returns>
        /// <exception cref="T:System.InvalidCastException">
        /// Thrown if cast to the specified type is invalid.
        /// </exception>
        public Maybe<TResult> Cast<TResult>()
            => HasValue ? new Maybe<TResult>((TResult)(object)Value) : Maybe.Empty;
        /// <summary>
        /// Determines whether the specified container equals to the actual one.
        /// </summary>
        /// <param name="other">Container to compare.</param>
        /// <returns>
        /// <c>true</c> either if both containers are empty or both contains equal value, <c>false</c> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [DebuggerStepThrough]
        public bool Equals(Maybe<T> other)
        {
            if (HasValue)
            {
                return other.HasValue && _comparer.Equals(Value, other.Value);
            }
            return !other.HasValue;
        }
        /// <summary>
        /// Determines whether the specified object equals to the actual one.
        /// </summary>
        /// <param name="object">Object to compare.</param>
        /// <returns>
        /// <c>true</c> either if both containers are empty or both contains equal value, <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object @object)
            => null != @object && @object is Maybe<T> other && Equals(other);
        /// <docinherit />
        public override int GetHashCode()
            => HasValue ? Value.GetHashCode() : 0;
        /// <summary>
        /// Calls one of specified callbacks depending on actual state.
        /// </summary>
        /// <param name="onValue">Callback called if the container is not empty.</param>
        /// <param name="onEmpty">Callback called if the container is not empty.</param>
        public void Match(Action<T> onValue, Action onEmpty = null)
        {
            if (HasValue)
            {
                onValue?.Invoke(Value);
            }
            else
            {
                onEmpty?.Invoke();
            }
        }
        /// <summary>
        /// Calls one of specified callbacks depending on actual state.
        /// </summary>
        /// <param name="onValue">Callback called if the container is not empty.</param>
        /// <param name="onEmpty">Callback called if the container is not empty.</param>
        /// <returns>Return value of called callback.</returns>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onEmpty)
            => HasValue ? onValue(Value) : onEmpty();
        /// <summary>
        /// Returns either empty <see cref="T:NCoreUtils.Maybe{TResult}" /> or
        /// <see cref="T:NCoreUtils.Maybe{TResult}" /> containing the upcasted element.
        /// </summary>
        /// <returns>
        /// Either empty <see cref="T:NCoreUtils.Maybe{TResult}" /> or <see cref="T:NCoreUtils.Maybe{TResult}" />
        /// containing the upcasted element.
        /// </returns>
        public Maybe<TResult> OfType<TResult>() where TResult : T
        {
            if (!HasValue)
            {
                return Maybe.Empty;
            }
            if (Value is TResult result)
            {
                return result;
            }
            return Maybe.Empty;
        }
        /// <summary>
        /// Retrieves the stored value if straightforward manner.
        /// </summary>
        /// <param name="value">Variable to return the value if present.</param>
        /// <returns>
        /// <c>true</c>, if value was present, <c>false</c> otherwise.
        /// </returns>
        public bool TryGetValue(out T value)
        {
            value = Value;
            return HasValue;
        }
    }
}