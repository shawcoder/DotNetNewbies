namespace NuGetHandler.AppConfigHandling
{
	using System.Collections.Generic;

	public class NuGetDestination
	{
		public string DestinationName { get; set; }
		public List<NuGetRepository> Repositories { get; set; }

		public NuGetDestination() { Repositories = new List<NuGetRepository>(); }
	}
}
