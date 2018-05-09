namespace Gremlin.Linq.Linq
{
    public class OutEdgeSelector<TEntity> : EdgeSelector
    {
        private string _alias;

        public OutEdgeSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public OutEdgeSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }
        public OutEdgeSelector<TEntity> As<T>()
        {
            _alias = typeof(T).GetLabel();
            return this;
        }



        public override string BuildGremlinQuery()
        {
            var result = ParentSelector.BuildGremlinQuery() + $".outE().has('label','{typeof(TEntity).GetLabel()}')";
            if (!string.IsNullOrEmpty(_alias))
            {
                result = result + $".as('{_alias}')";
            }
            return result;
        }
    }
}