namespace Gremlin.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGraphClient : IDisposable
    {
        Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(string gremlinExpression) where TEntity : new();
        Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>>> SubmitDynamicAsync<T1, T2>(string gremlinExpression) where T1 : new() where T2 : new();
        Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>> SubmitDynamicAsync<T1, T2, T3>(string gremlinExpression) where T1 : new() where T2 : new() where T3 : new();

        Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(string gremlinExpression) where TEntity : new();
        Task SubmitAsync(string gremlinExpression);
        Task<long> SelectLong(string query);
    }
}