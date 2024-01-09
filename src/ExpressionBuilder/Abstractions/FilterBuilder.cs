using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Abstractions;

/// <summary>
/// Строитель выражения фильтрации.
/// </summary>
/// <typeparam name="TSource"> Тип объекта фильтрации.</typeparam>
public abstract class FilterBuilder<TSource>
{
    protected Expression<Func<TSource, bool>> Expression { get; set; }

    /// <inheritdoc cref="FilterBuilder{TSource}"/>
    /// <param name="startFiltration"> Начальное выражение фильтрации.</param>
    protected FilterBuilder(Expression<Func<TSource, bool>> startFiltration)
    {
        Expression = startFiltration;
    }

    
    public abstract FilterBuilder<TSource> And(Expression<Func<TSource, bool>> additionalPredicate);
    
    public abstract FilterBuilder<TSource> Or(Expression<Func<TSource, bool>> additionalPredicate);
    

    public static implicit operator Expression<Func<TSource, bool>>(
        FilterBuilder<TSource> builder) => builder.Expression;
}