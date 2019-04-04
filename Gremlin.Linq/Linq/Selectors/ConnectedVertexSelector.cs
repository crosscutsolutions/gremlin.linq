namespace Gremlin.Linq.Linq
{
    public class ConnectedVertexSelector<T> : Selector<T>
    {
        private readonly string _relation;

        public ConnectedVertexSelector(IGraphClient graphClient, string relation) : base(graphClient)
        {
            _relation = relation;
        }

        public override string BuildGremlinQuery()
        {
            var vertexLabel = typeof(T).GetLabel();
            return ParentSelector.BuildGremlinQuery() + $".out('{_relation}').hasLabel('{vertexLabel}')";
        }
    }
}