using ExpressionBuilder.Abstractions.Expressions;

namespace ExpressionBuilder.Abstractions.Methods.Base;

public abstract class PropertyMethods<TSource, TProperty>
{
    protected readonly MemberExpression<TSource, TProperty> MemberPath;

    protected PropertyMethods(MemberExpression<TSource, TProperty> memberPath)
    {
        MemberPath = memberPath;
    }
}