namespace Gremlin.Linq.Linq
{
    public class OutSelector<TEntity> : Selector<TEntity>, ICountable
    {
        private string _alias;

        public OutSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public OutSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }
        public OutSelector<TEntity> As<T>()
        {
            _alias = typeof(T).Name;
            return this;
        }

        public override string BuildGremlinQuery()
        {
            var result= ParentSelector.BuildGremlinQuery() + $".out().has('label','{typeof(TEntity).Name}')";
            if (!string.IsNullOrEmpty(_alias))
            {
                result = result + $".as('{_alias}')";
            }
            return result;
        }
    }
}