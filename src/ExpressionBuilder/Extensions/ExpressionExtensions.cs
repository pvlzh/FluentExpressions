using System;
using System.Linq.Expressions;
using ExpressionBuilder.Visitors;

namespace ExpressionBuilder.Extensions;

/// <summary>
/// Методы расширения для выражений.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Объединить два логический выражения оператором "И" (значение второго не будет вычислено если первое = false).
    /// </summary>
    public static Expression<Func<TSource, bool>> AndAlso<TSource>(
        this Expression<Func<TSource, bool>> sourceExpression, 
        Expression<Func<TSource, bool>> additionalExpression)
    {
        var sourceParameter = sourceExpression.Parameters[0]; 
        var replaceableParameter = additionalExpression.Parameters[0]; 
        
        var expression = additionalExpression.Body.ReplaceParameter(replaceableParameter, sourceParameter);
        var andExpression = Expression.AndAlso(sourceExpression.Body, expression);
        return Expression.Lambda<Func<TSource, bool>>(andExpression, sourceExpression.Parameters);
    }
    
    /// <summary>
    /// Объединить два логический выражения оператором "ИЛИ" (значение второго не будет вычислено если первое = true).
    /// </summary>
    public static Expression<Func<TSource, bool>> OrElse<TSource>(
        this Expression<Func<TSource, bool>> sourceExpression, 
        Expression<Func<TSource, bool>> additionalExpression)
    {
        var sourceParameter = sourceExpression.Parameters[0]; 
        var replaceableParameter = additionalExpression.Parameters[0]; 
        
        var expression = additionalExpression.Body.ReplaceParameter(replaceableParameter, sourceParameter);
        var orExpression = Expression.OrElse(sourceExpression.Body, expression);
        return Expression.Lambda<Func<TSource, bool>>(orExpression, sourceExpression.Parameters);
    }

    /// <summary>
    /// Получить выражение доступа к полю или свойству элемента.
    /// </summary>
    /// <param name="expression"> Лямбда к полю или свойству элемента.</param>
    /// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
    /// <typeparam name="TProperty"> Тип поля или свойства.</typeparam>
    /// <returns> <see cref="MemberExpression"/></returns>
    /// <exception cref="ArgumentException"> Лямбда выражение не представляет собой доступ к полю или свойству.</exception>
    public static MemberExpression GetMemberExpression<TSource, TProperty>(
        this Expression<Func<TSource, TProperty>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            throw new ArgumentException($"Expression '{expression}' is not represents " +
                                        $"accessing a field or property");
        }

        return memberExpression;
    }
    
    /// <summary>
    /// Получить выражение инициализации элемента.
    /// </summary>
    /// <param name="expression"> Лямбда инициализации элемента.</param>
    /// <typeparam name="TSource"> Тип исходного объекта.</typeparam>
    /// <typeparam name="TDestination"> Тип инициализируемого элемента.</typeparam>
    /// <returns> <see cref="MemberInitExpression"/></returns>
    /// <exception cref="ArgumentException"> Лямбда выражение не представляет собой инициализацию объекта.</exception>
    public static MemberInitExpression GetMemberInitExpression<TSource, TDestination>(
        this Expression<Func<TSource, TDestination>> expression)
    {
        if (expression.Body is not MemberInitExpression memberInitExpression)
        {
            throw new ArgumentException($"Expression '{expression}' is not represents initialization " +
                                        $"one or more members of the new object");
        }

        return memberInitExpression;
    }
    
    /// <summary>
    /// Заменить параметр в выражении.
    /// </summary>
    /// <param name="expressionTree"> Выражение в котором будет производиться замена.</param>
    /// <param name="replaceableParameter"> Параметр, который необходимо заменить.</param>
    /// <param name="replacement"> Выражение, используемое в качестве замены.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Expression ReplaceParameter(this Expression expressionTree, ParameterExpression replaceableParameter, Expression replacement)
    {
        var visitor = new ParameterReplacingVisitor(replaceableParameter, replacement);
        var result = visitor.Visit(expressionTree);
        if (result == null)
        {
            throw new Exception($"Failed to replace the parameter '{replaceableParameter}' " +
                                $"in the expression tree '{expressionTree}'");
        }

        return result;
    }
}