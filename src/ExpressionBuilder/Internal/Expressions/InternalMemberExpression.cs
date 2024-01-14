using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Expressions;

namespace ExpressionBuilder.Internal.Expressions;

/// <inheritdoc />
internal class InternalMemberExpression<TSource, TMember> : MemberExpression<TSource, TMember>
{
    /// <inheritdoc cref="InternalMemberExpression{TSource, TMember}"/>
    public InternalMemberExpression(Expression<Func<TSource, TMember>> memberPathExpression) 
        : base(memberPathExpression)
    {
    }
}