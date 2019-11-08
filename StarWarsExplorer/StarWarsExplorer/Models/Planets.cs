using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWarsExplorer.Models
{
	public class Planets : Entity
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("rotation_period")]
		public string RotationPeriod { get; set; }

		[JsonProperty("orbital_period")]
		public string OrbitalPeriod { get; set; }

		[JsonProperty("diameter")]
		public string Diameter { get; set; }

		[JsonProperty("climate")]
		public string Climate { get; set; }

		[JsonProperty("gravity")]
		public string Gravity { get; set; }

		[JsonProperty("terrain")]
		public string Terrain { get; set; }

		[JsonProperty("surface_water")]
		public string SurfaceWater { get; set; }

		[JsonProperty("population")]
		public string Population { get; set; }

		[JsonProperty("residents")]
		public IEnumerable<string> Residents { get; set; }

		[JsonProperty("films")]
		public IEnumerable<string> Films { get; set; }

		public override string ShortDisplay => $"{Name} [Diameter:{Diameter}, Climate:{Climate}]";
	}
}