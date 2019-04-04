using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System.Linq;
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

            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess &&
                binaryExpression.Right.NodeType == ExpressionType.Call)
            {
                //
                result += new CallExpressionEvaluator(binaryExpression).Evaluate();
            }
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
            if ((binaryExpression.Left.NodeType != ExpressionType.MemberAccess && binaryExpression.Right is MethodCallExpression))
            {
                result += new MethodCallExpressionEvaluator(binaryExpression.Right).Evaluate();
            }
            if (binaryExpression.Right is ConstantExpression && binaryExpression.Left is MemberExpression)
            {
                result += new ConstantExpressionEvaluator(binaryExpression).Evaluate();
            }
            if (binaryExpression.Right is MemberExpression && binaryExpression.Left is MemberExpression)
            {
                result += new MemberExpressionEvaluator(binaryExpression).Evaluate();
            }

            return result;
        }
    }

    public class CallExpressionEvaluator : IExpressionEvaluator
    {
        private readonly BinaryExpression _binaryExpression;

        public CallExpressionEvaluator(BinaryExpression binaryExpression)
        {
            _binaryExpression = binaryExpression;
        }

        public string Evaluate()
        {
            var callExpression = (MethodCallExpression) _binaryExpression.Right;
            var values = callExpression.Arguments.Select(a=>(Evaluator.PartialEval(a) as ConstantExpression).Value).ToArray();
            var value = callExpression.Method.Invoke(null, values);
            var label = (_binaryExpression.Left as MemberExpression)?.Member.GetType().GetLabel();
            return $".has('{label}', '{value}')";
        }
    }
}
