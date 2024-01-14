using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Internal.Expressions;

namespace ExpressionBuilder.Abstractions.Expressions;

public abstract class MemberExpression<TSource, TMember>
{
    private readonly Expression<Func<TSource, TMember>> _memberPath;

    private readonly MemberExpression _member;
    internal IReadOnlyCollection<ParameterExpression> Parameters => _memberPath.Parameters;

    protected MemberExpression(Expression<Func<TSource, TMember>> memberPathExpression)
    {
        if (memberPathExpression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException("A non-member expression was passed", nameof(memberPathExpression));
        }
        _member = memberExpression;
        _memberPath = memberPathExpression;
    }

    public static implicit operator MemberExpression(
        MemberExpression<TSource, TMember> memberExpression) =>
        memberExpression._member;
    
    public static implicit operator MemberExpression<TSource, TMember>(
        Expression<Func<TSource, TMember>> memberExpression) =>
        new InternalMemberExpression<TSource, TMember>(memberExpression);
}