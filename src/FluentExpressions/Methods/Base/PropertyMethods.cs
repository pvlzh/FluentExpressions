using System;
using System.Linq.Expressions;
using FluentExpressions.Extensions;

namespace FluentExpressions.Methods.Base;

/// <summary>
/// An abstract class of operations on a field or property of an object with the type <see cref="TSource"/>
/// </summary>
/// <typeparam name="TSource"> The type of the source element.</typeparam>
/// <typeparam name="TProperty"> Type of property.</typeparam>
public abstract class PropertyMethods<TSource, TProperty>
{
    protected readonly MemberExpression MemberExpression;
    protected readonly ParameterExpression SourceParameter;

    /// <inheritdoc cref="PropertyMethods{TSource, TProperty}"/>
    /// <param name="memberExpression"> Expression of access to a field or property of an element.</param>
    protected PropertyMethods(Expression<Func<TSource, TProperty>> memberExpression)
    {
        MemberExpression = memberExpression.GetMemberExpression();
        SourceParameter = memberExpression.Parameters[0];
    }
}