using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentExpressions.Methods.Base;

namespace FluentExpressions.Methods;

/// <summary>
/// Операций над полем или свойством типа <see cref="IEnumerable{TItem}"/> объекта <see cref="TSource"/>.
/// </summary>
/// <typeparam name="TSource"> Тип исходного элемента.</typeparam>
/// <typeparam name="TItem"> Тип элементов коллекции.</typeparam>
public class CollectionOptions<TSource, TItem> : PropertyMethods<TSource, IEnumerable<TItem>>
{
    /// <inheritdoc cref="PropertyMethods{TSource, TProperty}"/>
    internal CollectionOptions(Expression<Func<TSource, IEnumerable<TItem>>> memberExpression) 
        : base(memberExpression)
    {
    }

    /// <summary>
    /// Определяет, удовлетворяет ли хотя бы элемент последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Логическое выражение над элементом коллекции.</param>
    public Expression<Func<TSource, bool>> Any(Expression<Func<TItem, bool>> itemPredicate)
    {
        var anyMethod = AnyMethod(typeof(TItem));
        var callExpression = Expression.Call(anyMethod, MemberExpression, itemPredicate);
        return Expression.Lambda<Func<TSource, bool>>(callExpression, SourceParameter);
    }

    /// <summary>
    /// Определяет, удовлетворяют ли все элементы последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Логическое выражение над элементом коллекции.</param>
    public Expression<Func<TSource, bool>> All(Expression<Func<TItem, bool>> itemPredicate)
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