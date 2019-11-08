using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StarWarsExplorer.Models;

namespace StarWarsExplorerUnitTests
{
	[TestClass]
	public class FilmsTests
	{
		[TestMethod]
		public void Deserialize()
		{
			const string JSON = @"{
    ""title"": ""A New Hope"", 

		""episode_id"": 4, 
    ""opening_crawl"": ""It is a period of civil war.\r\nRebel spaceships, striking\r\nfrom a hidden base, have won\r\ntheir first victory against\r\nthe evil Galactic Empire.\r\n\r\nDuring the battle, Rebel\r\nspies managed to steal secret\r\nplans to the Empire's\r\nultimate weapon, the DEATH\r\nSTAR, an armored space\r\nstation with enough power\r\nto destroy an entire planet.\r\n\r\nPursued by the Empire's\r\nsinister agents, Princess\r\nLeia races home aboard her\r\nstarship, custodian of the\r\nstolen plans that can save her\r\npeople and restore\r\nfreedom to the galaxy...."", 
    ""director"": ""George Lucas"", 
    ""producer"": ""Gary Kurtz, Rick McCallum"", 
    ""release_date"": ""1977-05-25"", 
    ""characters"": [
        ""https://swapi.co/api/people/1/"",
        ""https://swapi.co/api/people/2/"",
        ""https://swapi.co/api/people/3/"",
        ""https://swapi.co/api/people/4/"",
        ""https://swapi.co/api/people/5/"",
        ""https://swapi.co/api/people/6/"",
        ""https://swapi.co/api/people/7/"",
        ""https://swapi.co/api/people/8/"",
        ""https://swapi.co/api/people/9/"",
        ""https://swapi.co/api/people/10/"",
        ""https://swapi.co/api/people/12/"",
        ""https://swapi.co/api/people/13/"",
        ""https://swapi.co/api/people/14/"",
        ""https://swapi.co/api/people/15/"",
        ""https://swapi.co/api/people/16/"",
        ""https://swapi.co/api/people/18/"",
        ""https://swapi.co/api/people/19/"",
        ""https://swapi.co/api/people/81/""
    ], 
    ""planets"": [
        ""https://swapi.co/api/planets/2/"",
        ""https://swapi.co/api/planets/3/"",
        ""https://swapi.co/api/planets/1/""
    ], 
    ""starships"": [
        ""https://swapi.co/api/starships/2/"",
        ""https://swapi.co/api/starships/3/"",
        ""https://swapi.co/api/starships/5/"",
        ""https://swapi.co/api/starships/9/"",
        ""https://swapi.co/api/starships/10/"",
        ""https://swapi.co/api/starships/11/"",
        ""https://swapi.co/api/starships/12/"",
        ""https://swapi.co/api/starships/13/""
    ], 
    ""vehicles"": [
        ""https://swapi.co/api/vehicles/4/"",
        ""https://swapi.co/api/vehicles/6/"",
        ""https://swapi.co/api/vehicles/7/"",
        ""https://swapi.co/api/vehicles/8/""
    ], 
    ""species"": [
        ""https://swapi.co/api/species/5/"",
        ""https://swapi.co/api/species/3/"",
        ""https://swapi.co/api/species/2/"",
        ""https://swapi.co/api/species/1/"",
        ""https://swapi.co/api/species/4/""
    ], 
    ""created"": ""2014-12-10T14:23:31.880000Z"", 
    ""edited"": ""2015-04-11T09:46:52.774897Z"", 
    ""url"": ""https://swapi.co/api/films/1/""
}";

			Films result = JsonConvert.DeserializeObject<Films>(JSON);

			Assert.AreEqual(4, result.EpisodeId);
			Assert.AreEqual("George Lucas", result.Director);
			Assert.AreEqual("A New Hope", result.Title);
			Assert.AreEqual(new DateTime(1977,5,25).Date, result.ReleaseDate.Date);
		}

		[TestMethod]
		public void Planet()
		{
			string input =
				@"{
  ""name"": ""Alderaan"", 
	""rotation_period"": ""24"", 
	""orbital_period"": ""364"", 
	""diameter"": ""12500"", 
	""climate"": ""temperate"", 
	""gravity"": ""1 standard"", 
	""terrain"": ""grasslands, mountains"", 
	""surface_water"": ""40"", 
	""population"": ""2000000000"", 
	""residents"": [
	""https://swapi.co/api/people/5/"",
	""https://swapi.co/api/people/68/"",
	""https://swapi.co/api/people/81/""
		], 
	""films"": [
	""https://swapi.co/api/films/6/"",
	""https://swapi.co/api/films/1/""
		], 
	""created"": ""2014-12-10T11:35:48.479000Z"", 
	""edited"": ""2014-12-20T20:58:18.420000Z"", 
	""url"": ""https://swapi.co/api/planets/2/""
}";

			Planets x = JsonConvert.DeserializeObject<Planets>(input);

			Assert.AreEqual("Alderaan", x.Name);
			Assert.AreEqual(new DateTime(2014, 12, 10).Date, x.Created.Date);
		}

		[TestMethod]
		public void Testing()
		{
			var person = new People
			{
				Name = "Test",
				Created = DateTime.Now,
				EyeColor = "blue",
				HomeWorld = "world",
				Films = new List<string> { "one", "two", "three" }
			};

			var p2 = new People();
			_ = p2.Properties;
			_ = p2.CollectionProperties;

			var result = person.Properties.ToArray();
			var r2 = person.CollectionProperties.ToArray();

			Assert.IsTrue(result.Any(r => r.Name == "Name" && r.Value == person.Name));
			Assert.IsTrue(r2.Any(r => r.Name == "Films" && r.Value == "3"));
		}
	}
}
