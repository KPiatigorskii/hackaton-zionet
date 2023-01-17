using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MsSqlAccessor.Models;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Helpers
{
    public static class QueryableExtensions
    {
		//public static IQueryable<T> IncludeVirtualProperties<T>(this IQueryable<T> query, T entity) where T : class
		//{
		//	//var virtualPropertiesFull = entity.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual && !p.IsDefined(typeof(JsonIgnoreAttribute)) && !typeof(ICollection).IsAssignableFrom(p.PropertyType));
		//	//var virtualProperties = entity.GetType().GetProperties().Where(p => p.GetMethod.IsVirtual);
		//	var virtualProperties = entity.GetType().GetProperties()
		//		.Where(p => p.GetMethod.IsVirtual && !p.IsDefined(typeof(JsonIgnoreAttribute)) && (p.Name != "Id") && (p.Name != "StatusId"));
		//	foreach (var property in virtualProperties)
		//	{
		//		query = query.Include(property.Name);
		//	}
		//	return query;
		//}

		public static IQueryable<T> IncludeVirtualProperties<T>(this IQueryable<T> query, T entity) where T : class
		{
			var properties = typeof(T).GetProperties();
			foreach (var property in properties)
			{
				var propertyType = property.PropertyType;
				if (propertyType.IsClass && !propertyType.IsAbstract && propertyType != typeof(string))
				{
					query = query.Include(property.Name);
					var nestedProperties = propertyType.GetProperties();
					foreach (var nestedProperty in nestedProperties)
					{
						if (nestedProperty.PropertyType.IsClass && !nestedProperty.PropertyType.IsAbstract && nestedProperty.PropertyType != typeof(string))
						{
							query = query.Include($"{property.Name}.{nestedProperty.Name}");
						}
					}
				}
			}
			return query;
		}
	}
}
