namespace Gremlin.Linq.Linq
{
    public class InSelector<T> : Selector<T>
    {
        public InSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override string BuildGremlinQuery()
        {
            return ParentSelector.BuildGremlinQuery() + $".in().has('label','{typeof(T).Name}')";
        }
    }
}