using System;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Linq.Linq;
using Microsoft.Extensions.Configuration;

namespace Gremlin.Linq.TestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }

        private static async Task MainAsync(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", false)
                .Build();
            var settings = new GraphClientSettings(config);
            var client = new GremlinGraphClient(settings.Url, settings.Database, settings.Collection, settings.Password)
            {
                Logger = new GremlinLogger()
            };

            var users = await client.From<User>().SubmitAsync();
            Console.WriteLine(users.Count());

            var user = await client
                .Add(new User
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