using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Extensions;
using FluentExpressions.Methods;

namespace FluentExpressions.Builders;

/// <summary>
/// Строитель выражения фильтрации.
/// </summary>
/// <typeparam name="TSource"> Тип объекта фильтрации.</typeparam>
public class FilterBuilder<TSource>
{
    private Expression<Func<TSource, bool>> _predicate;

    /// <inheritdoc cref="FilterBuilder{TSource}"/>
    /// <param name="startPredicate"> Начальное выражение фильтрации.</param>
    internal FilterBuilder(Expression<Func<TSource, bool>> startPredicate)
    {
        _predicate = startPredicate;
    }

    /// <summary>
    /// Дополнить логическим выражением над <see cref="TSource"/> связв с исходным через логический оператор "И".
    /// </summary>
    /// <param name="additionalPredicate"> Логическое выражение над <see cref="TSource"/></param>
    /// <returns> Строитель выражения фильтрации.</returns>
    public FilterBuilder<TSource> And(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var andExpression = Expression.AndAlso(_predicate.Body, ReplaceParameter(additionalPredicate));
        _predicate = andExpression.ToLambdaExpression<TSource>(_predicate.Parameters);
        return this;
    }

    /// <summary>
    /// Дополнить логическим выражением над <see cref="TSource"/> связв с исходным через логический оператор "ИЛИ".
    /// </summary>
    /// <param name="additionalPredicate"> Логическое выражение над <see cref="TSource"/></param>
    /// <returns> Строитель выражения фильтрации.</returns>
    public FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var orExpression = Expression.OrElse(_predicate.Body, ReplaceParameter(additionalPredicate));
        _predicate = orExpression.ToLambdaExpression<TSource>(_predicate.Parameters);
        return this;
    }

    /// <summary>
    /// Дополнить логическим выражением над <see cref="TItem"/> связанной коллекции <see cref="TSource"/> через логический оператор "И".
    /// </summary>
    /// <param name="collectionProperty"> Выражение к свойству связанной коллекции.</param>
    /// <param name="collectionDelegate"> Операция над коллекцией.</param>
    /// <typeparam name="TItem"> Тип элемента коллекции.</typeparam>
    /// <returns> Строитель выражения фильтрации.</returns>
    public FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty,
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionPredicate = CreateCollectionPredicate(collectionProperty, collectionDelegate);
        And(collectionPredicate);
        return this;
    }

    /// <summary>
    /// Дополнить логическим выражением над <see cref="TItem"/> связанной коллекции <see cref="TSource"/> через логический оператор "ИЛИ".
    /// </summary>
    /// <param name="collectionProperty"> Выражение к свойству связанной коллекции.</param>
    /// <param name="collectionDelegate"> Операция над коллекцией.</param>
    /// <typeparam name="TItem"> Тип элемента коллекции.</typeparam>
    /// <returns> Строитель выражения фильтрации.</returns>
    public FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty,
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionPredicate = CreateCollectionPredicate(collectionProperty, collectionDelegate);
        Or(collectionPredicate);
        return this;
    }
    
    /// <summary>
    /// Заменить параметр в добавляемом выражении на параметр исходного.
    /// </summary>
    /// <param name="additionalPredicate"></param>
    private Expression ReplaceParameter(Expression<Func<TSource, bool>> additionalPredicate)
    {
        var sourceParameter = _predicate.Parameters[0]; 
        var replaceableParameter = additionalPredicate.Parameters[0]; 
        return additionalPredicate.Body.ReplaceParameter(replaceableParameter, sourceParameter);
    }

    /// <summary>
    /// Создать предикат для <see cref="TSource"/> свойства коллекции.
    /// </summary>
    /// <param name="collectionProperty"> Выражение к свойству связанной коллекции.</param>
    /// <param name="collectionDelegate"> Операция над коллекцией.</param>
    /// <typeparam name="TItem"> Тип элемента коллекции.</typeparam>
    internal static Expression<Func<TSource, bool>> CreateCollectionPredicate<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var collectionMethods = new CollectionOptions<TSource, TItem>(collectionProperty);
        var collectionPredicate = collectionDelegate?.Invoke(collectionMethods) ?? (_ => true);
        return collectionPredicate;
    }

    public static implicit operator Expression<Func<TSource, bool>>(FilterBuilder<TSource> builder) => builder._predicate;
}