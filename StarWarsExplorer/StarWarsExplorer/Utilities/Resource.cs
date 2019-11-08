using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWarsExplorer.Models;

namespace StarWarsExplorer.Utilities
{
	public class Resource<TEntity> : IResource where TEntity : Entity
	{
		private static readonly HttpClient _client = new HttpClient();
		private readonly Subject<ResourceEvent> _statusUpdateStream;
		private string _lastErrorMessage;

		public Resource(string apiBaseUrl, 
			IConnectableObservable<(string resource, string search)> searchRequestStream)
		{
			Name = typeof(TEntity).Name;
			_statusUpdateStream = new Subject<ResourceEvent>();
			searchRequestStream
				.Where(request => !string.IsNullOrWhiteSpace(request.resource))
				.Subscribe(request =>
				{
					IsActive = request.resource.Equals(Name, StringComparison.OrdinalIgnoreCase) ||
					           request.resource.Equals(StarWarsApi.ALL_RESOURCES);
					if (!IsActive) return;

					Entities = null;
					string searchText = !string.IsNullOrWhiteSpace(request.search)
						? $"?search={Uri.EscapeUriString(request.search)}"
						: null;
					Search($"{apiBaseUrl}/{Name.ToLower()}/{searchText}");
				});
			searchRequestStream.Connect();
		}

		public string Name { get; private set; }

		public bool IsActive { get; private set; }

		public string ErrorMessage => !string.IsNullOrWhiteSpace(_lastErrorMessage) ? $"Resource {Name} Error: {_lastErrorMessage}" : null;

		public string NextPageUrl { get; set; }

		public IObservable<ResourceEvent> ActiveSearchStream => _statusUpdateStream;

		public int TotalCount { get; set; }

		public IEnumerable<Entity> Entities { get; private set; }

		public void Search(string url)
		{
			_lastErrorMessage = null;
			var tokenSource = new CancellationTokenSource();
			_statusUpdateStream.OnNext(new ResourceEvent(ApiEvent.Searching, tokenSource));
			SearchAsync(url, tokenSource.Token)
				.ToObservable()
				.Subscribe(results =>
					{
						TotalCount = results.Count;
						NextPageUrl = results.NextPage;
						var resultEntities = results.Items.Cast<Entity>();
						Entities = Entities != null ? Entities.Union(resultEntities) : resultEntities;
						_statusUpdateStream.OnNext(new ResourceEvent(ApiEvent.Completed, tokenSource));
					},
					error =>
					{
						if (error is TaskCanceledException) return;
						_lastErrorMessage = error.InnerException?.Message ?? error.Message;
						_statusUpdateStream.OnNext(new ResourceEvent(ApiEvent.Error, tokenSource));
					});
		}

		private Task<Results<TEntity>> SearchAsync(string url, CancellationToken token)
		{
			return Task.Run(async () =>
			{
				HttpResponseMessage result = await _client.GetAsync(url, token);
				result.EnsureSuccessStatusCode();
				string resultJson = await result.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<Results<TEntity>>(resultJson);
			}, token);
		}
	}
}