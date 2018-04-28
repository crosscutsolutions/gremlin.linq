namespace Gremlin.Linq.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class WhereInSelector<T> : Selector<T>, ICountable, IWhereSelector<T>
    {
        private readonly Expression<Func<T, object>> _expression;
        private readonly IEnumerable<object> _values;

        public WhereInSelector(IGraphClient graphClient, Expression<Func<T, object>> expression,
            IEnumerable<object> values) : base(graphClient)
        {
            _expression = expression;
            _values = values;
        }

        public override string BuildGremlinQuery()
        {
            var exp = _expression;
            if (exp == null)
            {
                throw new ArgumentException($"Expression is not valid - {_expression}");
            }

            var prop = ((MemberExpression) exp.Body).Member.Name;

            return ParentSelector.BuildGremlinQuery() +
                   $".has('{prop}',within({_values.Aggregate(string.Empty, (acc, a) => acc + "'" + a + "',", a => a.Substring(0, a.Length - 1))}))";
        }
    }
}