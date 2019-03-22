using System;

namespace Gremlin.Linq
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GremlinPropertyAttribute : Attribute
    {
        public GremlinPropertyAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}