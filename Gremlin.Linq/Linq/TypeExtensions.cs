using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Gremlin.Linq.Linq
{
    public static class TypeExtensions
    {
        public static string GetLabel(this Type type)
        {
            return type.GetCustomAttribute<GremlinLabelAttribute>()?.Label ??
                   type.GetCustomAttribute<JsonObjectAttribute>()?.Id ??
                   type.Name;  
        }
    }
}