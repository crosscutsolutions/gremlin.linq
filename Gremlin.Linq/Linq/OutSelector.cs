namespace Gremlin.Linq.Linq
{
    public class OutSelector<T> : Selector<T>
    {
        public OutSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override string BuildGremlinQuery()
        {
            return ParentSelector.BuildGremlinQuery() + $".out().has('label','{typeof(T).Name}')";
        }
    }
}