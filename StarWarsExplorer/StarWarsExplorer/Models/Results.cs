using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWarsExplorer.Models
{
	public class Results<T> where T : Entity
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("next")]
		public string NextPage { get; set; }

		[JsonProperty("previous")]
		public string PreviousPage { get; set; }

		[JsonProperty("results")]
		public IEnumerable<T> Items { get; set; }
	}
}