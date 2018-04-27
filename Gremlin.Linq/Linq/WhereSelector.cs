namespace Gremlin.Linq.Linq
{
    using System;
    using System.Linq.Expressions;

    public class WhereSelector<T> : Selector<T>, ICountable, IWhereSelector<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public WhereSelector(IGraphClient graphClient, Expression<Func<T, bool>> expression) : base(graphClient)
        {
            _expression = expression;
        }

        public override string BuildGremlinQuery()
        {
            var binaryExpression = _expression.Body as BinaryExpression;
            if (binaryExpression == null)
            {
                throw new ArgumentException($"Expression is not valid - {_expression}");
            }

            var memberName = binaryExpression.Left as MemberExpression;
            var memberValue = binaryExpression.Right as MemberExpression;
            var expression = Evaluator.PartialEval(memberValue);
            var value = expression != null
                ? expression.ToString()
                : (binaryExpression.Right as ConstantExpression)?.Value;
            if (binaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                value = $"gt({value})";
            }
            else if (binaryExpression.NodeType == ExpressionType.LessThan)
            {
                value = $"lt({value})";
            }
            else if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                if (value.GetType() == typeof(string) && !value.ToString().StartsWith("\""))
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
                throw new ArgumentException($"Unsupported nodetype {binaryExpression.NodeType}");
            }

            return ParentSelector.BuildGremlinQuery() + $".has('{memberName.Member.Name}', {value})";
        }
    }
}