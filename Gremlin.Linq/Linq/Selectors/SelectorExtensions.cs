namespace Gremlin.Linq.Linq
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class SelectorExtensions
    {
        public static async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(this Selector selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitAsync<TEntity>(query);
            return queryResult;
        }

        public static async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(
            this Selector<TEntity> selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitAsync<TEntity>(query);
            return queryResult;
        }

        public static async Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(
            this Selector<TEntity> selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitWithSingleResultAsync<TEntity>(query);
            return queryResult;
        }
    }
}