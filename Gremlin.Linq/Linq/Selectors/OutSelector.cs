namespace Gremlin.Linq.Linq
{
    public class OutSelector<TEntity> : Selector<TEntity>, ICountable
    {
        private readonly string _edgeLabel;
        private string _alias;

        public OutSelector(IGraphClient graphClient, string edgeLabel) : base(graphClient)
        {
            _edgeLabel = edgeLabel;
        }

        public override string BuildGremlinQuery()
        {
            string result;
            var vertexLabel = typeof(TEntity).GetLabel();
            if (!string.IsNullOrEmpty(_edgeLabel))
                result = ParentSelector.BuildGremlinQuery() + $".out('{_edgeLabel}').has('label','{vertexLabel}')";
            else
                result = ParentSelector.BuildGremlinQuery() + $".out().has('label','{vertexLabel}')";
            if (!string.IsNullOrEmpty(_alias))
                result += $".as('{_alias}')";
            return result;
        }

        public OutSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }

        public OutSelector<TEntity> As<T>()
        {
            var vertexLabel = typeof(T).GetLabel();
            _alias = vertexLabel;
            return this;
        }
    }
}