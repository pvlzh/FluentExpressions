using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Expressions;
using ExpressionBuilder.Abstractions.Methods.Base;

namespace ExpressionBuilder.Abstractions.Methods;

/// <summary>
/// Методы предикатов для строки.
/// </summary>
public abstract class StringMethods<TSource> : PropertyMethods<TSource, string>
{
    protected StringMethods(MemberExpression<TSource, string> memberPath) 
        : base(memberPath)
    {
    }

    /// <summary>
    /// Начинается со значения.
    /// </summary>
    /// <param name="value"> Значение.</param>
    public abstract Expression<Func<TSource, bool>> StartWith(string value);

    /// <summary>
    /// Заканчивается значением.
    /// </summary>
    /// <param name="value"> Значение.</param>
    public abstract Expression<Func<TSource, bool>> EndWith(string value);

    /// <summary>
    /// Содержит значение.
    /// </summary>
    /// <param name="value"> Значение.</param>
    public abstract Expression<Func<TSource, bool>> Contains(string value);
}