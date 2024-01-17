using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions.Methods;

namespace ExpressionBuilder.Internal.Methods;

/// <inheritdoc />
internal class InternalStringMethods<TSource> : StringMethods<TSource> 
{
    public InternalStringMethods(Expression<Func<TSource, string>> memberExpression) 
        : base(memberExpression)
    {
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> StartWith(string value)
    {
        var parameter = Expression.Constant(value);
        var startWithMethod = StartWithMethod();
        var callExpression = Expression.Call(MemberExpression, startWithMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> EndWith(string value)
    {
        var parameter = Expression.Constant(value);
        var endWithMethod = EndWithMethod();
        var callExpression = Expression.Call(MemberExpression, endWithMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> Contains(string value)
    {
        var parameter = Expression.Constant(value);
        var containsMethod = ContainsMethod();
        var callExpression = Expression.Call(MemberExpression, containsMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
    }
    
    
    private static MethodInfo ContainsMethod()
    {
        var containsInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.Contains) && m.GetParameters().Length == 1);

        return containsInfo;
    }
    
    private static MethodInfo StartWithMethod()
    {
        var startWithInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.StartsWith) && m.GetParameters().Length == 1);

        return startWithInfo;
    }
    
    private static MethodInfo EndWithMethod()
    {
        var endWithInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.EndsWith) && m.GetParameters().Length == 1);

        return endWithInfo;
    }
}