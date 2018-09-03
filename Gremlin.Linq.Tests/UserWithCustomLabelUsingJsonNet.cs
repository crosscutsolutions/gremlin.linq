using Newtonsoft.Json;

namespace Gremlin.Linq.Entities
{
    [JsonObject("JsonNetUser")]
    public class UserWithCustomLabelUsingJsonNet : Vertex
    {
        public string Name { get; set; }
    }
}