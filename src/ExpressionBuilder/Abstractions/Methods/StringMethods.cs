using System;
using System.Linq.Expressions;
using ExpressionBuilder.Abstractions.Methods.Base;

namespace ExpressionBuilder.Abstractions.Methods;

/// <summary>
/// Операций над полем или свойством типа <see cref="string"/> объекта <see cref="TSource"/>.
/// </summary>
/// <typeparam name="TSource"> Тип исходного элемента.</typeparam>
public abstract class StringMethods<TSource>
    : PropertyMethods<TSource, string>
{
    protected StringMethods(Expression<Func<TSource, string>> memberExpression) 
        : base(memberExpression)
    {
    }

    /// <summary>
    /// Значение поля или свойства с типом <see cref="string"/> начинается со значения <see cref="value"/>.
    /// </summary>
    /// <param name="value"> Проверяемое значение.</param>
    public abstract Expression<Func<TSource, bool>> StartWith(string value);

    /// <summary>
    /// Значение поля или свойства с типом <see cref="string"/> заканчивается значением <see cref="value"/>.
    /// </summary>
    /// <param name="value"> Проверяемое значение.</param>
    public abstract Expression<Func<TSource, bool>> EndWith(string value);

    /// <summary>
    /// Значение поля или свойства с типом <see cref="string"/> содержит <see cref="value"/>.
    /// </summary>
    /// <param name="value"> Проверяемое значение.</param>
    public abstract Expression<Func<TSource, bool>> Contains(string value);
}