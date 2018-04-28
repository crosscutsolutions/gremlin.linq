namespace Gremlin.Linq.Linq
{
    public class FromAnySelector : Selector
    {
        public FromAnySelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override string BuildGremlinQuery()
        {
            return "g.V()";
        }
    }
}