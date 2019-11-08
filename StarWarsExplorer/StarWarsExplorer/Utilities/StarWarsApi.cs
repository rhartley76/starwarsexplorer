using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using StarWarsExplorer.Models;

namespace StarWarsExplorer.Utilities
{
	public class StarWarsApi
	{
		public const string ALL_RESOURCES = "All";
		protected const string API_URL = "https://swapi.co/api";
		private readonly Subject<(string resource, string search)> _searchRequests;
		private readonly Subject<ApiEvent> _statusUpdates;
		private readonly ObservableCollection<CancellationTokenSource> _activeSearches;
		private readonly IResource[] _resources;

		public StarWarsApi()
		{
			_statusUpdates = new Subject<ApiEvent>();
			_activeSearches = new ObservableCollection<CancellationTokenSource>();
			_searchRequests = new Subject<(string resource, string search)>();

			var factory = new ResourceFactory(API_URL, _searchRequests.Publish());
			_resources = new[]
			{
				factory.CreateResource<People>(),
				factory.CreateResource<Planets>(),
				factory.CreateResource<Films>(),
				factory.CreateResource<Species>(),
				factory.CreateResource<Vehicles>(),
				factory.CreateResource<StarShips>()
			};
			_resources.Select(r => r.ActiveSearchStream)
				.Merge()
				.Subscribe(ProcessEvent);
		}

		public void SearchRequest(string resourceName, string searchText)
		{
			foreach (CancellationTokenSource search in _activeSearches.ToArray())
			{
				search.Cancel();
			}
			_activeSearches.Clear();
			_searchRequests.OnNext((resourceName, searchText));
		}

		public void LoadMoreData()
		{
			foreach (var resource in _resources
				.Where(r => r.IsActive && !string.IsNullOrWhiteSpace(r.NextPageUrl)))
			{
				resource.Search(resource.NextPageUrl);
			}
		}

		public IEnumerable<Entity> Results => _resources
			.Where(r => r.IsActive).SelectMany(r => r.Entities);

		public IEnumerable<string> Resources => _resources
			.Select(r => r.Name).Union(new[] { ALL_RESOURCES });

		public IEnumerable<string> Errors => _resources
			.Where(r => r.IsActive).Select(r => r.ErrorMessage);

		public bool HasMoreData => _resources.Where(r => r.IsActive)
			.Any(r => !string.IsNullOrWhiteSpace(r.NextPageUrl));

		public int TotalResultsAvailable => _resources
			.Where(r => r.IsActive)
			.Sum(r => r.TotalCount);

		public IObservable<ApiEvent> StatusStream => _statusUpdates;

		private void ProcessEvent(ResourceEvent @event)
		{
			switch (@event.Event)
			{
				case ApiEvent.Completed:
					_activeSearches.Remove(@event.TokenSource);
					if (_activeSearches.Count == 0)
					{
						_statusUpdates.OnNext(@event.Event);
					}
					break;
				case ApiEvent.Searching:
					_activeSearches.Add(@event.TokenSource);
					_statusUpdates.OnNext(@event.Event);
					break;
				case ApiEvent.Error:
					_activeSearches.Remove(@event.TokenSource);
					if (_activeSearches.Count == 0)
					{
						_statusUpdates.OnNext(@event.Event);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}