namespace Gremlin.Linq
{
    using Microsoft.Extensions.Configuration;

    public class GraphClientSettings
    {
        public GraphClientSettings(IConfiguration configuration)
        {
            configuration.Bind("gremlin", this);    
        }

        public string Collection { get; set; }
        public object Database { get; set; }
        public string Url { get; set; }
        public string Password { get; set; }
    }
}