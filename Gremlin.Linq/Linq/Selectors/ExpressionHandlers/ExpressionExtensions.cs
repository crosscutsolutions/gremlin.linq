using System.Linq;
using System.Reflection;

namespace Gremlin.Linq.Linq.Selectors.ExpressionHandlers
{
    public static class ExpressionExtensions
    {
        public static string GetGremlinPropertyName(this MemberInfo memberInfo)
        {
            var propertyName = memberInfo.Name;
            if (memberInfo.GetCustomAttributes(false).AsEnumerable().OfType<GremlinPropertyAttribute>()
                .SingleOrDefault() is GremlinPropertyAttribute propertyAttribute)
                propertyName = propertyAttribute.Name;
            return propertyName;
        }
    }
}