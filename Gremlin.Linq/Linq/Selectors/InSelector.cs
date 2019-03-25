namespace Gremlin.Linq.Linq
{
    public class InSelector<T> : Selector<T>
    {
        public InSelector(IGraphClient graphClient) : base(graphClient)
        {
        }

        public override string BuildGremlinQuery()
        {
            var label = typeof(T).GetLabel();
            return ParentSelector.BuildGremlinQuery() + $".in().has('label','{label}')";
        }
    }
}