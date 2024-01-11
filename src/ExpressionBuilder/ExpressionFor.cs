using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Abstractions.Collections;
using ExpressionBuilder.Extensions;
using ExpressionBuilder.Internal;

namespace ExpressionBuilder;

public static class ExpressionFor<TSource>
{
    public static FilterBuilder<TSource> Where(Expression<Func<TSource, bool>> startExpression)
    {
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static FilterBuilder<TSource> Where<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Action<CollectionPredicate<TItem>> collectionMethod)
    {
        var startExpression = collectionProperty.Apply(collectionMethod);
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static ProjectionBuilder<TSource, TDestination> Select<TDestination>(
        Expression<Func<TSource, TDestination>> selectExpression)
    {
        return new InternalProjectionBuilder<TSource, TDestination>(selectExpression);
    }
}