namespace Gremlin.Linq.Entities
{
    using System.Collections.Generic;

    public class GremlinPropertyTest : Vertex
    {
        [GremlinProperty("gp")]
        public string Property { get; set; }
    }
}