using System.Linq.Expressions;

namespace FluentExpressions.Visitors;

/// <summary>
/// Заменяет одно выражение другим в заданном дереве выражений.
/// </summary>
internal class ParameterReplacingVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _replaceableParameter;
    private readonly Expression _replacement;

    /// <inheritdoc cref="ParameterReplacingVisitor"/>
    /// <param name="replaceableParameter"> Параметр, который необходимо заменить.</param>
    /// <param name="replacement"> Выражение, используемое в качестве замены.</param>
    public ParameterReplacingVisitor(ParameterExpression replaceableParameter, Expression replacement)
    {
        _replaceableParameter = replaceableParameter;
        _replacement = replacement;
    }
    
    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _replaceableParameter ? _replacement 
            : base.VisitParameter(node);
    }
}