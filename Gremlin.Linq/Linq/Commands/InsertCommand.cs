using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Gremlin.Linq.Linq
{
    public class InsertCommand : Command
    {
        private readonly object _entity;

        internal InsertCommand(IGraphClient client, object entity) : base(client)
        {
            _entity = entity;
        }

        public Command ParentCommand { get; set; }

        public override string BuildGremlinQuery()
        {
            var result = new StringBuilder();
            result.Append(ParentCommand == null ? "g" : ParentCommand.BuildGremlinQuery());
            result.Append($".addV('{_entity.GetType().GetLabel()}')");
            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var gremlinCode = propertyInfo.BuildGremlinQuery(_entity);
                if (!string.IsNullOrEmpty(gremlinCode)) result.Append(gremlinCode);
            }

            return result.ToString();
        }
    }

    public class InsertCommand<TEntity> : Command<TEntity>
    {
        private readonly object _entity;

        public InsertCommand(IGraphClient client, TEntity entity) : base(client)
        {
            _entity = entity;
        }

        public Command<TEntity> ParentCommand { get; set; }

        public override string BuildGremlinQuery()
        {
            var result = new StringBuilder();
            result.Append(ParentCommand == null ? "g" : ParentCommand.BuildGremlinQuery());

            result.Append($".addV('{_entity.GetType().GetLabel()}')");
            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var gremlinQuery = propertyInfo.BuildGremlinQuery(_entity);
                if (!string.IsNullOrEmpty(gremlinQuery)) result.Append(gremlinQuery);
            }

            return result.ToString();
        }
    }

    public static class InsertCommandExtensions
    {
        public static string BuildGremlinQuery(this PropertyInfo propertyInfo, object entity)
        {
            if (propertyInfo.GetCustomAttribute<IgnoreAttribute>() != null) return null;
            var value = propertyInfo.GetGetMethod().Invoke(entity, new object[0]);
            if (value == null) return string.Empty;

            var propertyName = propertyInfo.Name;
            if (propertyInfo.GetCustomAttributes(typeof(GremlinPropertyAttribute)).SingleOrDefault() is
                GremlinPropertyAttribute propertyAttribute) propertyName = propertyAttribute.Name;
            return propertyName.BuildGremlinQueryForValue(value);
        }

        public static string BuildGremlinQueryForValue(this string propertyName, object value)
        {
            if (value is DateTime time)
            {
                var val = time.ToString("s");
                return $".property('{propertyName}', '{val}')";
            }

            if (value is double || value is float)
            {
                var val = (double) value;
                return $".property('{propertyName}', {val.ToString(CultureInfo.GetCultureInfo("en-US"))})";
            }

            if (value.GetType().IsEnum) return $".property('{propertyName}', {(int) value})";

            if (value is bool boolValue) return $".property('{propertyName}', {boolValue.ToString().ToLower()})";

            if (value.GetType().IsPrimitive) return $".property('{propertyName}', {value})";

            if (value is string s) return $".property('{propertyName}', '{s.Replace("\"", "")}')";

            if (value is IEnumerable) return $".property('{propertyName}', '{JsonConvert.SerializeObject(value)}')";

            if (value.GetType().IsClass || value.GetType().IsInterface)
                return $".property('{propertyName}', '{JsonConvert.SerializeObject(value)}')";

            return $".property('{propertyName}', '{value}')";
        }
    }
}