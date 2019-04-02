using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System.Linq.Expressions;

    internal class MemberExpressionEvaluator : IExpressionEvaluator
    {
        private readonly BinaryExpression _binaryExpression;

        public MemberExpressionEvaluator(Expression expression)
        {
            this._binaryExpression = expression as BinaryExpression;
        }
        public string Evaluate()
        {            
            var expression = Evaluator.PartialEval(_binaryExpression.Right as MemberExpression);
            var value = expression.ToString();        
            var label = (_binaryExpression.Left as MemberExpression)?.Member.GetGremlinPropertyName();
            return $".has('{label}', '{value.Replace("\"", "")}')";
        }
    }
}
