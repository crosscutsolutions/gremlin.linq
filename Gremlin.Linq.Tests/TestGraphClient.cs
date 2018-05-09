namespace Gremlin.Linq.Tests
{
    using System;
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

        public Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>>> SubmitDynamicAsync<T1, T2>(string gremlinExpression) where T1 : new() where T2 : new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>> SubmitDynamicAsync<T1, T2, T3>(string gremlinExpression) where T1 : new() where T2 : new() where T3 : new()
        {
            throw new NotImplementedException();
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