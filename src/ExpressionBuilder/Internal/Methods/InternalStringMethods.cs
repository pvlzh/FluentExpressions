using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions.Expressions;
using ExpressionBuilder.Abstractions.Methods;

namespace ExpressionBuilder.Internal.Methods;

/// <inheritdoc />
internal class InternalStringMethods<TSource> : StringMethods<TSource> 
{
    public InternalStringMethods(MemberExpression<TSource, string> memberPath) 
        : base(memberPath)
    {
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> StartWith(string value)
    {
        var parameter = Expression.Constant(value);
        var startWithMethod = StartWithMethod(typeof(string));
        var callExpression = Expression.Call(MemberPath, startWithMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, MemberPath.Parameters);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> EndWith(string value)
    {
        var parameter = Expression.Constant(value);
        var endWithMethod = EndWithMethod(typeof(string));
        var callExpression = Expression.Call(MemberPath, endWithMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, MemberPath.Parameters);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> Contains(string value)
    {
        var parameter = Expression.Constant(value);
        var containsMethod = ContainsMethod(typeof(string));
        var callExpression = Expression.Call(MemberPath, containsMethod, parameter);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, MemberPath.Parameters);
    }
    
    
    private static MethodInfo ContainsMethod(Type genericType)
    {
        var containsInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.Contains) && m.GetParameters().Length == 1);

        return containsInfo;
    }
    
    private static MethodInfo StartWithMethod(Type genericType)
    {
        var startWithInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.StartsWith) && m.GetParameters().Length == 1);

        return startWithInfo;
    }
    
    private static MethodInfo EndWithMethod(Type genericType)
    {
        var endWithInfo = typeof(string).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name == nameof(string.EndsWith) && m.GetParameters().Length == 1);

        return endWithInfo;
    }
}