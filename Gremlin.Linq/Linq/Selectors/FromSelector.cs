namespace Gremlin.Linq.Linq
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class FromSelector<TEntity> : Selector<TEntity>, ICountable, IWhereSelector<TEntity>
    {
        private readonly IGraphClient _graphClient;
        private string _alias;

        public FromSelector(IGraphClient graphClient) : base(graphClient)
        {
            _graphClient = graphClient;
        }

        public FromSelector<TEntity> As(string alias)
        {
            _alias = alias;
            return this;
        }

        public FromSelector<TEntity> As<T>()
        {
            _alias = typeof(T).GetLabel();
            return this;
        }

        public override string BuildGremlinQuery()
        {
            var result = $"g.V().has('label','{typeof(TEntity).GetLabel()}')";
            if (!string.IsNullOrEmpty(_alias))
            {
                result = result + $".as('{_alias}')";
            }
            return result;
        }

       

        public WhereSelector<TEntity> Where(Expression<Func<TEntity, bool>> func)
        {
            var whereSelector = new WhereSelector<TEntity>(_graphClient, func)
            {
                ParentSelector = this
            };
            return whereSelector;
        }
        
    }

    public static class TypeExtensions
    {
        public static string GetLabel(this Type t)
        {
            var labelAttribute = t.GetCustomAttribute<GremlinLabelAttribute>();
            if (labelAttribute != null)
            {
                return labelAttribute.Label;
            }

            return t.Name;            
        }
    }
}