namespace Gremlin.Linq.Linq
{
    using System;
    using System.Linq.Expressions;
    using Selectors.ExpressionHandlers;

    public class WhereSelector<T> : Selector<T>, ICountable, IWhereSelector<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public WhereSelector(IGraphClient graphClient, Expression<Func<T, bool>> expression) : base(graphClient)
        {
            _expression = expression;
        }

        public override string BuildGremlinQuery()
        {
            var value = ExpressionEvaluator.Evaluate(_expression.Body);            
            return ParentSelector.BuildGremlinQuery() + value;
        }
    }
}