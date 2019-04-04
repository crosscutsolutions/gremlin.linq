namespace Gremlin.Linq.Tests.Models
{
    [GremlinLabel("gl")] 
    public class GremlinLabelTest : Vertex
    {
        public string Property { get; set; }
    }
}