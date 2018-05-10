using System;

namespace Gremlin.Linq.TestApp
{
    using Linq;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        static async Task MainAsync(string[] args) { 
        var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var settings = new GraphClientSettings(config);
            var client = new GremlinGraphClient(settings.Url,settings.Database,settings.Collection,settings.Password)
                {
                    Logger = new GremlinLogger()
                }; 

            var users = await client.From<User>().SubmitAsync();
            Console.WriteLine(users.Count());

            var user = await client
                .Add(new User()
                {
                    Name = "John Doe"
                })
                .SubmitAsync();

            users = await client.From<User>().SubmitAsync();
            Console.WriteLine(users.Count());
        }
    }

    public class User : Vertex
    {
        public string Name { get; set; }        
    }
}
