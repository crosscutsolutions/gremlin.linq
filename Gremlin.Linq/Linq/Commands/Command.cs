namespace Gremlin.Linq.Linq
{
    public abstract class Command : IGremlinQueryable
    {
        protected internal Command(IGraphClient client)
        {
            Client = client;
        }

        public IGraphClient Client { get; }

        public abstract string BuildGremlinQuery();
    }

    public abstract class Command<TEntity> : Command
    {
        protected Command(IGraphClient client) : base(client)
        {
        }
    }
}