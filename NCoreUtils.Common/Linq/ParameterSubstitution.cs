using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace NCoreUtils.Linq
{
    /// <summary>
    /// Substitutes parameter nodes in the source expression with respect to the specified substitutions.
    /// </summary>
    public class ParameterSubstitution : ExpressionVisitor
    {
        /// <summary>
        /// Substitution rules.
        /// </summary>
        public IReadOnlyDictionary<ParameterExpression, Expression> Substitutions { get; }
        /// <summary>
        /// Initializes new instance of <see cref="T:NCoreUtils.Linq.ParameterSubstitution" /> with the specified
        /// substitution rules.
        /// </summary>
        /// <param name="substitutions"></param>
        public ParameterSubstitution(IEnumerable<KeyValuePair<ParameterExpression, Expression>> substitutions)
        {
            RuntimeAssert.ArgumentNotNull(substitutions, nameof(substitutions));
            switch (substitutions)
            {
                case ImmutableDictionary<ParameterExpression, Expression> dict:
                    Substitutions = dict;
                    break;
                default:
                    Substitutions = substitutions.ToImmutableDictionary();
                    break;
            }
        }

        /// <summary>
        /// Performes parameter expression substitution if matching rule is present.
        /// </summary>
        /// <param name="node">Visited node.</param>
        /// <returns>Either passed node or substituted node depending on whether matching rule has been found.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
            => Substitutions.TryGetValue(node, out var substitution) ? substitution : node;
    }
}