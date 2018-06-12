namespace Gremlin.Linq.Linq
{
    using Net.Process.Traversal;
    using Selectors;

    public static class GraphClientExtensions
    {
        public static InsertCommand<TEntity> Add<TEntity>(this IGraphClient client, TEntity entity)
        {
            var command = new InsertCommand<TEntity>(client, entity);
            return command;
        }
        public static UpdateCommand<TEntity> UpdateWith<TEntity>(this IWhereSelector<TEntity> selector, TEntity entity)
        {
            var command = new UpdateCommand<TEntity>(selector.Client, entity)
            {
                ParentSelector = selector
            };
            return command;
        }

        public static FromSelector<TEntity> From<TEntity>(this IGraphClient client)
        {
            var selector = new FromSelector<TEntity>(client);
            return selector;
        }


        public static WhereAnySelector Where(this IGraphClient client, string hasField, object value)
        {
            var fromSelector = new FromAnySelector(client);
            var whereSelector = new WhereAnySelector(client, hasField, value) {ParentSelector = fromSelector};
            return whereSelector;
        }


        public static AddEdgeCommand ConnectVerticies<TFromEntity, TToEntity>(this IGraphClient client,
            QueryResult<TFromEntity> fromEntity, TToEntity toEntity, string relation)
        {
            var fromSelector = new FromSelector<TFromEntity>(client);
            var whereSelector = new WhereAnySelector(client, "id", fromEntity.Id) {ParentSelector = fromSelector};
            var addCommand = new AddEdgeCommand(client, toEntity, relation)
            {
                ParentSelector = whereSelector,
                InsertCommand = new InsertCommand(client, toEntity)
            };
            return addCommand;
        }

        public static AddEdgeCommand ConnectVerticies<TFromEntity, TToEntity>(this IGraphClient client,
            QueryResult<TFromEntity> fromEntity, QueryResult<TToEntity> toEntity, string relation)
        {
            var fromSelector = new FromSelector<TFromEntity>(client);
            var whereSelector = new WhereAnySelector(client, "id", fromEntity.Id) {ParentSelector = fromSelector};
            var innerFromSelector = new FromSelector<TToEntity>(client);
            var innerWhereSelector =
                new WhereAnySelector(client, "id", toEntity.Id) {ParentSelector = innerFromSelector};
            var addCommand = new AddEdgeCommand(client, toEntity, relation)
            {
                ParentSelector = whereSelector,
                Selector = innerWhereSelector
            };
            return addCommand;
        }
    }
}