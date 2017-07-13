using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace NCoreUtils.Linq
{
    /// <summary>
    /// Contains expression splicing extensions.
    /// </summary>
    public static class EmbedExtensions
    {
        static T ThrowInvalidOperation<T>()
            => throw new InvalidOperationException("This method is not intended to be called at runtime.");

        /// <summary>
        /// Substitutes parameter nodes in the source expression with respect to the specified substitutions.
        /// </summary>
        /// <param name="source">Source expression.</param>
        /// <param name="substitutions">Substitutions.</param>
        /// <returns>Resulting expression.</returns>
        public static Expression SubstituteParameters(this Expression source, IEnumerable<KeyValuePair<ParameterExpression, Expression>> substitutions)
        {
            RuntimeAssert.ArgumentNotNull(source, nameof(source));
            return new ParameterSubstitution(substitutions).Visit(source);
        }

        /// <summary>
        /// Substitutes parameter node in the source expression with respect to the specified arguments.
        /// </summary>
        /// <param name="source">Source expression.</param>
        /// <param name="parameter">Parameter node to substitute.</param>
        /// <param name="substitution">Expression to perform substitution with.</param>
        /// <returns>Resulting expression.</returns>
        public static Expression SubstituteParameter(this Expression source, ParameterExpression parameter, Expression substitution)
        {
            RuntimeAssert.ArgumentNotNull(parameter, nameof(parameter));
            RuntimeAssert.ArgumentNotNull(substitution, nameof(substitution));
            return source.SubstituteParameters(ImmutableDictionary.CreateRange(new KeyValuePair<ParameterExpression, Expression>[] {
                new KeyValuePair<ParameterExpression, Expression>(parameter, substitution)
            }));
        }

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg">Value to be passed as parameter.</param>
        public static TResult EmbedCall<TArg, TResult>(this Expression<Func<TArg, TResult>> expression, TArg arg)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg1">Value to be passed as first parameter.</param>
        /// <param name="arg2">Value to be passed as second parameter.</param>
        public static TResult EmbedCall<TArg1, TArg2, TResult>(this Expression<Func<TArg1, TArg2, TResult>> expression, TArg1 arg1, TArg2 arg2)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg1">Value to be passed as first parameter.</param>
        /// <param name="arg2">Value to be passed as second parameter.</param>
        /// <param name="arg3">Value to be passed as third parameter.</param>
        public static TResult EmbedCall<TArg1, TArg2, TArg3, TResult>(this Expression<Func<TArg1, TArg2, TArg3, TResult>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg1">Value to be passed as first parameter.</param>
        /// <param name="arg2">Value to be passed as second parameter.</param>
        /// <param name="arg3">Value to be passed as third parameter.</param>
        /// <param name="arg4">Value to be passed as fourth parameter.</param>
        public static TResult EmbedCall<TArg1, TArg2, TArg3, TArg4, TResult>(this Expression<Func<TArg1, TArg2, TArg3, TArg4, TResult>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg1">Value to be passed as first parameter.</param>
        /// <param name="arg2">Value to be passed as second parameter.</param>
        /// <param name="arg3">Value to be passed as third parameter.</param>
        /// <param name="arg4">Value to be passed as fourth parameter.</param>
        /// <param name="arg5">Value to be passed as fifth parameter.</param>
        public static TResult EmbedCall<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(this Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed lambda expression invocation into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Lambda expression to embed.</param>
        /// <param name="arg1">Value to be passed as first parameter.</param>
        /// <param name="arg2">Value to be passed as second parameter.</param>
        /// <param name="arg3">Value to be passed as third parameter.</param>
        /// <param name="arg4">Value to be passed as fourth parameter.</param>
        /// <param name="arg5">Value to be passed as fifth parameter.</param>
        /// <param name="arg6">Value to be passed as sixth parameter.</param>
        public static TResult EmbedCall<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(this Expression<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> expression, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5, TArg6 arg6)
            => ThrowInvalidOperation<TResult>();

        /// <summary>
        /// Used to embed expression as typed value into the resulting expression tree. Not intended to be used at
        /// runtime.
        /// </summary>
        /// <param name="expression">Expression to embed.</param>
        public static T EmbedAs<T>(this Expression expression)
            => ThrowInvalidOperation<T>();

        /// <summary>
        /// Resolves all embedded expressions within the specified expression tree.
        /// </summary>
        /// <param name="expression">Expression to resolve embedded expressions within.</param>
        /// <returns>Resulting expression.</returns>
        public static T Splice<T>(this T expression)
            where T : Expression
            => EmbedInliner.SharedInstance.VisitAndConvert(expression, nameof(Splice));
    }
}