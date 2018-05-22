namespace NuGetHandler.Help
{
	using AppConfigHandling;
	using Run_NuGet;
	using static Help;
	using static ReplaceableTokens;

	public static class HelpReplaceableTokens
	{
		public static void OutputReplaceableTokenValues()
		{
			Add($"{TokenWithValue(API_KEY, TokenSetContainer.ApiKey)}");
			Add($"{TokenWithValue(ASSEMBLY_PATH, TokenSetContainer.AssemblyPath)}");
			Add($"{TokenWithValue(BASE_PATH, TokenSetContainer.BasePath)}");
			Add($"{TokenWithValue(CONFIG_FILE, TokenSetContainer.ConfigFile)}");
			Add($"{TokenWithValue(CONFIGURATION_NAME, TokenSetContainer.ConfigurationName)}");
			Add($"{TokenWithValue(EXCLUDE, TokenSetContainer.Exclude)}");
			Add($"{TokenWithValue(MIN_CLIENT_VERSION, TokenSetContainer.MinClientVersion)}");
			Add($"{TokenWithValue(MS_BUILD_PATH, TokenSetContainer.MSBuildPath)}");
			Add($"{TokenWithValue(MS_BUILD_VERSION, TokenSetContainer.MSBuildVersion)}");
			Add($"{TokenWithValue(NUSPEC_FILE_PATH, TokenSetContainer.NuSpecFilePath)}");
			Add($"{TokenWithValue(OUTPUT_PACKAGE_TO, TokenSetContainer.PackagePath)}");
			Add($"{TokenWithValue(PACKAGE_ID, TokenSetContainer.PackageName)}");
			Add($"{TokenWithValue(PACKAGE_NAME, TokenSetContainer.PackageName)}");
			Add($"{TokenWithValue(PACKAGE_PATH, TokenSetContainer.PackagePath)}");
			Add($"{TokenWithValue(PACKAGE_VERSION, TokenSetContainer.PackageVersion)}");
			Add($"{TokenWithValue(PROJECT_PATH, TokenSetContainer.ProjectPath)}");
			Add($"{TokenWithValue(PROPERTIES, TokenSetContainer.Properties)}");
			Add($"{TokenWithValue(ROOT, TokenSetContainer.Root)}");  // Equals Package Path for dotnet.exe
			Add($"{TokenWithValue(RUNTIME_IDENTIFIER, TokenSetContainer.RuntimeIdentifier)}");
			Add($"{TokenWithValue(SOURCE, TokenSetContainer.Source)}");
			Add($"{TokenWithValue(SYMBOL_SOURCE, TokenSetContainer.SymbolSource)}");
			Add($"{TokenWithValue(SYMBOL_API_KEY, TokenSetContainer.SymbolApiKey)}");
			Add($"{TokenWithValue(TIMEOUT, TokenSetContainer.Timeout)}");
			Add($"{TokenWithValue(VERBOSITY_DOTNET, TokenSetContainer.VerbosityDotNet)}");
			Add($"{TokenWithValue(VERBOSITY_NUGET, TokenSetContainer.VerbosityNuGet)}");
			Add($"{TokenWithValue(VERSION, TokenSetContainer.PackageVersion)}");
			Add($"{TokenWithValue(VERSION_SUFFIX_DOTNET, TokenSetContainer.VersionSuffixDotNet)}");
			Add($"{TokenWithValue(VERSION_SUFFIX_NUGET, TokenSetContainer.VersionSuffixNuGet)}");
		}

		public static void OutputReplaceableTokens()
		{
			Add(ReplaceableTokens.ToString());
		}

		public static void OutputTokens()
		{
			OutputReplaceableTokens();
			SectionBreak("Replaceable Token Values");
			OutputReplaceableTokenValues();
		}

	}
}
