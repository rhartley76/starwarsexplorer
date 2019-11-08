using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using PropertyChanged;
using StarWarsExplorer.Models;
using StarWarsExplorer.Utilities;

namespace StarWarsExplorer.ViewModels
{
	public class ExplorerViewModel : INotifyPropertyChanged
	{
		private readonly StarWarsApi _api;
		public event PropertyChangedEventHandler PropertyChanged;

		public ExplorerViewModel()
		{
			_api = new StarWarsApi();
			var whenPropertyChanges = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				handler => handler.Invoke,
				h => PropertyChanged += h,
				h => PropertyChanged += h);

			whenPropertyChanges
				.Where(p => p.EventArgs.PropertyName == nameof(SelectedChoice))
				.Subscribe(p => _api.SearchRequest(SelectedChoice, SearchText));
			whenPropertyChanges
				.Where(p => p.EventArgs.PropertyName == nameof(SearchText))
				.Throttle(TimeSpan.FromMilliseconds(500))
				.Subscribe(p => _api.SearchRequest(SelectedChoice, SearchText));

			_api.StatusStream.Subscribe(UpdateStatus);

			ResourceChoices = _api.Resources;
			SelectedChoice = ResourceChoices.First();
			LoadMoreCommand = new RelayCommand(() => _api.LoadMoreData(), () => true);
		}

		public bool IsWorking { get; set; }

		public IEnumerable<string> ResourceChoices { get; set; }

		public string SelectedChoice { get; set; }

		public string SearchText { get; set; }

		public IEnumerable<Entity> Entities { get; set; }

		public IEnumerable<string> Errors { get; set; }

		public bool HasErrors { get; set; }

		public bool HasResults { get; set; }

		public bool MoreDataOnServer { get; set; }

		public string RecordCountDisplay { get; set; }

		[DoNotNotify]
		public RelayCommand LoadMoreCommand { get; set; }

		private void UpdateStatus(ApiEvent @event)
		{
			switch (@event)
			{
				case ApiEvent.Error:
					Entities = Enumerable.Empty<Entity>();
					Errors = _api.Errors;
					HasErrors = true;
					IsWorking = false;
					RecordCountDisplay = null;
					break;
				case ApiEvent.Completed:
					Entities = _api.Results;
					Errors = Enumerable.Empty<string>();
					HasErrors = false;
					MoreDataOnServer = _api.HasMoreData;
					IsWorking = false;
					int currentResultsAvailable = Entities?.Count() ?? 0;
					HasResults = currentResultsAvailable > 0;
					RecordCountDisplay = $"{currentResultsAvailable}/{_api.TotalResultsAvailable}";
					break;
				case ApiEvent.Searching:
					IsWorking = true;
					Errors = Enumerable.Empty<string>();
					break;
			}
		}
	}
}