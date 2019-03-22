using System.Text;

namespace Gremlin.Linq.Linq
{
    public class UpdateCommand : Command
    {
        private readonly object _entity;

        internal UpdateCommand(IGraphClient client, object entity) : base(client)
        {
            _entity = entity;
        }

        public Command ParentCommand { get; set; }

        public override string BuildGremlinQuery()
        {
            var result = new StringBuilder();
            result.Append(ParentCommand == null ? "g" : ParentCommand.BuildGremlinQuery());
            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var gremlinCode = propertyInfo.BuildGremlinQuery(_entity);
                if (!string.IsNullOrEmpty(gremlinCode)) result.Append(gremlinCode);
            }

            return result.ToString();
        }
    }

    public class UpdateCommand<TEntity> : Command<TEntity>
    {
        private readonly object _entity;

        public UpdateCommand(IGraphClient client, TEntity entity) : base(client)
        {
            _entity = entity;
        }

        public Command<TEntity> ParentCommand { get; set; }
        public IGremlinQueryable ParentSelector { get; set; }

        public override string BuildGremlinQuery()
        {
            var result = new StringBuilder();
            if (ParentSelector != null)
                result.Append(ParentSelector.BuildGremlinQuery());
            else if (ParentCommand != null)
                result.Append(ParentCommand.BuildGremlinQuery());
            else
                result.Append("g");

            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var gremlinQuery = propertyInfo.BuildGremlinQuery(_entity);
                if (!string.IsNullOrEmpty(gremlinQuery)) result.Append(gremlinQuery);
            }

            return result.ToString();
        }
    }
}