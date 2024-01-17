using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions.Methods;

namespace ExpressionBuilder.Internal.Methods;

/// <inheritdoc />
internal class InternalCollectionMethods<TSource ,TItem> 
    : CollectionMethods<TSource, TItem>
{
    public InternalCollectionMethods(Expression<Func<TSource, IEnumerable<TItem>>> memberExpression)
        : base(memberExpression)
    {
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> Any(Expression<Func<TItem, bool>> itemPredicate)
    {
        var anyMethod = AnyMethod(typeof(TItem));
        var callExpression = Expression.Call(anyMethod, MemberExpression, itemPredicate);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
    }

    /// <inheritdoc />
    public override Expression<Func<TSource, bool>> All(Expression<Func<TItem, bool>> itemPredicate)
    {
        var allMethod = AllMethod(typeof(TItem));
        var callExpression = Expression.Call(allMethod, MemberExpression, itemPredicate);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
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