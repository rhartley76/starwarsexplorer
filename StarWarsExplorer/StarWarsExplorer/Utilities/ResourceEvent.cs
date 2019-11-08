using System.Threading;

namespace StarWarsExplorer.Utilities
{
	public class ResourceEvent
	{
		public ResourceEvent(ApiEvent @event, CancellationTokenSource tokenSource)
		{
			Event = @event;
			TokenSource = tokenSource;
		}

		public ApiEvent Event { get; }

		public CancellationTokenSource TokenSource { get; }
	}
}