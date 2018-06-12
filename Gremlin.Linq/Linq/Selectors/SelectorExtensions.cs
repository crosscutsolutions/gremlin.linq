namespace Gremlin.Linq.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using Commands;
    using Selectors;

    public static class SelectorExtensions
    {
        public static async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(this Selector selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitAsync<TEntity>(query);
            return queryResult;
        }

        public static async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(
            this SelectSelector<TEntity> selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitAsync<TEntity>(query);
            return queryResult;
        }
        public static async Task<IEnumerable<QueryResult<TEntity>>> SubmitAsync<TEntity>(
            this Selector<TEntity> selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitAsync<TEntity>(query);
            return queryResult;
        }

        public static async Task<QueryResult<TEntity>> SubmitWithSingleResultAsync<TEntity>(
            this Selector<TEntity> selector)
            where TEntity : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitWithSingleResultAsync<TEntity>(query);
            return queryResult;
        }

        public static SelectSelector Select(this Selector selector, params string[] selectors)
        {
            var selectSelector = new SelectSelector(selector.Client, selectors) {ParentSelector = selector};
            return selectSelector;
        }

        public static SelectSelector<T1> Select<T1>(this Selector selector)
        {
            var selectSelector = new SelectSelector<T1>(selector.Client) { ParentSelector = selector };
            return selectSelector;
        }        

        public static SelectSelector<T1> Select<T1>(this SetCommand<T1> selector)
        {
            var selectSelector = new SelectSelector<T1>(selector.Client) { ParentSelector = selector };
            return selectSelector;
        }

        public static SelectSelector<TSelect1> Select<TSelect1>(this SetCommand selector)
        {
            var selectSelector = new SelectSelector<TSelect1>(selector.Client) { ParentSelector = selector };
            return selectSelector;
        }

        public static WhereSelector<T> Where<T>(this Selector<T> selector, Expression<Func<T, bool>> expression)
        {
            var whereSelector = new WhereSelector<T>(selector.Client,expression)
            {
                ParentSelector = selector
            };
            return whereSelector;
        }

        public static SetCommand<T> Set<T,TValue>(this SelectSelector<T> selector, Expression<Func<T,TValue>> expression, TValue value)
        {
            PropertyInfo property;
            if (expression.Body is MemberExpression memberExpression)
            {
                property = typeof(T).GetProperty(memberExpression.Member.Name);
            }
            else
            {
                throw new ArgumentException("Expression must be of type MemberExpression");
            }
            var result = new SetCommand<T>(selector.Client, property,value)
            {
                ParentSelector = selector
            };
            return result;
        }

        public static SelectSelector<T1,T2> Select<T1,T2>(this Selector selector)
        {
            var selectSelector = new SelectSelector<T1,T2>(selector.Client) { ParentSelector = selector };
            return selectSelector;
        }

        public static SelectSelector<T1, T2, T3> Select<T1, T2,T3>(this Selector selector)
        {
            var selectSelector = new SelectSelector<T1, T2, T3>(selector.Client) { ParentSelector = selector };
            return selectSelector;
        }

        public static DropCommand Drop(this Selector selector)
        {
            var dropCommand = new DropCommand(selector.Client)
            {
                ParentSelector = selector
            };
            return dropCommand;
        }

        public static async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>>> SubmitAsync<T1, T2>(this SelectSelector selector) where T1 : new() where T2 : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitDynamicAsync<T1, T2>(query);
            return queryResult;
        }

        public static async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>>>> SubmitAsync<T1,T2>(this SelectSelector<T1,T2> selector) where T1 : new() where T2 : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitDynamicAsync<T1, T2>(query);
            return queryResult;
        }

        public static async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>> SubmitAsync<T1, T2, T3>(this SelectSelector selector) where T1 : new() where T2 : new() where T3: new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitDynamicAsync<T1, T2, T3>(query);
            return queryResult;
        }

        public static async Task<IEnumerable<Tuple<QueryResult<T1>, QueryResult<T2>, QueryResult<T3>>>> SubmitAsync<T1,T2,T3>(this SelectSelector<T1,T2,T3> selector) where T1 : new() where T2 : new() where T3 : new()
        {
            var query = selector.BuildGremlinQuery();
            var queryResult = await selector.Client.SubmitDynamicAsync<T1, T2, T3>(query);
            return queryResult;
        }
        
    }
}