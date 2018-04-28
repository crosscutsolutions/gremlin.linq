using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    using System.Linq.Expressions;

    internal class MemberExpressionEvaluator : IExpressionEvaluator
    {
        private MemberExpression memberExpression;

        public MemberExpressionEvaluator(Expression expression)
        {
            this.memberExpression = expression as MemberExpression;
        }
        public string Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
