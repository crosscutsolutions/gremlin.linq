namespace Gremlin.Linq.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class WhereSelectorExtensions
    {
        public static EdgeSelector<TEdgeEntity> SelectOut<TEdgeEntity>(this IWhereSelector selector, string relation)
        {
            var edgeSelector = new EdgeSelector<TEdgeEntity>(selector.Client, relation)
            {
                ParentSelector = selector
            };
            return edgeSelector;
        }

        public static AddEdgeCommand AddOut<TEdgeEntity>(this IWhereSelector selector, TEdgeEntity entity,
            string relation)
        {
            var addCommand = new AddEdgeCommand(selector.Client, entity, relation)
            {
                ParentSelector = selector,
                InsertCommand = new InsertCommand(selector.Client, entity)
            };
            return addCommand;
        }

        public static AddEdgeCommand<TEdgeEntity> AddOut<TFromEntity, TEdgeEntity>(this Command<TFromEntity> command,
            TEdgeEntity entity, string relation)
        {
            var addCommand = new AddEdgeCommand<TEdgeEntity>(command.Client, entity, relation)
            {
                ParentCommand = command,
                InsertCommand = new InsertCommand(command.Client, entity)
            };
            return addCommand;
        }

        public static WhereSelector<T> Where<T>(this IWhereSelector<T> selector, Expression<Func<T, bool>> func)
        {
            var whereSelector = new WhereSelector<T>(selector.Client, func)
            {
                ParentSelector = selector
            };
            return whereSelector;
        }

        public static WhereInSelector<T> WhereIn<T>(this IWhereSelector<T> selector, Expression<Func<T, object>> func,
            IEnumerable<object> values)
        {
            var whereSelector = new WhereInSelector<T>(selector.Client, func, values)
            {
                ParentSelector = selector
            };
            return whereSelector;
        }

        public static OutSelector<T> Out<T>(this IWhereSelector selector)
        {
            var outSelector = new OutSelector<T>(selector.Client)
            {
                ParentSelector = selector
            };
            return outSelector;
        }

        public static InSelector<T> In<T>(this IWhereSelector selector)
        {
            var inSelector = new InSelector<T>(selector.Client)
            {
                ParentSelector = selector
            };
            return inSelector;
        }
    }
}