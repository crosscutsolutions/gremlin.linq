namespace Gremlin.Linq.Linq
{
    using System.Threading.Tasks;

    public static class CommandExtensions
    {
        public static async Task<QueryResult<TEntity>> SubmitAsync<TEntity>(this Command<TEntity> command)
            where TEntity : new()
        {
            var query = command.BuildGremlinQuery();
            var queryResult = await command.Client.SubmitWithSingleResultAsync<TEntity>(query);
            return queryResult;
        }

        public static async Task SubmitAsync(this Command command)
        {
            var query = command.BuildGremlinQuery();
            await command.Client.SubmitAsync(query);
        }
    }
}