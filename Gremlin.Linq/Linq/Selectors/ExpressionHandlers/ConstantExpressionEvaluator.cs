using System;
using System.Linq.Expressions;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    public class ConstantExpressionEvaluator : IExpressionEvaluator
    {
        private readonly BinaryExpression _binaryExpression;

        public ConstantExpressionEvaluator(Expression expression)
        {
            _binaryExpression = (BinaryExpression) expression;
        }

        public string Evaluate()
        {
            var memberValue = _binaryExpression.Right as MemberExpression;
            var expression = Evaluator.PartialEval(memberValue);
            var value = expression != null
                ? expression.ToString()
                : (_binaryExpression.Right as ConstantExpression)?.Value;
            if (_binaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                value = $"gt({value})";
            }
            else if (_binaryExpression.NodeType == ExpressionType.LessThan)
            {
                value = $"lt({value})";
            }
            else if (value != null && _binaryExpression.NodeType == ExpressionType.Equal)
            {
               if (value is bool)
                {
                    value = $"{value.ToString().ToLower()}";
                }
                else if (value is string && !value.ToString().StartsWith("\""))
                {
                    value = $"'{value}'";
                }
                else
                {
                    value = $"{value}";
                }
            }
            else
            {
                throw new ArgumentException($"Unsupported nodetype {_binaryExpression.NodeType}");
            }

            return $".has('{((MemberExpression) _binaryExpression.Left).Member.Name}', {value})";
        }
    }
}