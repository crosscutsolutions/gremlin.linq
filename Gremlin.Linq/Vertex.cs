namespace Gremlin.Linq
{
    using System;
    public class Vertex : IGremlinEntity, IComparable<Vertex>
    {
        public string Id { get; set; }
        public int CompareTo(Vertex other)
        {
            return String.Compare(this.Id, other.Id, StringComparison.Ordinal);
        }
    }
}