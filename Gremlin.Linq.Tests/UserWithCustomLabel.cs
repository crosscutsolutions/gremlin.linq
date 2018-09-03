namespace Gremlin.Linq.Entities
{
    [GremlinLabel("CustomUser")]
    public class UserWithCustomLabel : Vertex
    {
        public string Name { get; set; }
    }
}