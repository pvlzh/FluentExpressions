using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Builders;
using FluentExpressions.Methods;

namespace FluentExpressions;

public static class ExpressionFor<TSource>
{
    /// <summary>
    /// Задать правило фильтрации для последовательности <see cref="TSource"/>
    /// </summary>
    /// <param name="startExpression"> Начальное правило фильтрации.</param>
    /// <returns> Строитель выражения фильтрации.</returns>
    public static FilterBuilder<TSource> Where(Expression<Func<TSource, bool>> startExpression)
    {
        return new FilterBuilder<TSource>(startExpression);
    }

    /// <summary>
    /// Задать правило фильтрации для последовательности <see cref="TSource"/>
    /// </summary>
    /// <param name="collectionProperty"> Выражение к свойству связанной коллекции.</param>
    /// <param name="collectionDelegate"> Операция над коллекцией.</param>
    /// <typeparam name="TItem"> Тип элемента коллекции.</typeparam>
    /// <returns> Строитель выражения фильтрации.</returns>
    public static FilterBuilder<TSource> Where<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionOptions<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate)
    {
        var startExpression = FilterBuilder<TSource>.CreateCollectionPredicate(collectionProperty, collectionDelegate);
        return Where(startExpression);
    }
    
    /* todo: review the logic of projections
    /// <summary>
    /// Задать правило проекции из <see cref="TSource"/> в <see cref="TDestination"/>
    /// </summary>
    /// <param name="selectExpression"> Выражение проекции.</param>
    /// <typeparam name="TDestination"> Тип результата.</typeparam>
    /// <returns> Строитель выражения проекции.</returns>
    public static ProjectionBuilder<TSource, TDestination> Select<TDestination>(
        Expression<Func<TSource, TDestination>> selectExpression)
    {
        return new ProjectionBuilder<TSource, TDestination>(selectExpression);
    }
    */
    
    /// <summary>
    /// If-Else statement expression.
    /// </summary>
    /// <param name="condition"> The initial condition.</param>
    /// <param name="then"> The body of the expression for the positive result of the condition.</param>
    /// <returns> <see cref="ConditionBuilder{TSource, TDestination}"/>.</returns>
    public static ConditionBuilder<TSource, TDestination> If<TDestination>(
        Expression<Func<TSource, bool>> condition, TDestination then)
    {
        return new ConditionBuilder<TSource, TDestination>(condition, then);
    }
}