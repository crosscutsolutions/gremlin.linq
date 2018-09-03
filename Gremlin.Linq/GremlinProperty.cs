using System;

namespace Gremlin.Linq
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GremlinPropertyAttribute : Attribute
    {
        public string Name { get; }

        public GremlinPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}