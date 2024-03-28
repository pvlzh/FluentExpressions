using System.Linq.Expressions;

namespace FluentExpressions.Visitors;

/// <summary>
/// Replaces one expression with another in the specified expression tree.
/// </summary>
internal class ParameterReplacingVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _replaceableParameter;
    private readonly Expression _replacement;

    /// <inheritdoc cref="ParameterReplacingVisitor"/>
    /// <param name="replaceableParameter"> The parameter that needs to be replaced.</param>
    /// <param name="replacement"> The expression used as a substitute.</param>
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