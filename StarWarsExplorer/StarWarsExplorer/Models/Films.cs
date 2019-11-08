using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarWarsExplorer.Models
{
	public class Films : Entity
	{
		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("episode_id")]
		public int EpisodeId { get; set; }

		[JsonProperty("opening_crawl")]
		public string OpeningCrawl { get; set; }

		[JsonProperty("director")]
		public string Director { get; set; }

		[JsonProperty("producer")]
		public string Producer { get; set; }

		[JsonProperty("release_date")]
		public DateTime ReleaseDate { get; set; }

		[JsonProperty("characters")]
		public IEnumerable<string> Characters { get; set; }

		[JsonProperty("planets")]
		public IEnumerable<string> Planets { get; set; }

		[JsonProperty("starships")]
		public IEnumerable<string> StarShips { get; set; }

		[JsonProperty("vehicles")]
		public IEnumerable<string> Vehicles { get; set; }

		[JsonProperty("species")]
		public IEnumerable<string> Species { get; set; }

		public override string ShortDisplay => $"Episode {EpisodeId}: {Title} Released: {ReleaseDate.ToLongDateString()}";
	}
}