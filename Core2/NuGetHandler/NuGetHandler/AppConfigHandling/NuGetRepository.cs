namespace NuGetHandler.AppConfigHandling
{
	public class NuGetRepository
	{
		public string RepositoryName { get; set; }
		public string Source { get; set; }
		public string SourceApiKey { get; set; }
		public string SymbolSource { get; set; }
		public string SymbolSourceApiKey { get; set; }
		public bool HasSource { get; set; }
		public bool HasSymbolSource { get; set; }
		public bool IsNuGetServer { get; set; }

	}
}
