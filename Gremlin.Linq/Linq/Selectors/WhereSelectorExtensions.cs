namespace Gremlin.Linq.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class WhereSelectorExtensions
    {
        public static ConnectedVertexSelector<TEdgeEntity> SelectOut<TEdgeEntity>(this IWhereSelector selector, string relation)
        {
            var edgeSelector = new ConnectedVertexSelector<TEdgeEntity>(selector.Client, relation)
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

        public static WhereInSelector<T> WhereIn<T>(this IWhereSelector<T> selector, Expression<Func<T, object>> func,
            IEnumerable<object> values)
        {
            var whereSelector = new WhereInSelector<T>(selector.Client, func, values)
            {
                ParentSelector = selector
            };
            return whereSelector;
        }
        

        public static OutSelector<T> Out<T>(this Selector selector, string edgeLabel = null)
        {
            var outSelector = new OutSelector<T>(selector.Client, edgeLabel)
            {
                ParentSelector = selector
            };
            return outSelector;
        }

        public static InEdgeSelector<T> InEdge<T>(this IWhereSelector selector) where T : Edge
        {
            var outSelector = new InEdgeSelector<T>(selector.Client)
            {
                ParentSelector = selector
            };
            return outSelector;
        }
        
        public static OutEdgeSelector<T> OutEdge<T>(this IWhereSelector selector) where T:Edge
        {
            var outSelector = new OutEdgeSelector<T>(selector.Client)
            {
                ParentSelector = selector
            };
            return outSelector;
        }

        public static InSelector<T> In<T>(this IWhereSelector selector, string edgeLabel = null)
        {
            var inSelector = new InSelector<T>(selector.Client, edgeLabel)
            {
                ParentSelector = selector
            };
            return inSelector;
        }
    }

    public static class EdgeSelectorExtensions
    {
        public static OutVertexSelector<TVertex> OutVertex<TVertex>(this EdgeSelector edgeSelector)
        {
            return new OutVertexSelector<TVertex>(edgeSelector.Client)
            {
                ParentSelector = edgeSelector
            };
        }
        public static InVertexSelector<TVertex> InVertex<TVertex>(this EdgeSelector edgeSelector)
        {
            return new InVertexSelector<TVertex>(edgeSelector.Client)
            {
                ParentSelector = edgeSelector
            };
        }
    }
}