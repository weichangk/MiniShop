using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using MiniShop.Dto.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniShop.Api.Code.Filters
{
    /// <summary>
    /// 隐藏属性
    /// </summary>
    public class IgnorePropertySchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var ignoreProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<IgnorePropertyAttribute>() != null);

            foreach (var ignorePropertyInfo in ignoreProperties)
            {
                var propertyToRemove = schema.Properties.Keys.SingleOrDefault(x => x.ToLower() == ignorePropertyInfo.Name.ToLower());

                if (propertyToRemove != null)
                {
                    schema.Properties.Remove(propertyToRemove);
                }
            }
        }
    }
}
