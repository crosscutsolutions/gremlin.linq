namespace Gremlin.Linq.Linq
{
    public class Selector<TEntity> : Selector
    {
        public Selector(IGraphClient graphClient) : base(graphClient)
        {
        }
    }

    public class Selector : IGremlinQueryable
    {
        public Selector(IGraphClient graphClient)
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