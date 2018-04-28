using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System.Linq.Expressions;

    public class ExpressionEvaluator
    {
        private static IExpressionEvaluator GetEvaluator(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                return new BinaryExpressionEvaluator(expression);
            }
            else if (expression is MethodCallExpression)
            {
                return new MethodCallExpressionEvaluator(expression);
            }
            return null;
        }

        public static string Evaluate(Expression expression)
        {
            var evaluator = GetEvaluator(expression);
            return evaluator.Evaluate();
        }
    }
}
