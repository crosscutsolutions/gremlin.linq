namespace Gremlin.Linq.Linq
{
    using System;
    using System.Linq.Expressions;

    public class FromSelector<TEntity> : Selector<TEntity>, ICountable, IWhereSelector<TEntity>
    {
        private readonly IGraphClient _graphClient;

        public FromSelector(IGraphClient graphClient) : base(graphClient)
        {
            _graphClient = graphClient;
        }


        public override string BuildGremlinQuery()
        {
            var result = $"g.V().has('label','{typeof(TEntity).Name}')";
            return result;
        }

        public WhereSelector<TEntity> Where(Expression<Func<TEntity, bool>> func)
        {
            var whereSelector = new WhereSelector<TEntity>(_graphClient, func)
            {
                ParentSelector = this
            };
            return whereSelector;
        }
    }
}