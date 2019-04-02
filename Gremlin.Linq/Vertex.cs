using System;

namespace Gremlin.Linq
{
    public class Vertex : IGremlinEntity, IComparable<Vertex>
    {
        public int CompareTo(Vertex other)
        {
            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }

        public string Id { get; set; }
    }
}