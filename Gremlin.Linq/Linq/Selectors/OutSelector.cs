namespace Gremlin.Linq.Linq
{
    public class OutSelector<TEntity> : Selector<TEntity>, ICountable
    {
        private string _alias;

        public OutSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override string BuildGremlinQuery()
        {
            var label = typeof(TEntity).GetLabel();
            var result = ParentSelector.BuildGremlinQuery() + $".out().has('label','{label}')";
            if (!string.IsNullOrEmpty(_alias)) result = result + $".as('{_alias}')";
            return result;
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
    }
}