using System;
using System.Linq.Expressions;
using ExpressionBuilder.Extensions;

namespace ExpressionBuilder.Methods.Base;

/// <summary>
/// Абстрактный класс операций над полем или свойством объекта с типом <see cref="TSource"/>
/// </summary>
/// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
/// <typeparam name="TProperty"> Тип поля или свойства исходного объекта.</typeparam>
public abstract class PropertyMethods<TSource, TProperty>
{
    protected readonly MemberExpression MemberExpression;
    protected readonly ParameterExpression SourceParameter;

    /// <inheritdoc cref="PropertyMethods{TSource, TProperty}"/>
    /// <param name="memberExpression"> Выражение доступа к полю или свойству элемента.</param>
    protected PropertyMethods(Expression<Func<TSource, TProperty>> memberExpression)
    {
        MemberExpression = memberExpression.GetMemberExpression();
        SourceParameter = memberExpression.Parameters[0];
    }
}