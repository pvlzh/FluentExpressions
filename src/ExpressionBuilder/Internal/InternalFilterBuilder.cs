using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Abstractions.Collections;
using ExpressionBuilder.Extensions;

namespace ExpressionBuilder.Internal;

/// <summary>
/// Внутренняя реализация абстракции строителя фильтра.
/// </summary>
/// <typeparam name="TSource"></typeparam>
internal sealed class InternalFilterBuilder<TSource> : FilterBuilder<TSource>
{
    /// <inheritdoc cref="InternalFilterBuilder{TSource}"/>
    public InternalFilterBuilder(Expression<Func<TSource, bool>> startFiltration) 
        : base(startFiltration)
    {
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> And(Expression<Func<TSource, bool>> additionalPredicate)
    {
        PredicateExpression = PredicateExpression.AndAlso(additionalPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate)
    {
        PredicateExpression = PredicateExpression.OrElse(additionalPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Action<CollectionPredicate<TItem>> collectionMethod)
    {
        var collectionPredicate = collectionProperty.Apply(collectionMethod);
        PredicateExpression = PredicateExpression.AndAlso(collectionPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Action<CollectionPredicate<TItem>> collectionMethod)
    {
        var collectionPredicate = collectionProperty.Apply(collectionMethod);
        PredicateExpression = PredicateExpression.OrElse(collectionPredicate);
        return this;
    }
}