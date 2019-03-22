using System.Reflection;

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

        public IGremlinLogger Logger { get; set; }

        public void Dispose()
        {
            _gremlinClient?.Dispose();
        }

        public async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(string gremlinExpression)
            where TEntity : new()
        {
            Logger?.Log(gremlinExpression);
            var result = await _gremlinClient.SubmitAsync<dynamic>(gremlinExpression);
            return MaterializeCollection<TEntity>(result);
        }

        public async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>>>
            SubmitDynamicAsync<T1, T2>(string gremlinExpression) where T1 : new() where T2 : new()
        {
            Logger?.Log(gremlinExpression);
            var result = await _gremlinClient.SubmitAsync<dynamic>(gremlinExpression);
            return MaterializeDynamicCollection<T1, T2>(result);
        }

        public async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>>
            SubmitDynamicAsync<T1, T2, T3>(string gremlinExpression) where T1 : new() where T2 : new() where T3 : new()
        {
            Logger?.Log(gremlinExpression);
            var result = await _gremlinClient.SubmitAsync<dynamic>(gremlinExpression);
            return MaterializeDynamicCollection<T1, T2, T3>(result);
        }

        public async Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(string gremlinExpression)
            where TEntity : new()
        {
            Logger?.Log(gremlinExpression);
            var result = await _gremlinClient.SubmitWithSingleResultAsync<dynamic>(gremlinExpression);
            if (result == null)
            {
                return null;
            }

            var materialized = Materialize<TEntity>(result);
            return materialized as QueryResult<TEntity>;
        }

        public async Task SubmitAsync(string gremlinExpression)
        {
            Logger?.Log(gremlinExpression);
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

        private IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>> MaterializeDynamicCollection<T1, T2>(
            IReadOnlyCollection<object> obj) where T1 : new() where T2 : new()
        {
            foreach (var o in obj)
            {
                yield return MaterializeDynamic<T1, T2>(o as IDictionary<string, object>);
            }
        }

        private IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>
            MaterializeDynamicCollection<T1, T2, T3>(IReadOnlyCollection<object> obj)
            where T1 : new() where T2 : new() where T3 : new()
        {
            foreach (var o in obj)
            {
                yield return MaterializeDynamic<T1, T2, T3>(o as IDictionary<string, object>);
            }
        }

        private Tuple<QueryResult<T1>, QueryResult<T2>> MaterializeDynamic<T1, T2>(IDictionary<string, object> obj)
            where T1 : new() where T2 : new()
        {
            var values = obj.Values.ToArray();
            var t1 = Materialize<T1>(values[0] as IDictionary<string, object>);
            var t2 = Materialize<T2>(values[1] as IDictionary<string, object>);
            var result = new Tuple<QueryResult<T1>, QueryResult<T2>>(t1, t2);
            return result;
        }

        private Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>
            MaterializeDynamic<T1, T2, T3>(IDictionary<string, object> obj)
            where T1 : new() where T2 : new() where T3 : new()
        {
            var values = obj.Values.ToArray();
            var t1 = Materialize<T1>(values[0] as IDictionary<string, object>);
            var t2 = Materialize<T2>(values[1] as IDictionary<string, object>);
            var t3 = Materialize<T3>(values[2] as IDictionary<string, object>);
            var result = new Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>(t1, t2, t3);
            return result;
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
            if (resultEntity is IGremlinEntity gremlinEntity)
            {
                gremlinEntity.Id = result.Id;
            }

            var entityProperties = typeof(TEntity).GetProperties();
            if (!obj.TryGetValue("properties", out var propertiesObj))
            {
                return result;
            };
            var properties = (IDictionary<string, object>) propertiesObj;
            foreach (var propertyInfo in entityProperties)
            {
                var propertyName = propertyInfo.Name;
                if (propertyInfo.GetCustomAttributes(typeof(GremlinPropertyAttribute)).SingleOrDefault() is
                    GremlinPropertyAttribute propertyAttribute) propertyName = propertyAttribute.Name;
                if (!properties.TryGetValue(propertyName, out dynamic value))
                {
                    continue;
                }

                object storedValue;
                if (value is IEnumerable<object> objects)
                {
                    var dict = (IDictionary<string, object>) objects.ToList().First();
                    if (!dict.TryGetValue("value", out storedValue))
                    {
                        continue;
                    }
                }
                else
                {
                    storedValue = value;
                }

                if (propertyInfo.PropertyType.IsEnum)
                {
                    if (storedValue is long l)
                    {
                        propertyInfo.GetSetMethod().Invoke(resultEntity,
                            new object[] {(int) l});
                    }
                    else if (storedValue is string s)
                    {
                        propertyInfo.GetSetMethod().Invoke(resultEntity,
                            new[] {Enum.Parse(propertyInfo.PropertyType, s)});
                    }
                }
                else if (propertyInfo.PropertyType.IsPrimitive)
                {
                    propertyInfo.GetSetMethod().Invoke(resultEntity,
                        new[] {Convert.ChangeType(storedValue.ToString(), propertyInfo.PropertyType)});
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    var datetimeStr = storedValue.ToString();
                    if (DateTime.TryParse(datetimeStr, out var datetime))
                    {
                        propertyInfo.GetSetMethod().Invoke(resultEntity,
                            new object[] {datetime});
                    }
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