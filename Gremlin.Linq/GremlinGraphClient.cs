namespace Gremlin.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Net.Driver;
    using Net.Structure.IO.GraphSON;
    using Newtonsoft.Json;

    public class GremlinGraphClient : IGraphClient
    {
        private readonly GremlinClient _gremlinClient;

        public GremlinGraphClient(string url, object database, string collection, string password)
        {
            var gremlinServer = new GremlinServer(url, 443, true,
                "/dbs/" + database + "/colls/" + collection,
                password);

            _gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType);
        }

        public void Dispose()
        {
            _gremlinClient?.Dispose();
        }

        public async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(string gremlinExpression)
            where TEntity : new()
        {
            var result = await _gremlinClient.SubmitAsync<dynamic>(gremlinExpression);
            return MaterializeCollection<TEntity>(result);
        }

        public async Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(string gremlinExpression)
            where TEntity : new()
        {
            var result = await _gremlinClient.SubmitWithSingleResultAsync<dynamic>(gremlinExpression);
            return Materialize<TEntity>(result);
        }

        public async Task SubmitAsync(string gremlinExpression)
        {
            await _gremlinClient.SubmitAsync<dynamic>(gremlinExpression);
        }

        public async Task<long> SelectLong(string query)
        {
            var result = await _gremlinClient.SubmitAsync<dynamic>(query);
            return result.First();
        }

        private IEnumerable<QueryResult<TEntity>> MaterializeCollection<TEntity>(IReadOnlyCollection<object> obj)
            where TEntity : new()
        {
            foreach (var o in obj)
            {
                yield return Materialize<TEntity>(o as IDictionary<string, object>);
            }
        }

        private QueryResult<TEntity> Materialize<TEntity>(IDictionary<string, object> obj) where TEntity : new()
        {
            var resultEntity = new TEntity();
            var result = new QueryResult<TEntity>
            {
                Entity = resultEntity,
                Id = obj["id"].ToString(),
                Label = obj["label"].ToString()
            };
            if (resultEntity is Vertex vertex)
            {
                vertex.Id = result.Id;
            }
            var entityProperties = typeof(TEntity).GetProperties();
            var properties = (Dictionary<string, object>) obj["properties"];
            foreach (var propertyInfo in entityProperties)
            {
                if (!properties.TryGetValue(propertyInfo.Name, out dynamic value))
                {
                    continue;
                }

                var dict = (IDictionary<string, object>) ((IEnumerable<object>) value).ToList().First();
                if (!dict.TryGetValue("value", out var storedValue))
                {
                    continue;
                }

                if (propertyInfo.PropertyType.IsPrimitive)
                {
                    propertyInfo.GetSetMethod().Invoke(resultEntity,
                        new[] {Convert.ChangeType(storedValue.ToString(), propertyInfo.PropertyType)});
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.GetSetMethod().Invoke(resultEntity,
                        new[] {Convert.ChangeType(storedValue.ToString(), propertyInfo.PropertyType)});
                }
                else if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
                {
                    var o = JsonConvert.DeserializeObject(storedValue.ToString(), propertyInfo.PropertyType);
                    propertyInfo.GetSetMethod().Invoke(resultEntity,
                        new[] {o});
                }
            }

            return result;
        }
    }
}