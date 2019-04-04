namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class MethodCallExpressionEvaluator : IExpressionEvaluator
    {
        private readonly MethodCallExpression _expression;

        public MethodCallExpressionEvaluator(Expression expression)
        {
            _expression = expression as MethodCallExpression;
        }
        public string Evaluate()
        {
            var member = ((MemberExpression) _expression.Object)?.Member;
            var propertyName = member.GetGremlinPropertyName();
            var value = _expression.Arguments.First();
            return $".has('{propertyName}', '{value.ToString().Replace("\"","")}')";
        }
    }
}