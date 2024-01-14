using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Abstractions.Methods;
using ExpressionBuilder.Extensions;
using ExpressionBuilder.Internal.Methods;

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
    public override FilterBuilder<TSource> And(
        Expression<Func<TSource, bool>> additionalPredicate)
    {
        PredicateExpression = PredicateExpression.AndAlso(additionalPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or(
        Expression<Func<TSource, bool>> additionalPredicate)
    {
        PredicateExpression = PredicateExpression.OrElse(additionalPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionMethods<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new InternalCollectionMethods<TSource, TItem>(collectionProperty);
        var collectionPredicate = collectionDelegate?.Invoke(collectionMethods) ?? (_ => true);
        PredicateExpression = PredicateExpression.AndAlso(collectionPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionMethods<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new InternalCollectionMethods<TSource, TItem>(collectionProperty);
        var collectionPredicate = collectionDelegate?.Invoke(collectionMethods) ?? (_ => true);
        PredicateExpression = PredicateExpression.OrElse(collectionPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> And(
        Expression<Func<TSource, string>> stringProperty, 
        Func<StringMethods<TSource>, Expression<Func<TSource, bool>>> stringDelegate)
    {
        var stringMethods = new InternalStringMethods<TSource>(stringProperty);
        var stringPredicate = stringDelegate?.Invoke(stringMethods) ?? (_ => true);
        PredicateExpression = PredicateExpression.AndAlso(stringPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or(
        Expression<Func<TSource, string>> stringProperty, 
        Func<StringMethods<TSource>, Expression<Func<TSource, bool>>> stringDelegate)
    {
        var stringMethods = new InternalStringMethods<TSource>(stringProperty);
        var stringPredicate = stringDelegate?.Invoke(stringMethods) ?? (_ => true);
        
        PredicateExpression = PredicateExpression.OrElse(stringPredicate);
        return this;
    }
}