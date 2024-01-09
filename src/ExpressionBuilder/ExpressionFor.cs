using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Internal;

namespace ExpressionBuilder;

public static class ExpressionFor<TSource>
{
    public static FilterBuilder<TSource> Where(Expression<Func<TSource, bool>> startExpression)
    {
        return new InternalFilterBuilder<TSource>(startExpression);
    }

    public static ProjectionBuilder<TSource, TDestination> Select<TDestination>(
        Expression<Func<TSource, TDestination>> selectExpression)
    {
        return new InternalProjectionBuilder<TSource, TDestination>(selectExpression);
    }
}