namespace Gremlin.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGraphClient : IDisposable
    {
        Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(string gremlinExpression) where TEntity : new();
        Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(string gremlinExpression) where TEntity : new();
        Task SubmitAsync(string gremlinExpression);
        Task<long> SelectLong(string query);
    }
    
}