using System;
using System.Collections.Generic;
using StarWarsExplorer.Models;

namespace StarWarsExplorer.Utilities
{
	public interface IResource
	{
		string Name { get; }

		bool IsActive { get; }

		IEnumerable<Entity> Entities { get; }

		string ErrorMessage { get; }

		string NextPageUrl { get; }

		int TotalCount { get; }

		IObservable<ResourceEvent> ActiveSearchStream { get; }

		void Search(string url);
	}
}