using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Visitors;

namespace FluentExpressions.Extensions;

/// <summary>
/// Extension methods for expressions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Lead to lambda expression.
    /// </summary>
    /// <param name="expression"> The original expression.</param>
    /// <param name="parameters"> Lambda parameters.</param>
    /// <typeparam name="TSource"> Type of predicate input parameter.</typeparam>
    /// <returns> Lambda expression</returns>
    public static Expression<Func<TSource, bool>> ToLambdaExpression<TSource>(
        this BinaryExpression expression,
        IReadOnlyCollection<ParameterExpression> parameters) =>
            Expression.Lambda<Func<TSource, bool>>(expression, parameters);
         
    /// <summary>
    /// Get an expression for accessing a field or property of an element.
    /// </summary>
    /// <param name="expression"> Lambda to a field or property of an element.</param>
    /// <typeparam name="TSource"> The type of the source object.</typeparam>
    /// <typeparam name="TProperty"> Type of field or property.</typeparam>
    /// <returns> <see cref="MemberExpression"/> Accessing a field or property.</returns>
    /// <exception cref="ArgumentException"> A lambda expression does not represent access to a field or property.</exception>
    public static MemberExpression GetMemberExpression<TSource, TProperty>(
        this Expression<Func<TSource, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"Expression '{expression}' is not represents " +
                                        $"accessing a field or property");
        }

        return memberExpression;
    }
    
    /// <summary>
    /// Replace the parameter in the expression.
    /// </summary>
    /// <param name="expressionTree"> The expression in which the replacement will be performed.</param>
    /// <param name="replaceableParameter"> The parameter that needs to be replaced.</param>
    /// <param name="replacement"> The expression used as a substitute.</param>
    /// <returns> An expression with a replaced parameter.</returns>
    /// <exception cref="Exception"> Failed to replace.</exception>
    public static Expression ReplaceParameter(this Expression expressionTree, ParameterExpression replaceableParameter, Expression replacement)
    {
        var visitor = new ParameterReplacingVisitor(replaceableParameter, replacement);
        var result = visitor.Visit(expressionTree);
        if (result == null)
        {
            throw new Exception($"Failed to replace the parameter '{replaceableParameter}' " +
                                $"in the expression tree '{expressionTree}'");
        }
        return result;
    }
}