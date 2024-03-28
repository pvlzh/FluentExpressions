using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Extensions;
using FluentExpressions.Methods;

namespace FluentExpressions.Builders;

/// <summary>
/// Filtering Expression Builder.
/// </summary>
/// <typeparam name="TSource"> Type of filtering object.</typeparam>
public class FilterBuilder<TSource>
{
    private Expression<Func<TSource, bool>> _predicate;

    /// <inheritdoc cref="FilterBuilder{TSource}"/>
    /// <param name="startPredicate"> Initial filtering expression.</param>
    internal FilterBuilder(Expression<Func<TSource, bool>> startPredicate)
    {
        _predicate = startPredicate;
    }

    /// <summary>
    /// Add a logical expression over <see cref="TSource"/> to link to the source via the logical operator "And".
    /// </summary>
    /// <param name="additionalPredicate"> Boolean expression over <see cref="TSource"/></param>
    /// <returns> Filtering Expression Builder.</returns>
    public FilterBuilder<TSource> And(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var andExpression = Expression.AndAlso(_predicate.Body, ReplaceParameter(additionalPredicate));
        _predicate = andExpression.ToLambdaExpression<TSource>(_predicate.Parameters);
        return this;
    }

    /// <summary>
    /// Add a logical expression over <see cref="TSource"/> to link to the source via the logical operator "OR".
    /// </summary>
    /// <param name="additionalPredicate"> Boolean expression over <see cref="TSource"/></param>
    /// <returns> Filtering Expression Builder.</returns>
    public FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var orExpression = Expression.OrElse(_predicate.Body, ReplaceParameter(additionalPredicate));
        _predicate = orExpression.ToLambdaExpression<TSource>(_predicate.Parameters);
        return this;
    }

    /// <summary>
    /// Add a logical expression over the <see cref="TItem"/> related collection <see cref="TSource"/> using the logical operator "And".
    /// </summary>
    /// <param name="collectionProperty"> Expression to a property of a linked collection.</param>
    /// <param name="collectionDelegate"> Operation on a collection.</param>
    /// <typeparam name="TItem"> Type of the collection item.</typeparam>
    /// <returns> Filtering Expression Builder.</returns>
    public FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty,
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionPredicate = CreateCollectionPredicate(collectionProperty, collectionDelegate);
        And(collectionPredicate);
        return this;
    }

    /// <summary>
    /// Add a logical expression over the <see cref="TItem"/> related collection <see cref="TSource"/> using the logical operator "OR".
    /// </summary>
    /// <param name="collectionProperty"> Expression to a property of a linked collection.</param>
    /// <param name="collectionDelegate"> Operation on a collection.</param>
    /// <typeparam name="TItem"> Type of the collection item.</typeparam>
    /// <returns> Filtering Expression Builder.</returns>
    public FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty,
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionPredicate = CreateCollectionPredicate(collectionProperty, collectionDelegate);
        Or(collectionPredicate);
        return this;
    }

    /// <summary>
    /// Return an expression from the builder.
    /// </summary>
    public Expression<Func<TSource, bool>> ToExpression()
    {
        return _predicate;
    }
    
    /// <summary>
    /// Replace the parameter in the added expression with the parameter of the original one.
    /// </summary>
    /// <param name="additionalPredicate"> Additional condition.</param>
    private Expression ReplaceParameter(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var sourceParameter = _predicate.Parameters[0]; 
        var replaceableParameter = additionalPredicate.Parameters[0]; 
        return additionalPredicate.Body.ReplaceParameter(replaceableParameter, sourceParameter);
    }

    /// <summary>
    /// Create a predicate for <see cref="TSource"/> collection properties.
    /// </summary>
    /// <param name="collectionProperty"> Expression to a property of a linked collection.</param>
    /// <param name="collectionDelegate"> Operation on a collection.</param>
    /// <typeparam name="TItem"> Type of the collection item.</typeparam>
    internal static Expression<Func<TSource, bool>> CreateCollectionPredicate<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new CollectionOptions<TSource, TItem>(collectionProperty);
        var collectionPredicate = collectionDelegate.Invoke(collectionMethods) ?? (_ => true);
        return collectionPredicate;
    }

    public static implicit operator Expression<Func<TSource, bool>>(FilterBuilder<TSource> builder) => builder.ToExpression();
}