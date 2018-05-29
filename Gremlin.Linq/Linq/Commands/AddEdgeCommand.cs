namespace Gremlin.Linq.Linq
{
    public class AddEdgeCommand : Command
    {
        internal readonly object Entity;
        protected readonly string Relation;

        internal AddEdgeCommand(IGraphClient client, object entity, string relation) : base(client)
        {
            Entity = entity;
            Relation = relation;
        }   

        internal IGremlinQueryable ParentSelector { get; set; }
        internal InsertCommand InsertCommand { get; set; }
        public Selector Selector { get; set; }

        public override string BuildGremlinQuery()
        {
            if (InsertCommand != null)
            {
                return ParentSelector.BuildGremlinQuery() +
                       $".addE('{Relation}').to({InsertCommand.BuildGremlinQuery()}).inV()";
            }

            if (Selector != null)
            {
                return ParentSelector.BuildGremlinQuery() +
                       $".addE('{Relation}').to({Selector.BuildGremlinQuery()}).inV()";
            }

            return ParentSelector.BuildGremlinQuery();
        }
    }

    public class AddEdgeCommand<TEntity> : Command<TEntity>
    {
        private readonly object _entity;
        private readonly string _relation;

        internal AddEdgeCommand(IGraphClient client, object entity, string relation) : base(client)
        {
            _entity = entity;
            _relation = relation;
        }

        internal Command ParentCommand { get; set; }
        public InsertCommand InsertCommand { get; set; }
        public Command<TEntity> ParentSelector { get; set; }
        public Selector<TEntity> Selector { get; set; }

        public override string BuildGremlinQuery()
        {
            var command = ParentCommand != null
                ? ParentCommand.BuildGremlinQuery()
                : ParentSelector.BuildGremlinQuery();

            if (InsertCommand != null)
            {
                return command +
                       $".addE('{_relation}').to({InsertCommand.BuildGremlinQuery()}).inV()";
            }

            if (Selector != null)
            {
                return command +
                       $".addE('{_relation}').to({Selector.BuildGremlinQuery()}).inV()";
            }

            return command;
        }
    }
}