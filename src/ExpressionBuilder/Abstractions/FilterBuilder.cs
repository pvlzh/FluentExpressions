using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Methods;

namespace ExpressionBuilder.Abstractions;

/// <summary>
/// Строитель выражения фильтрации.
/// </summary>
/// <typeparam name="TSource"> Тип объекта фильтрации.</typeparam>
public abstract class FilterBuilder<TSource>
{
    protected Expression<Func<TSource, bool>> PredicateExpression { get; set; }

    /// <inheritdoc cref="FilterBuilder{TSource}"/>
    /// <param name="startFiltration"> Начальное выражение фильтрации.</param>
    protected FilterBuilder(Expression<Func<TSource, bool>> startFiltration)
    {
        PredicateExpression = startFiltration;
    }

    #region Base
    public abstract FilterBuilder<TSource> And(
        Expression<Func<TSource, bool>> additionalPredicate);
    
    public abstract FilterBuilder<TSource> Or(
        Expression<Func<TSource, bool>> additionalPredicate);
    #endregion
    

    #region Collection
    public abstract FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionMethods<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate);
    
    public abstract FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Func<CollectionMethods<TSource, TItem>, Expression<Func<TSource, bool>>> collectionDelegate);
    #endregion
    

    #region String
    public abstract FilterBuilder<TSource> And(
        Expression<Func<TSource, string>> stringProperty, 
        Func<StringMethods<TSource>, Expression<Func<TSource, bool>>> stringDelegate);
    
    public abstract FilterBuilder<TSource> Or(
        Expression<Func<TSource, string>> stringProperty, 
        Func<StringMethods<TSource>, Expression<Func<TSource, bool>>> stringDelegate);
    #endregion
    

    public static implicit operator Expression<Func<TSource, bool>>(
        FilterBuilder<TSource> builder) => builder.PredicateExpression;
}