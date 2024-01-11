#nullable enable
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Abstractions.Collections;

namespace ExpressionBuilder.Internal.Collections;

internal class InternalCollectionPredicate<TItem> : CollectionPredicate<TItem>
{
    private readonly MemberExpression _memberExpression;
    private Expression? PredicateExpression { get; set; }

    public InternalCollectionPredicate(MemberExpression memberExpression)
    {
        _memberExpression = memberExpression;
    }
    public override void Any(Expression<Func<TItem, bool>> itemPredicate)
    {
        var anyMethod = AnyMethod(typeof(TItem));
        PredicateExpression = Expression.Call(anyMethod, _memberExpression, itemPredicate);
    }

    public override void All(Expression<Func<TItem, bool>> itemPredicate)
    {
        var anyMethod = AllMethod(typeof(TItem));
        PredicateExpression = Expression.Call(anyMethod, _memberExpression, itemPredicate);
    }

    public static implicit operator Expression(InternalCollectionPredicate<TItem> collectionPredicate)
    {
        return collectionPredicate.PredicateExpression ?? Expression.Constant(true);
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