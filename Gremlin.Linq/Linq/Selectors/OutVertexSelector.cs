namespace Gremlin.Linq.Linq
{
    public class OutVertexSelector<TEntity> : Selector<TEntity>
    {
        private string _alias;

        public OutVertexSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public OutVertexSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }
        public OutVertexSelector<TEntity> As<T>()
        {
            _alias = typeof(T).GetLabel();
            return this;
        }

        public override string BuildGremlinQuery()
        {
            var result = ParentSelector.BuildGremlinQuery() + $".outV().has('label','{typeof(TEntity).GetLabel()}')";
            if (!string.IsNullOrEmpty(_alias))
            {
                result = result + $".as('{_alias}')";
            }
            return result;
        }
    }
}