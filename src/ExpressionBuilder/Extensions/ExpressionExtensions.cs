using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Extensions;

/// <summary>
/// Методы расширения для выражений.
/// </summary>
public static class ExpressionExtensions
{
    public static Expression<Func<TSource, bool>> AndAlso<TSource>(
        this Expression<Func<TSource, bool>> sourceExpression, 
        Expression<Func<TSource, bool>> additionalExpression)
    {
        var andExpression = Expression.AndAlso(sourceExpression.Body, 
            Expression.Invoke(additionalExpression, sourceExpression.Parameters));
        
        return Expression.Lambda<Func<TSource, bool>>(andExpression, sourceExpression.Parameters);
    }
    
    public static Expression<Func<TSource, bool>> OrElse<TSource>(
        this Expression<Func<TSource, bool>> sourceExpression, 
        Expression<Func<TSource, bool>> additionalExpression)
    {
        var orExpression = Expression.OrElse(sourceExpression.Body, 
            Expression.Invoke(additionalExpression, sourceExpression.Parameters));
        
        return Expression.Lambda<Func<TSource, bool>>(orExpression, sourceExpression.Parameters);
    }
}