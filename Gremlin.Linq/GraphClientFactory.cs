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

        public IGraphClient CreateGremlinGraphClient(GraphClientSettings settings)
        {
            return new GremlinGraphClient(settings.Url, settings.Database, settings.Collection,settings.Password);
        }

        public IGraphClient CreateGremlinGraphClient()
        {
            var settings = new GraphClientSettings(_configuration);
            return new GremlinGraphClient(settings.Url, settings.Database, settings.Collection,settings.Password);
        }
    }
}