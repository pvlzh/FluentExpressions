using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions;

namespace ExpressionBuilder.Internal;

/// <summary>
/// Внутренняя реализация абстракции строителя проекции.
/// </summary>
/// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
/// <typeparam name="TDestination"> Тип результирующего.</typeparam>
internal sealed class InternalProjectionBuilder<TSource, TDestination> 
    : ProjectionBuilder<TSource, TDestination>
{
    /// <inheritdoc cref="InternalProjectionBuilder{TSource, TDestination}"/>
    public InternalProjectionBuilder(Expression<Func<TSource, TDestination>> startProjection) 
        : base(startProjection)
    {
    }
}