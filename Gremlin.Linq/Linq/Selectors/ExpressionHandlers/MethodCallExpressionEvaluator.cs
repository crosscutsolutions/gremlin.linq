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
            var memberName = (_expression.Object as MemberExpression)?.Member.GetPropertyName();
            var value = _expression.Arguments.First();
            return $".has('{memberName}', '{value.ToString().Replace("\"","")}')";
        }
    }
}