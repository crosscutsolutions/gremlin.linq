using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq.Linq.Selectors
{
    using System.Linq;

    public class SelectSelector : Selector
    {
        protected readonly string[] Selectors;

        public SelectSelector(IGraphClient graphClient, params string[] selectors) : base(graphClient)
        {
            Selectors = selectors;
        }

        public override string BuildGremlinQuery()
        {
            var select = $".select(" + GetSelectors() + ")";
            return ParentSelector.BuildGremlinQuery() + select;
        }

        public string GetSelectors()
        {
            return string.Join(",", Selectors.Select(a => $"\'{a}\'").ToArray());
        }
    }

    public class SelectSelector<T1> : SelectSelector
    {
        public SelectSelector(IGraphClient graphClient) : base(graphClient, typeof(T1).GetLabel())
        {
        }
    }
    public class SelectSelector<T1,T2> : SelectSelector
    {
        public SelectSelector(IGraphClient graphClient) : base(graphClient, typeof(T1).GetLabel(), typeof(T2).GetLabel())
        {
        }
    }
    public class SelectSelector<T1,T2, T3> : SelectSelector
    {
        public SelectSelector(IGraphClient graphClient) : base(graphClient, typeof(T1).GetLabel(), typeof(T2).GetLabel(), typeof(T3).GetLabel())
        {
        }
    }

}