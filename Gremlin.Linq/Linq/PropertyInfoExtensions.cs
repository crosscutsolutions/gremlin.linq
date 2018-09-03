using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Gremlin.Linq
{
    public static class PropertyInfoExtensions
    {
        public static string GetPropertyName(this PropertyInfo element)
        {
            return element.GetCustomAttribute<GremlinPropertyAttribute>()?.Name ??
                   element.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   element.Name;
        }

        public static string GetPropertyName(this MemberInfo element)
        {
            return element.GetCustomAttribute<GremlinPropertyAttribute>()?.Name ??
                   element.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   element.Name;
        }
    }
}