using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentExpressions.Extensions;

namespace FluentExpressions.Builders;
/*
/// <summary>
/// Строитель выражения проекции одного объекта на другой.
/// </summary>
/// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
/// <typeparam name="TDestination"> Тип результирующего.</typeparam>
public class ProjectionBuilder<TSource, TDestination> 
{
    private MemberInitExpression _projection { get; set; }
    private ParameterExpression _sourceParameter { get; set; }

    internal ProjectionBuilder(Expression<Func<TSource, TDestination>> startProjection)
    {
        _projection = startProjection.GetMemberInitExpression();
        _sourceParameter = startProjection.Parameters[0];
    }


    public ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, TDestProperty>> to,
        Expression<Func<TSource, TSourceProperty>> from,
        Expression<Func<TSourceProperty, TDestProperty>> projectionExpression)
    {
        var sourceMember = from.GetMemberExpression();
        var destMember = to.GetMemberExpression();

        var bindings = _projection.Bindings.ToList();
        
        var sourceAccessMember = Expression.MakeMemberAccess(_sourceParameter, sourceMember.Member);
        
        var projectionFromSource = projectionExpression.Body.ReplaceParameter(
            projectionExpression.Parameters[0], sourceAccessMember);
        
        var bindToDestMember = Expression.Bind(destMember.Member, projectionFromSource);
        bindings.Add(bindToDestMember);
        
        _projection = Expression.MemberInit(
            _projection.NewExpression, 
            bindings);
        return this;
    }

    public ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, IEnumerable<TDestProperty>>> to,
        Expression<Func<TSource, IEnumerable<TSourceProperty>>> from,
        Expression<Func<TSourceProperty, TDestProperty>> itemProjectionExpression)
    {
        throw new NotImplementedException();
    }

    private static MethodInfo SelectMethod(Type genericType)
    {
        var endWithInfo = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Queryable.Select) && m.GetParameters().Length == 2);

        return endWithInfo;
    }
    
    public static implicit operator Expression<Func<TSource, TDestination>>(
        ProjectionBuilder<TSource, TDestination> builder) => 
        Expression.Lambda<Func<TSource, TDestination>>(builder._projection, builder._sourceParameter);
}
*/