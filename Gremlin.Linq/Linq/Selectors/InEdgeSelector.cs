namespace Gremlin.Linq.Linq
{
    public class InEdgeSelector<TEntity> : EdgeSelector
    {
        private string _alias;

        public InEdgeSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public InEdgeSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }
        public InEdgeSelector<TEntity> As<T>()
        {
            _alias = typeof(T).Name;
            return this;
        }



        public override string BuildGremlinQuery()
        {
            var result = ParentSelector.BuildGremlinQuery() + $".inE().has('label','{typeof(TEntity).Name}')";
            if (!string.IsNullOrEmpty(_alias))
            {
                result = result + $".as('{_alias}')";
            }
            return result;
        }
    }

}