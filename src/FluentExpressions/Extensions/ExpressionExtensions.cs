using System;
using System.Linq.Expressions;

namespace FluentExpressions.Extensions;

/// <summary>
/// Extension methods for expressions.
/// </summary>
public static class ExpressionExtensions
{
    public static Expression<Func<TSource, bool>> GreaterThan<TSource>(
        this Expression<Func<TSource, int>> sourceExpression, int value)
    {
        var parameter = Expression.Constant(value);
        var expression = Expression.GreaterThan(sourceExpression.Body, parameter)
            .ToLambdaExpression<TSource>(sourceExpression.Parameters);
        return expression;
    }
    
    public static Expression<Func<TSource, bool>> GreaterThan<TSource>(
        this Expression<Func<TSource, int>> sourceExpression, 
        Expression<Func<TSource, int>> propertyExpression)
    {
        var property = ReduceToCommonParameter(sourceExpression, propertyExpression);
        var expression = Expression.GreaterThan(sourceExpression.Body, property)
            .ToLambdaExpression<TSource>(sourceExpression.Parameters);
        return expression;
    }
    
    public static Expression<Func<TSource, bool>> LessThan<TSource>(
        this Expression<Func<TSource, int>> sourceExpression, int value)
    {
        var parameter = Expression.Constant(value);
        var expression = Expression.LessThan(sourceExpression.Body, parameter)
            .ToLambdaExpression<TSource>(sourceExpression.Parameters);
        return expression;
    }
    
    public static Expression<Func<TSource, bool>> LessThan<TSource>(
        this Expression<Func<TSource, int>> sourceExpression, 
        Expression<Func<TSource, int>> propertyExpression)
    {
        var property = ReduceToCommonParameter(sourceExpression, propertyExpression);
        var expression = Expression.LessThan(sourceExpression.Body, property)
            .ToLambdaExpression<TSource>(sourceExpression.Parameters);
        return expression;
    }

    /// <summary>
    /// To reduce the property expression to a common parameter.
    /// </summary>
    /// <param name="sourceExpression"> The original expression.</param>
    /// <param name="propertyExpression"> Source property expression.</param>
    /// <typeparam name="TSource"> Type of input value.</typeparam>
    /// <returns></returns>
    private static Expression ReduceToCommonParameter<TSource>(Expression<Func<TSource, int>> sourceExpression, Expression<Func<TSource, int>> propertyExpression)
    {
        var replaceableParameter = propertyExpression.Parameters[0];
        var sourceParameter = sourceExpression.Parameters[0];
        
        return propertyExpression.Body.ReplaceParameter(replaceableParameter, sourceParameter);
    }
}