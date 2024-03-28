using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Builders;
using FluentExpressions.Methods;

namespace FluentExpressions;

public static class ExpressionFor<TSource>
{
    /// <summary>
    /// Set a filter rule for the sequence <see cref="TSource"/>
    /// </summary>
    /// <param name="startExpression"> The initial filtering rule.</param>
    /// <returns> Filtering Expression Builder.</returns>
    public static FilterBuilder<TSource> Where(Expression<Func<TSource, bool>> startExpression)
    {
        return new FilterBuilder<TSource>(startExpression);
    }

    /// <summary>
    /// Set a filter rule for the sequence <see cref="TSource"/>
    /// </summary>
    /// <param name="collectionProperty"> Expression to a property of a linked collection.</param>
    /// <param name="collectionDelegate"> Operation on a collection.</param>
    /// <typeparam name="TItem"> Type of the collection item.</typeparam>
    /// <returns> Filtering Expression Builder.</returns>
    public static FilterBuilder<TSource> Where<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var startExpression = FilterBuilder<TSource>.CreateCollectionPredicate(collectionProperty, collectionDelegate);
        return Where(startExpression);
    }
    
    
    /// <summary>
    /// If-Else statement expression.
    /// </summary>
    /// <param name="condition"> The initial condition.</param>
    /// <param name="then"> The body of the expression for the positive result of the condition.</param>
    /// <returns> <see cref="ConditionBuilder{TSource, TDestination}"/>.</returns>
    public static ConditionBuilder<TSource, TDestination> If<TDestination>(
        Expression<Func<TSource, bool>> condition, TDestination then)
    {
        return new ConditionBuilder<TSource, TDestination>(condition, then);
    }
}