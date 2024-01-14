using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions.Expressions;
using ExpressionBuilder.Abstractions.Methods;

namespace ExpressionBuilder.Internal.Methods;

/// <inheritdoc />
internal class InternalCollectionMethods<TSource ,TItem> 
    : CollectionMethods<TSource, TItem>
{
    public InternalCollectionMethods(MemberExpression<TSource, IEnumerable<TItem>> memberPath)
        : base(memberPath)
    {
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> Any(Expression<Func<TItem, bool>> itemPredicate)
    {
        var anyMethod = AnyMethod(typeof(TItem));
        var callExpression = Expression.Call(anyMethod, MemberPath, itemPredicate);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, MemberPath.Parameters);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> All(Expression<Func<TItem, bool>> itemPredicate)
    {
        var allMethod = AllMethod(typeof(TItem));
        var callExpression = Expression.Call(allMethod, MemberPath, itemPredicate);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, MemberPath.Parameters);
    }


    private static MethodInfo AnyMethod(Type genericType)
    {
        var anyInfo = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2);

        return anyInfo.MakeGenericMethod(genericType);
    }

    private static MethodInfo AllMethod(Type genericType)
    {
        var anyInfo = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Enumerable.All) && m.GetParameters().Length == 2);

        return anyInfo.MakeGenericMethod(genericType);
    }
}