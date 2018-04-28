using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System.Linq.Expressions;

    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        private BinaryExpression binaryExpression;

        public BinaryExpressionEvaluator(Expression expression)
        {
            this.binaryExpression = expression as BinaryExpression;
        }

        public string Evaluate()
        {
            var result = string.Empty;

            if (binaryExpression.Left is BinaryExpression)
            {
                result += new BinaryExpressionEvaluator(binaryExpression.Left).Evaluate();                
            }
            if (binaryExpression.Left is MethodCallExpression)
            {
                result += new MethodCallExpressionEvaluator(binaryExpression.Left).Evaluate();
            }
            if (binaryExpression.Right is BinaryExpression)
            {
                result += new BinaryExpressionEvaluator(binaryExpression.Right).Evaluate();
            }
            if (binaryExpression.Right is MethodCallExpression)
            {
                result += new MethodCallExpressionEvaluator(binaryExpression.Right).Evaluate();
            }
            if (binaryExpression.Right is ConstantExpression && binaryExpression.Left is MemberExpression)
            {
                result += new ConstantExpressionEvaluator(binaryExpression).Evaluate();
            }
            
            return result;
        }
    }

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
            else if (value != null && (_binaryExpression.NodeType == ExpressionType.Equal))
            {
                if (value is string && !value.ToString().StartsWith("\""))
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
