namespace NuGetHandler.Infrastructure
{
	using System.Collections.Generic;

	public static class ErrorContainer
	{
		public static List<string> Errors { get; } = new List<string>();

	}
}
