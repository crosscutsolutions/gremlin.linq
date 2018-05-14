namespace Gremlin.Linq
{
    using Newtonsoft.Json;

    public class QueryResult
    {
    }

    public class QueryResult<TEntity> : QueryResult
    {
        public string Id { get; set; }
        public string Label { get; set; }

        [JsonProperty("properties")] public TEntity Entity { get; set; }
        
    }
}