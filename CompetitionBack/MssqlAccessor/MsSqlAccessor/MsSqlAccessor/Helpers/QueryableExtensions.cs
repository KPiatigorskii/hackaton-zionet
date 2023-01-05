using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeVirtualProperties<T>(this IQueryable<T> query, T entity) where T : class
        {
            //var virtualPropertiesFull = entity.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual && !p.IsDefined(typeof(JsonIgnoreAttribute)) && !typeof(ICollection).IsAssignableFrom(p.PropertyType));
            //var virtualProperties = entity.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual);
            var virtualProperties = entity.GetType().GetProperties()
                .Where(p => p.GetMethod.IsVirtual && !p.IsDefined(typeof(JsonIgnoreAttribute)) && (p.Name != "Id") && (p.Name != "StatusId"));
            foreach (var property in virtualProperties)
            {
                query = query.Include(property.Name);
            }
            return query;
        }
    }
}
