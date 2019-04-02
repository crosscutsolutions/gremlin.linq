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
        }

        private static async Task MainAsync(string[] args)
        {
            using (var client = CreateGraphClient())
            {
                // await CreateSampleData(client);

                await PerformTests(client);

                // await CleanupSampleData(client);
            }
        }

        private static async Task CleanupSampleData(GremlinGraphClient client)
        {
            Console.WriteLine("Cleanup old data?");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                await client.From<Person>().Drop().SubmitAsync();
                await client.From<Skill>().Drop().SubmitAsync();
                Console.WriteLine("Cleaned up.");
            }
            else
            {
                Console.WriteLine("NOT cleaned up.");
            }
        }

        private static async Task PerformTests(GremlinGraphClient client)
        {
            Console.WriteLine("** All people:  ");
            var allPeople = await client
                .From<Person>()
                .SubmitAsync();
            foreach (var user in allPeople)
                Console.WriteLine($"\t{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");

            Console.WriteLine("** The 30 and under club:  ");
            var youngsters = await client
                .From<Person>()
                .Where(u => u.Age <= 30)
                .SubmitAsync();
            foreach (var user in youngsters)
                Console.WriteLine($"\t{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");

            Console.WriteLine("** People that love Eve:  ");
            var eveLovers = await client
                .From<Person>()
                .Where(u => u.FirstName == "Eve")
                .In<Person>("loves")
                .SubmitAsync();
            foreach (var user in eveLovers)
                Console.WriteLine($"\t{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");

            Console.WriteLine("** People that Tim hates:  ");
            var hatedByTim = await client
                .From<Person>()
                .Where(u => u.FirstName == "Tim")
                .Out<Person>("hates")
                .SubmitAsync();
            foreach (var user in hatedByTim)
                Console.WriteLine($"\t{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");

            Console.WriteLine("** People that Bob connects to:  ");
            var relatedToTim = await client
                .From<Person>()
                .Where(u => u.FirstName == "Bob")
                .Out<Person>()
                .SubmitAsync();
            foreach (var user in relatedToTim)
                Console.WriteLine($"\t{user.Entity.FirstName} {user.Entity.LastName}, Age = {user.Entity.Age}");

            Console.WriteLine("** Ned's skills:  ");
            var hasManySkills = await client
                .From<Person>()
                .Where(u => u.FirstName == "Ned")
                .Out<Skill>("can")
                .SubmitAsync();
            foreach (var user in hasManySkills)
                Console.WriteLine($"\t{user.Entity.Name}");
        }

        private static async Task CreateSampleData(GremlinGraphClient client)
        {
            Console.Write("Adding people...");
            var bob = await client.Add(new Person {FirstName = "Bob", LastName = "Oak", Age = 33}).SubmitAsync();
            var eve = await client.Add(new Person {FirstName = "Eve", LastName = "Maple", Age = 29}).SubmitAsync();
            var tim = await client.Add(new Person {FirstName = "Tim", LastName = "Spruce", Age = 26}).SubmitAsync();
            var ned = await client.Add(new Person {FirstName = "Ned", LastName = "Hickory", Age = 37}).SubmitAsync();
            Console.WriteLine("Done.");

            Console.Write("Adding skills...");
            var cook = await client.Add(new Skill {Name = "Cooking"}).SubmitAsync();
            var juggle = await client.Add(new Skill {Name = "Juggle"}).SubmitAsync();
            var sew = await client.Add(new Skill {Name = "Sew"}).SubmitAsync();
            var changeLightbulb = await client.Add(new Skill {Name = "Change lightbulb"}).SubmitAsync();
            var skateboard = await client.Add(new Skill {Name = "Skateboard"}).SubmitAsync();
            Console.WriteLine("Done.");

            Console.Write("Adding relationships...");
            await client.ConnectVerticies(bob, eve, "loves").SubmitAsync();
            await client.ConnectVerticies(bob, tim, "likes").SubmitAsync();
            await client.ConnectVerticies(eve, bob, "likes").SubmitAsync();
            await client.ConnectVerticies(tim, eve, "hates").SubmitAsync();
            await client.ConnectVerticies(ned, tim, "loves").SubmitAsync();
            await client.ConnectVerticies(ned, eve, "loves").SubmitAsync();

            await client.ConnectVerticies(bob, sew, "can").SubmitAsync();
            await client.ConnectVerticies(bob, changeLightbulb, "can").SubmitAsync();
            await client.ConnectVerticies(eve, juggle, "can").SubmitAsync();
            await client.ConnectVerticies(eve, changeLightbulb, "can").SubmitAsync();
            await client.ConnectVerticies(tim, cook, "can").SubmitAsync();
            await client.ConnectVerticies(ned, sew, "can").SubmitAsync();
            await client.ConnectVerticies(ned, changeLightbulb, "can").SubmitAsync();
            await client.ConnectVerticies(ned, juggle, "can").SubmitAsync();
            await client.ConnectVerticies(ned, skateboard, "can").SubmitAsync();
            Console.WriteLine("Done.");
        }

        private static GremlinGraphClient CreateGraphClient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", false)
                .Build();
            var settings = new GraphClientSettings(config);
            var client =
                new GremlinGraphClient(settings.Url, settings.Database, settings.Collection, settings.Password);
            return client;
        }
    }

    [GremlinLabel("person")]
    public class Person : Vertex
    {
        [GremlinProperty("given-name")] public string FirstName { get; set; }
        [GremlinProperty("surname")] public string LastName { get; set; }
        public int Age { get; set; }
    }

    [GremlinLabel("skill")]
    public class Skill : Vertex
    {
        [GremlinProperty("skill-name")] public string Name { get; set; }
    }
}