using System;
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

            await client
                .Add(new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 31
                })
                .SubmitAsync();

            var users = await client.From<User>().SubmitAsync();
            foreach (var user in users)
                Console.WriteLine($"{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");
        }
    }

    [GremlinLabel("u")]
    public class User : Vertex
    {
        [GremlinProperty("first-name")] public string FirstName { get; set; }

        [GremlinProperty("last-name")] public string LastName { get; set; }
        public int Age { get; set; }
    }
}