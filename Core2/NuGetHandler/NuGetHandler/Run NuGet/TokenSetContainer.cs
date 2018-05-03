namespace NuGetHandler.Run_NuGet
{
	public static class TokenSetContainer
	{
		// From the App.config file

		public static string ApiKey { get; set; }
		public static string AssemblyPath { get; set; }
		public static string BasePath { get; set; }
		public static string ConfigFile { get; set; }
		public static string ConfigurationName { get; set; }
		public static string Exclude { get; set; }
		public static string MinClientVersion { get; set; }
		public static string MSBuildPath { get; set; }
		public static string MSBuildVersion { get; set; }
		public static string Properties { get; set; }
		public static string Root { get; set; }
		public static string RuntimeIdentifier { get; set; }
		public static string Timeout { get; set; }
		public static string VerbosityNuGet { get; set; }
		public static string VerbosityDotNet { get; set; }
		public static string VersionSuffixNuGet { get; set; }
		public static string VersionSuffixDotNet { get; set; }

		// Calculated internally

		// Path to generated .nuspec path with framework is full framework.
		public static string NuSpecFilePath { get; set; }
		public static string PackageId { get; set; }
		public static string PackageName { get; set; }
		public static string PackageFileName { get; set; }
		public static string PackageDir { get; set; }
		public static string PackagePath { get; set; }
		public static string PackageVersion { get; set; }
		public static string ProjectPath { get; set; }
		public static string Source { get; set; }
		public static string SymbolSource { get; set; }
		public static string SymbolApiKey { get; set; }

	}
}
