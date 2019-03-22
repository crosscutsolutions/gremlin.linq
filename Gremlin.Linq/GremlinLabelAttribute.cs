using System;

namespace Gremlin.Linq
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GremlinLabelAttribute : Attribute
    {
        public GremlinLabelAttribute(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(label));
            Label = label;
        }

        public string Label { get; }
    }
}