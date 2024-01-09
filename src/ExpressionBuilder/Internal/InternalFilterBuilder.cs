using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;
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
        Expression = Expression.AndAlso(additionalPredicate);
        return this;
    }

    /// <inheritdoc />
    public override FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate)
    {
        Expression = Expression.OrElse(additionalPredicate);
        return this;
    }
}