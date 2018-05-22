namespace NuGetHandler.Run_NuGet
{
	using System.IO;
	using System.Text;
	using Infrastructure;
	using static ReplaceableTokens;
	using static TokenSetContainer;

	public static class ApplyTokenValuesToCommandLine
	{
		/// <summary>
		/// Replace the tokens with their desired values. Example command line
		/// follows:
		/// "-AssemblyPath $AssemblyPath$ -Force -ForceEnglishOutput -NonInteractive -Verbosity $VerbosityNuGet$"/>
		/// </summary>
		/// <param name="aTokenizedCommandLine"></param>
		/// <returns></returns>
		public static string BuildNuGetSpec(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(ASSEMBLY_PATH.AsToken(), AssemblyPath)
				.Replace(VERBOSITY_NUGET.AsToken(), VerbosityNuGet);
			return vResult.ToString();
		}

		public static string BuildNuGetPack
			(string aTokenizedCommandLine, bool aUseNuSpecFileIfAvailable)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			string vSourcePath =
				aUseNuSpecFileIfAvailable
					? NuSpecFilePath
					: ProjectPath;
			vResult
				.Replace(BASE_PATH.AsToken(), BasePath)
				.Replace(EXCLUDE.AsToken(), Exclude)
				.Replace(MIN_CLIENT_VERSION.AsToken(), MinClientVersion)
				.Replace(MS_BUILD_PATH.AsToken(), MSBuildPath)
				.Replace(MS_BUILD_VERSION.AsToken(), MSBuildVersion)
				//				.Replace(OUTPUT_PACKAGE_TO.AsToken(), PackagePath)
				.Replace
				(
					OUTPUT_PACKAGE_TO.AsToken()
					, PackageDir + Path.DirectorySeparatorChar
				)
				.Replace(PACKAGE_VERSION.AsToken(), PackageVersion)
				.Replace(PROJECT_PATH.AsToken(), vSourcePath)
				.Replace(PROPERTIES.AsToken(), Properties)
				.Replace(VERSION_SUFFIX_NUGET.AsToken(), VersionSuffixNuGet)
				.Replace(VERBOSITY_NUGET.AsToken(), VerbosityNuGet);
			return vResult.ToString();
		}

		public static string BuildNuGetPush(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(PACKAGE_PATH.AsToken(), PackagePath)
				.Replace(API_KEY.AsToken(), ApiKey)
				.Replace(CONFIG_FILE.AsToken(), ConfigFile)
				.Replace(SOURCE.AsToken(), Source)
				.Replace(SYMBOL_SOURCE.AsToken(), SymbolSource)
				.Replace(SYMBOL_API_KEY.AsToken(), SymbolApiKey)
				.Replace(TIMEOUT.AsToken(), Timeout)
				.Replace(VERBOSITY_NUGET.AsToken(), VerbosityNuGet);
			return vResult.ToString();
		}

		public static string BuildNuGetAdd(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(SOURCE.AsToken(), Source)
				.Replace(CONFIG_FILE.AsToken(), ConfigFile)
				.Replace(VERBOSITY_NUGET.AsToken(), VerbosityNuGet);
			return vResult.ToString();
		}

		public static string BuildNuGetDelete(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(PACKAGE_ID.AsToken(), PackageId)
				.Replace(PACKAGE_VERSION.AsToken(), PackageVersion)
				.Replace(API_KEY.AsToken(), ApiKey)
				.Replace(CONFIG_FILE.AsToken(), ConfigFile)
				.Replace(SOURCE.AsToken(), Source)
				.Replace(VERBOSITY_NUGET.AsToken(), VerbosityNuGet);
			return vResult.ToString();
		}

		// DotNet nuget command builders

		public static string BuildDotNetNuGetPack(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(PROJECT_PATH.AsToken(), ProjectPath)
				.Replace(CONFIGURATION_NAME.AsToken(), ConfigurationName)
				.Replace
				(
					OUTPUT_PACKAGE_TO.AsToken()
					, PackageDir + Path.DirectorySeparatorChar
				)
				.Replace(PACKAGE_VERSION.AsToken(), PackageVersion)
				.Replace(RUNTIME_IDENTIFIER.AsToken(), RuntimeIdentifier)
				.Replace(VERSION_SUFFIX_DOTNET.AsToken(), VersionSuffixDotNet)
				.Replace(VERBOSITY_DOTNET.AsToken(), VerbosityDotNet);
			return vResult.ToString();
		}

		public static string BuildDotNetNuGetPush(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(PACKAGE_PATH.AsToken(), PackagePath)
				.Replace(SOURCE.AsToken(), Source)
				.Replace(API_KEY.AsToken(), ApiKey)
				.Replace(SYMBOL_SOURCE.AsToken(), SymbolSource)
				.Replace(SYMBOL_API_KEY.AsToken(), SymbolApiKey)
				.Replace(TIMEOUT.AsToken(), Timeout)
				.Replace(VERBOSITY_DOTNET.AsToken(), VerbosityDotNet);
			return vResult.ToString();
		}

		public static string BuildDotNetNuGetDelete(string aTokenizedCommandLine)
		{
			StringBuilder vResult = new StringBuilder(aTokenizedCommandLine);
			vResult
				.Replace(PACKAGE_ID.AsToken(), PackageId)
				.Replace(PACKAGE_VERSION.AsToken(), PackageVersion)
				.Replace(SOURCE.AsToken(), Source)
				.Replace(API_KEY.AsToken(), ApiKey)
				.Replace(VERBOSITY_DOTNET.AsToken(), VerbosityDotNet);
			return vResult.ToString();
		}

	}
}
