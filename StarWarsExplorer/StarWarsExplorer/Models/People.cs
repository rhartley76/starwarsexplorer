using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWarsExplorer.Models
{
	public class People : Entity
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("height")]
		public string Height { get; set; }

		[JsonProperty("mass")]
		public string Mass { get; set; }

		[JsonProperty("hair_color")]
		public string HairColor { get; set; }

		[JsonProperty("skin_color")]
		public string SkinColor { get; set; }

		[JsonProperty("eye_color")]
		public string EyeColor { get; set; }

		[JsonProperty("birth_year")]
		public string BirthYear { get; set; }

		[JsonProperty("gender")]
		public string Gender { get; set; }

		[JsonProperty("homeworld")]
		public string HomeWorld { get; set; }

		[JsonProperty("films")]
		public IEnumerable<string> Films { get; set; }

		[JsonProperty("species")]
		public IEnumerable<string> Species { get; set; }

		[JsonProperty("vehicles")]
		public IEnumerable<string> Vehicles { get; set; }

		[JsonProperty("starships")]
		public IEnumerable<string> StarShips { get; set; }

		public override string ShortDisplay => $"{Name} [Gender:{Gender}, Birth Year:{BirthYear}]";
	}
}