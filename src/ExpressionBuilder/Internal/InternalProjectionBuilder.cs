using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions;
using ExpressionBuilder.Extensions;

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

    /// <inheritdoc />
    public override ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, TDestProperty>> to, 
        Expression<Func<TSource, TSourceProperty>> from,
        Expression<Func<TSourceProperty, TDestProperty>> projectionExpression)
    {
        var sourceMember = from.GetMemberExpression();
        var destMember = to.GetMemberExpression();

        var bindings = ProjectionExpression.Bindings.ToList();
        
        var sourceAccessMember = Expression.MakeMemberAccess(SourceParameter, sourceMember.Member);
        
        var projectionFromSource = projectionExpression.Body.ReplaceParameter(
            projectionExpression.Parameters[0], sourceAccessMember);
        
        var bindToDestMember = Expression.Bind(destMember.Member, projectionFromSource);
        bindings.Add(bindToDestMember);
        
        ProjectionExpression = Expression.MemberInit(
            ProjectionExpression.NewExpression, 
            bindings);
        return this;
    }

    public override ProjectionBuilder<TSource, TDestination> With<TSourceProperty, TDestProperty>(
        Expression<Func<TDestination, IEnumerable<TDestProperty>>> to, 
        Expression<Func<TSource, IEnumerable<TSourceProperty>>> from,
        Expression<Func<TSourceProperty, TDestProperty>> itemProjectionExpression)
    {
        var sourceMember = from.GetMemberExpression();
        var destMember = to.GetMemberExpression();
        
        
        throw new NotImplementedException();
    }


    private static MethodInfo SelectMethod(Type genericType)
    {
        var endWithInfo = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Queryable.Select) && m.GetParameters().Length == 2);

        return endWithInfo;
    }
}