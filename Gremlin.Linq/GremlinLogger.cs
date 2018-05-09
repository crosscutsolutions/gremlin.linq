namespace Gremlin.Linq
{
    using System;

    public class GremlinLogger : IGremlinLogger
    {
        public void Log(string gremlinExpression)
        {
            Console.WriteLine(gremlinExpression);
        }
    }
}