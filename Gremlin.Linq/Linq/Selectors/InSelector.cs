namespace Gremlin.Linq.Linq
{
    public class InSelector<T> : Selector<T>
    {
        private readonly string _edgeLabel;

        public InSelector(IGraphClient graphClient, string edgeLabel) : base(graphClient)
        {
            _edgeLabel = edgeLabel;
        }

        public override string BuildGremlinQuery()
        {
            var vertexLabel = typeof(T).GetLabel();
            if (!string.IsNullOrEmpty(_edgeLabel))
                return ParentSelector.BuildGremlinQuery() + $".in('{_edgeLabel}').has('label','{vertexLabel}')";
            return ParentSelector.BuildGremlinQuery() + $".in().has('label','{vertexLabel}')";
        }
    }
}