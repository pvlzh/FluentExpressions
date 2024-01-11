using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Collections;
using ExpressionBuilder.Internal.Collections;

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

    public static Expression<Func<TSource, bool>> Apply<TSource, TItem>(
        this Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty,
        Action<CollectionPredicate<TItem>> collectionMethod)
    {
        if (collectionProperty.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("A non-member expression was passed", nameof(collectionProperty));
        }
        
        var collectionPredicate = new InternalCollectionPredicate<TItem>(memberExpression);
        collectionMethod(collectionPredicate);
        
        return Expression.Lambda<Func<TSource, bool>>(collectionPredicate, collectionProperty.Parameters);
    }
}