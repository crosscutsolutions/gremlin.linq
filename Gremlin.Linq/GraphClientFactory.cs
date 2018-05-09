namespace Gremlin.Linq
{
    using Microsoft.Extensions.Configuration;

    public class GraphClientFactory
    {
        private readonly IConfiguration _configuration;

        public GraphClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IGremlinLogger Logger { get; set; }

        public IGraphClient CreateGremlinGraphClient(GraphClientSettings settings)
        {
            var client =
                new GremlinGraphClient(settings.Url, settings.Database, settings.Collection, settings.Password)
                {
                    Logger = Logger
                };
            return client;
        }

        public IGraphClient CreateGremlinGraphClient()
        {
            var settings = new GraphClientSettings(_configuration);
            var gremlinGraphClient =
                new GremlinGraphClient(settings.Url, settings.Database, settings.Collection, settings.Password)
                {
                    Logger = Logger
                };
            return gremlinGraphClient;
        }
    }
}