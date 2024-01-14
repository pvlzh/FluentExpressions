using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Expressions;
using ExpressionBuilder.Abstractions.Methods.Base;

namespace ExpressionBuilder.Abstractions.Methods;

/// <summary>
/// Методы предиката коллекции.
/// </summary>
/// <typeparam name="TSource"> Тип исходного элемента.</typeparam>
/// <typeparam name="TItem"> Тип элементов коллекции.</typeparam>
public abstract class CollectionMethods<TSource, TItem> 
    : PropertyMethods<TSource, IEnumerable<TItem>>
{
    protected CollectionMethods(MemberExpression<TSource, IEnumerable<TItem>> memberPath) 
        : base(memberPath)
    {
    }

    /// <summary>
    /// Определяет, удовлетворяет ли хотя бы элемент последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Предикат элемента коллекции.</param>
    public abstract Expression<Func<TSource, bool>> Any(Expression<Func<TItem, bool>> itemPredicate);

    /// <summary>
    /// Определяет, удовлетворяют ли все элементы последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Предикат элемента коллекции.</param>
    public abstract Expression<Func<TSource, bool>> All(Expression<Func<TItem, bool>> itemPredicate);
}