namespace Gremlin.Linq.Linq
{
    public class InVertexSelector<TEntity> : Selector<TEntity>
    {
        private string _alias;

        public InVertexSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public InVertexSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }

        public InVertexSelector<TEntity> As<T>()
        {
            _alias = typeof(T).GetLabel();
            return this;
        }

        public override string BuildGremlinQuery()
        {
            var label = typeof(TEntity).GetLabel();
            var result = ParentSelector.BuildGremlinQuery() + $".inV().has('label','{label}')";
            if (!string.IsNullOrEmpty(_alias)) result = result + $".as('{_alias}')";
            return result;
        }
    }
}