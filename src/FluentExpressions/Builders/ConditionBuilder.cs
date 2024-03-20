using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentExpressions.Extensions;

namespace FluentExpressions.Builders;

/// <summary>
/// A fluent constructor of condition expressions.
/// </summary>
/// <typeparam name="TSource"> Type of input value.</typeparam>
/// <typeparam name="TDestination"> Type of case output value.</typeparam>
public class ConditionBuilder<TSource, TDestination>
{
    private readonly Stack<ConditionItem> _conditionStacks = new();
    private readonly ParameterExpression _sourceParameter;
    
    /// <summary>
    /// Create if condition expression.
    /// </summary>
    /// <param name="condition"> Condition expression.</param>
    /// <param name="then"> Value if condition is true.</param>
    /// <returns> This condition builder.</returns>
    internal ConditionBuilder(Expression<Func<TSource, bool>> condition, TDestination then)
    {
        _sourceParameter = condition.Parameters[0];
        var thenExpression = Expression.Constant(then);
        _conditionStacks.Push(new ConditionItem(condition.Body, thenExpression));
    }
    
    /// <summary>
    /// Add else if condition expression.
    /// </summary>
    /// <param name="condition"> Condition expression.</param>
    /// <param name="then"> Value if condition is true.</param>
    /// <returns> This condition builder.</returns>
    public ConditionBuilder<TSource, TDestination> ElseIf(Expression<Func<TSource, bool>> condition, TDestination then)
    {
        var conditionExpression = condition.Body.ReplaceParameter(condition.Parameters[0], _sourceParameter);
        var thenExpression = Expression.Constant(then);
        _conditionStacks.Push(new ConditionItem(conditionExpression, thenExpression));
        return this;
    }
    
    /// <summary>
    /// Else if condition expression.
    /// </summary>
    /// <param name="otherwise"> Value of otherwise.</param>
    /// <returns> Lambda of ternary expression.</returns>
    public Expression<Func<TSource, TDestination>> Else(TDestination otherwise)
    {
        ConditionalExpression? expression = null;
        while (_conditionStacks.Count > 0)
        {
            var conditionItem = _conditionStacks.Pop();
            
            if (expression == null)
            {
                var thenExpression = conditionItem.Then;
                var elseExpression = Expression.Constant(otherwise);
                
                expression = Expression.Condition(conditionItem.Condition, thenExpression, elseExpression);
                continue;
            }
            
            expression = Expression.Condition(conditionItem.Condition, conditionItem.Then, expression);
        }
        return Expression.Lambda<Func<TSource, TDestination>>(expression!, _sourceParameter);
    }

    /// <summary>
    /// Struct for stack item.
    /// </summary>
    private readonly struct ConditionItem
    {
        public Expression Condition { get; }
        public Expression Then { get; }

        public ConditionItem(Expression condition, Expression then)
        {
            Condition = condition;
            Then = then;
        }
    }
}