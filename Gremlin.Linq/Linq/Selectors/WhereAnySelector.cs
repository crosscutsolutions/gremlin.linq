namespace Gremlin.Linq.Linq
{
    public class WhereAnySelector : Selector
    {
        private readonly IGraphClient _graphClient;
        private readonly string _hasField;
        private readonly object _value;

        public WhereAnySelector(IGraphClient graphClient, string hasField, object value) : base(graphClient)
        {
            _graphClient = graphClient;
            _hasField = hasField;
            _value = value;
        }

        public ConnectedVertexSelector<TEdgeEntity> SelectOut<TEdgeEntity>(string relation)
        {
            var edgeSelector = new ConnectedVertexSelector<TEdgeEntity>(_graphClient, relation)
            {
                ParentSelector = this
            };
            return edgeSelector;
        }

        public override string BuildGremlinQuery()
        {
            if (_value is string)
            {
                return ParentSelector.BuildGremlinQuery() + $".has('{_hasField}', '{_value}')";
            }

            return ParentSelector.BuildGremlinQuery() + $".has('{_hasField}', {_value})";
        }
    }
}