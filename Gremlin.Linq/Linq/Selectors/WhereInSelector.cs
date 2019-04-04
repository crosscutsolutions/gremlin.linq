using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gremlin.Linq.Linq.Selectors.ExpressionHandlers;

namespace Gremlin.Linq.Linq
{
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
            if (exp == null) throw new ArgumentException($"Expression is not valid - {_expression}");
            
            var member = ((MemberExpression) exp.Body).Member;
            var propertyName = member.GetGremlinPropertyName();

            return ParentSelector.BuildGremlinQuery() +
                   $".has('{propertyName}',within({_values.Aggregate(string.Empty, (acc, a) => acc + "'" + a + "',", a => a.Substring(0, a.Length - 1))}))";
        }
    }
}