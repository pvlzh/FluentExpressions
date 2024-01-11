using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Abstractions.Collections;

/// <summary>
/// Методы предиката коллекции.
/// </summary>
/// <typeparam name="TItem"> Тип элементов коллекции.</typeparam>
public abstract class CollectionPredicate<TItem>
{
    /// <summary>
    /// Определяет, удовлетворяет ли хотя бы элемент последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Предикат элемента коллекции.</param>
    public abstract void Any(Expression<Func<TItem, bool>> itemPredicate);
    
    /// <summary>
    /// Определяет, удовлетворяют ли все элементы последовательности условию.
    /// </summary>
    /// <param name="itemPredicate"> Предикат элемента коллекции.</param>
    public abstract void All(Expression<Func<TItem, bool>> itemPredicate);
}