namespace Gremlin.Linq.Tests.Models
{
    public class GremlinPropertyTest : Vertex
    {
        [GremlinProperty("sp")] public string StringProperty { get; set; }
        [GremlinProperty("ip")] public int IntegerProperty { get; set; }
        [GremlinProperty("bp")] public bool BooleanProperty { get; set; }
    }
}