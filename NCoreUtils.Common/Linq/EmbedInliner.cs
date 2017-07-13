using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NCoreUtils.Linq
{
    sealed class EmbedInliner : ExpressionVisitor
    {
        static object GetConstantValue(Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }
            if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Member is FieldInfo field)
                {
                    if (field.IsStatic)
                    {
                        return field.GetValue(null);
                    }
                    return field.GetValue(GetConstantValue(memberExpression.Expression));
                }
                if (memberExpression.Member is PropertyInfo property)
                {
                    if (property.GetMethod.IsStatic)
                    {
                        return property.GetValue(null, null);
                    }
                    return property.GetValue(GetConstantValue(memberExpression.Expression), null);
                }
            }
            throw new InvalidOperationException($"Unable to get constant value from {expression}.");
        }
        public static EmbedInliner SharedInstance { get; } = new EmbedInliner();
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType.Equals(typeof(EmbedExtensions)))
            {
                if (node.Method.Name == nameof(EmbedExtensions.EmbedCall))
                {
                    var expr = (LambdaExpression)GetConstantValue(node.Arguments[0]);
                    var arguments = node.Arguments.Skip(1);
                    var parameters = expr.Parameters;
                    var substitutions = parameters.Zip(arguments, (p, a) => new KeyValuePair<ParameterExpression, Expression>(p, a));
                    return Visit(expr.Body.SubstituteParameters(substitutions));
                }
                if (node.Method.Name == nameof(EmbedExtensions.EmbedAs))
                {
                    var expr = (Expression)GetConstantValue(node.Arguments[0]);
                    if (!node.Type.GetTypeInfo().IsAssignableFrom(expr.Type))
                    {
                        throw new InvalidOperationException($"Unable to embed {expr} as {node.Type.FullName}.");
                    }
                    return Visit(expr);
                }
            }
            return node;
        }
    }
}