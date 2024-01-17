using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionBuilder.Extensions;

namespace ExpressionBuilder.Abstractions;

/// <summary>
/// Строитель выражения проекции одного объекта на другой.
/// </summary>
/// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
/// <typeparam name="TDestination"> Тип результирующего.</typeparam>
public abstract class ProjectionBuilder<TSource, TDestination> 
{
    protected MemberInitExpression ProjectionExpression { get; set; }
    protected ParameterExpression SourceParameter { get; set; }

    protected ProjectionBuilder(Expression<Func<TSource, TDestination>> startProjection)
    {
        ProjectionExpression = startProjection.GetMemberInitExpression();
        SourceParameter = startProjection.Parameters[0];
    }


    public abstract ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, TDestProperty>> to, 
        Expression<Func<TSource, TSourceProperty>> from,
        Expression<Func<TSourceProperty, TDestProperty>> projectionExpression);
    
    public abstract ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, IEnumerable<TDestProperty>>> to, 
        Expression<Func<TSource, IEnumerable<TSourceProperty>>> from,
        Expression<Func<TSourceProperty, TDestProperty>> itemProjectionExpression);

    public static implicit operator Expression<Func<TSource, TDestination>>(
        ProjectionBuilder<TSource, TDestination> builder) => 
        Expression.Lambda<Func<TSource, TDestination>>(builder.ProjectionExpression, builder.SourceParameter);
}