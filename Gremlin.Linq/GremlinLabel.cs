using System;
using System.Collections.Generic;
using System.Text;

namespace Gremlin.Linq
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GremlinLabelAttribute : Attribute
    {
        public string Label { get; }

        public GremlinLabelAttribute(string label)
        {
            Label = label;
        }
    }
}
