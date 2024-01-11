using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Collections;

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
    
    public abstract FilterBuilder<TSource> And(Expression<Func<TSource, bool>> additionalPredicate);
    
    public abstract FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate);
    
    public abstract FilterBuilder<TSource> And<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Action<CollectionPredicate<TItem>> collectionMethod);
    
    public abstract FilterBuilder<TSource> Or<TItem>(
        Expression<Func<TSource, IEnumerable<TItem>>> collectionProperty, 
        Action<CollectionPredicate<TItem>> collectionMethod);
    

    public static implicit operator Expression<Func<TSource, bool>>(
        FilterBuilder<TSource> builder) => builder.PredicateExpression;
}