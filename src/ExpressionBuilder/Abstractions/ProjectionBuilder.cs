using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Abstractions;

/// <summary>
/// Строитель выражения проекции одного объекта на другой.
/// </summary>
/// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
/// <typeparam name="TDestination"> Тип результирующего.</typeparam>
public abstract class ProjectionBuilder<TSource, TDestination>
{
    protected Expression<Func<TSource, TDestination>> Expression { get; set; }

    protected ProjectionBuilder(Expression<Func<TSource, TDestination>> startProjection)
    {
        Expression = startProjection;
    }

    public static implicit operator Expression<Func<TSource, TDestination>>(
        ProjectionBuilder<TSource, TDestination> builder) => builder.Expression;
}