using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionBuilder.Extensions;

namespace ExpressionBuilder.Builders;

public class ConditionBuilder<TSource, TDestination>
{
    private readonly Stack<ConditionItem> _conditionStacks = new();
    private readonly ParameterExpression _sourceParameter;
    
    internal ConditionBuilder(Expression<Func<TSource, bool>> condition, TDestination then)
    {
        _sourceParameter = condition.Parameters[0];
        var thenExpression = Expression.Constant(then);
        _conditionStacks.Push(new (condition.Body, thenExpression));
    }
    
    public ConditionBuilder<TSource, TDestination> ElseIf(Expression<Func<TSource, bool>> condition, TDestination then)
    {
        var conditionExpression = condition.Body.ReplaceParameter(condition.Parameters[0], _sourceParameter);
        var thenExpression = Expression.Constant(then);
        _conditionStacks.Push(new (conditionExpression, thenExpression));
        return this;
    }
    
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

    private readonly struct ConditionItem
    {
        public Expression Condition { get; }
        public Expression Then { get; }

        public ConditionItem(Expression condition, Expression body)
        {
            Condition = condition;
            Then = body;
        }
    }
}