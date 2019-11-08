using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace StarWarsExplorer.Models
{
	public abstract class Entity
	{
		private static readonly ConcurrentDictionary<string, IEnumerable<PropertyInfo>> _propertiesCache
			= new ConcurrentDictionary<string, IEnumerable<PropertyInfo>>();

		[JsonProperty("created")]
		public DateTime Created { get; set; }

		[JsonProperty("edited")]
		public DateTime Edited { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		public abstract string ShortDisplay { get; }

		public string Resource => GetType().Name;

		public IEnumerable<EntityProperty> Properties
		{
			get
			{
				var entityType = GetType();
				return _propertiesCache
					.GetOrAdd($"{entityType.Name}-strings",_ => entityType.GetProperties()
						.Where(propInfo => propInfo.Name != nameof(Properties) 
						                   && propInfo.Name != nameof(ShortDisplay)
						                   && propInfo.Name != nameof(Resource)))
					.Select(propInfo =>
					{
						var value = propInfo.PropertyType == typeof(IEnumerable<string>) 
							? (propInfo.GetValue(this) as IEnumerable<string>)?.Count().ToString() 
							: propInfo.GetValue(this)?.ToString();
						string name = $"{FormatForDisplay(propInfo.Name)}:";
						return new EntityProperty(name, value);
					});
			}
		}

		private static string FormatForDisplay(string source)
		{
			return source
				.Where(char.IsUpper)
				.Aggregate(source, (current, next) => current.Replace(next.ToString(), $" {next}"))
				.Trim();
		}
	}
}