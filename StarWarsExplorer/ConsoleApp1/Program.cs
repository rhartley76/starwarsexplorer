using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace ConsoleApp1
{
	public class Person
	{
		[JsonProperty("homeworld")]
		public string HomeWorld { get; set; }
		
		[JsonProperty("birth_year")]
		public string BirthYear { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("skin_color")]
		public string SkinColor { get; set; }

		[JsonProperty("mass")]
		public string Mass { get; set; }

		[JsonProperty("gender")]
		public string Gender { get; set; }

		[JsonProperty("height")]
		public string Height { get; set; }

		[JsonProperty("eye_color")]
		public string EyeColor { get; set; }
	}

	public class Results
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("next")]
		public string NextPage { get; set; }

		[JsonProperty("previous")]
		public string PreviousPage { get; set; }

		[JsonProperty("results")]
		public List<Person> People { get; set; }

		[JsonIgnore]
		public int ThreadID { get; set; }
	}

	class Program
	{
		static HttpClient _client = new HttpClient();
		static void Main(string[] args)
		{
			const string url = "https://swapi.co/api/people";
			var searchStream = Observable.Create<Results>(async observer =>
			{
				int threadId = Thread.CurrentThread.ManagedThreadId;
				string requestResults = await _client.GetStringAsync(url);
				Results results = JsonConvert.DeserializeObject<Results>(requestResults);
				results.ThreadID = threadId;
				observer.OnNext(results);
				observer.OnCompleted();
				return Disposable.Empty;
			});

			IDisposable subscription = searchStream.Subscribe(
				results =>
				{
					Console.WriteLine($"Searched on Thread: {results.ThreadID}. Found {results.Count} total people. Number in this set: {results.People.Count}");
				}, 
				error => { Console.WriteLine($"Error: {error.Message}"); },
				() => { Console.WriteLine("Search Completed!"); });


			Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}");
			Console.ReadKey(intercept: true);
		}
	}
}
