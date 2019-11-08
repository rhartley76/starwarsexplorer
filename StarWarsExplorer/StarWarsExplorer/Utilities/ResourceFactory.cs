using System.Reactive.Subjects;
using StarWarsExplorer.Models;

namespace StarWarsExplorer.Utilities
{
	public class ResourceFactory
	{
		private readonly string _apiBaseUrl;
		private readonly IConnectableObservable<(string resource, string search)> _searchRequestStream;

		public ResourceFactory(string apiBaseUrl, IConnectableObservable<(string resource, string search)> searchRequestStream)
		{
			_apiBaseUrl = apiBaseUrl;
			_searchRequestStream = searchRequestStream;
		}

		public IResource CreateResource<TEntity>() where TEntity : Entity
		{
			return new Resource<TEntity>(_apiBaseUrl, _searchRequestStream);
		}
	}
}