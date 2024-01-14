using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Abstractions.Methods;
using ExpressionBuilder.Internal;
using ExpressionBuilder.Internal.Methods;

namespace ExpressionBuilder;

public static class ExpressionFor<TSource>
{
    public static FilterBuilder<TSource> Where(Expression<Func<TSource, bool>> startExpression)
    {
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static FilterBuilder<TSource> Where<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionMethods<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new InternalCollectionMethods<TSource, TItem>(collectionProperty);
        var startExpression = collectionDelegate?.Invoke(collectionMethods) ?? (_ => true);
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static FilterBuilder<TSource> Where(
        Expression<Func<TSource, string>> stringProperty, 
        Func<StringMethods<TSource>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new InternalStringMethods<TSource>(stringProperty);
        var startExpression = collectionDelegate?.Invoke(collectionMethods) ?? (_ => true);
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static ProjectionBuilder<TSource, TDestination> Select<TDestination>(
        Expression<Func<TSource, TDestination>> selectExpression)
    {
        return new InternalProjectionBuilder<TSource, TDestination>(selectExpression);
    }
}