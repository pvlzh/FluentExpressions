using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Methods.Base;

namespace ExpressionBuilder.Abstractions.Methods;

/// <summary>
/// Операций над полем или свойством типа <see cref="IEnumerable{TItem}"/> объекта <see cref="TSource"/>.
/// </summary>
/// <typeparam name="TSource"> Тип исходного элемента.</typeparam>
/// <typeparam name="TItem"> Тип элементов коллекции.</typeparam>
public abstract class CollectionMethods<TSource, TItem> 
    : PropertyMethods<TSource, IEnumerable<TItem>>
{
    /// <inheritdoc cref="PropertyMethods{TSource, TProperty}"/>
    protected CollectionMethods(Expression<Func<TSource, IEnumerable<TItem>>> memberExpression) 
        : base(memberExpression)
    {
    }

    /// <summary>
    /// Определяет, удовлетворяет ли хотя бы элемент последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Логическое выражение над элементом коллекции.</param>
    public abstract Expression<Func<TSource, bool>> Any(Expression<Func<TItem, bool>> itemPredicate);

    /// <summary>
    /// Определяет, удовлетворяют ли все элементы последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Логическое выражение над элементом коллекции.</param>
    public abstract Expression<Func<TSource, bool>> All(Expression<Func<TItem, bool>> itemPredicate);
}