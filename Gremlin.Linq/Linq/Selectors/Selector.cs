namespace Gremlin.Linq.Linq
{
    public abstract class Selector<TEntity> : Selector
    {
        protected Selector(IGraphClient graphClient) : base(graphClient)
        {
        }
    }

    public abstract class Selector : IGremlinQueryable
    {
        protected Selector(IGraphClient graphClient)
        {
            Client = graphClient;
        }

        public IGremlinQueryable ParentSelector { get; protected internal set; }

        public IGraphClient Client { get; }

        public virtual string BuildGremlinQuery()
        {
            return string.Empty;
        }

        public FromSelector<TEntity> From<TEntity>()
        {
            return new FromSelector<TEntity>(Client);
        }
    }
}