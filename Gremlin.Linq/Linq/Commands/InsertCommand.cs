namespace Gremlin.Linq.Linq
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using Newtonsoft.Json;

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
            result.Append($".addV('{_entity.GetType().Name}')");
            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                result.Append(propertyInfo.BuildGremlinQuery(_entity));
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
            if (ParentCommand == null)
            {
                result.Append("g");
            }
            else
            {
                result.Append(ParentCommand.BuildGremlinQuery());
            }

            result.Append($".addV('{_entity.GetType().Name}')");
            var propertyInfos = _entity.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                result.Append(propertyInfo.BuildGremlinQuery(_entity));
            }

            return result.ToString();
        }
    }

    public static class InsertCommandExtensions
    {
        public static string BuildGremlinQuery(this PropertyInfo propertyInfo, object entity)
        {
            var value = propertyInfo.GetGetMethod().Invoke(entity, new object[0]);
            if (value == null)
            {
                return string.Empty;
            }

            if (value is DateTime)
            {
                var val = ((DateTime)value).ToString("s");
                return $".property('{propertyInfo.Name}', '{val}')";
            }

            if (value is double || value is float)
            {
                var val = (double) value;
                return $".property('{propertyInfo.Name}', {val.ToString(CultureInfo.GetCultureInfo("en-US"))})";
            }

            if (value is bool boolValue)
            {
                return $".property('{propertyInfo.Name}', {boolValue.ToString().ToLower()})";
            }

            if (value.GetType().IsPrimitive)
            {
                return $".property('{propertyInfo.Name}', {value})";
            }

            if (value is string)
            {
                return $".property('{propertyInfo.Name}', '{(value as string).Replace("\"", "")}')";
            }

            if (value is IEnumerable)
            {
                return $".property('{propertyInfo.Name}', '{JsonConvert.SerializeObject(value)}')";
            }

            if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
            {
                return $".property('{propertyInfo.Name}', '{JsonConvert.SerializeObject(value)}')";
            }

            return $".property('{propertyInfo.Name}', '{value}')";
        }
    }
}