namespace Gremlin.Linq.Linq
{
    using System.Threading.Tasks;

    public interface ICountable : IGremlinQueryable
    {
    }

    public interface IGremlinQueryable
    {
        IGraphClient Client { get; }
        string BuildGremlinQuery();
    }

    public static class CountableExtension
    {
        public static CountCommand Count(this ICountable countable)
        {
            var countCommand = new CountCommand(countable.Client) {ParentSelector = countable};
            return countCommand;
        }
    }

    public class CountCommand : Command
    {
        public CountCommand(IGraphClient client) : base(client)
        {
        }

        public IGremlinQueryable ParentSelector { get; set; }

        public override string BuildGremlinQuery()
        {
            return ParentSelector.BuildGremlinQuery() + ".count()";
        }

        public async Task<long> ExecuteAsync()
        {
            var query = BuildGremlinQuery();
            var result = await Client.SelectLong(query);
            return result;
        }
    }
}