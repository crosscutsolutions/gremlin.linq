namespace Gremlin.Linq.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TestGraphClient : IGraphClient
    {

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(string gremlinExpression) where TEntity : new()
        {
            throw new System.NotImplementedException();
        }

        public Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(string gremlinExpression) where TEntity : new()
        {
            throw new System.NotImplementedException();
        }

        public Task SubmitAsync(string gremlinExpression)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> SelectLong(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}