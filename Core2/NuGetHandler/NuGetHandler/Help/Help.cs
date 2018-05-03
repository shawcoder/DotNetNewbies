namespace NuGetHandler.Help
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using AppConfigHandling;
	using ConfigurationHandler;
	using Infrastructure;
	using ProjectFileProcessing;
	using Run_NuGet;
	using static System.Console;
	using static AppConfigHandling.CommandLineSettings;
	using static HelpCommandLine;
	using static ReplaceableTokens;

	public static class Help
	{
		private static readonly StringBuilder _HelpContent = new StringBuilder();

		private static void OutputReplaceableTokens()
		{
			Add("***** Replaceable Tokens from App.config\n");
			Add($"{nameof(API_KEY)} = {API_KEY.AsToken()}");
			Add($"{nameof(ASSEMBLY_PATH)} = {ASSEMBLY_PATH.AsToken()}");
			Add($"{nameof(BASE_PATH)} = {BASE_PATH.AsToken()}");
			Add($"{nameof(CONFIG_FILE)} = {CONFIG_FILE.AsToken()}");
			Add($"{nameof(CONFIGURATION_NAME)} = {CONFIGURATION_NAME.AsToken()}");
			Add($"{nameof(EXCLUDE)} = {EXCLUDE.AsToken()}");
			Add($"{nameof(MIN_CLIENT_VERSION)} = {MIN_CLIENT_VERSION.AsToken()}");
			Add($"{nameof(MS_BUILD_PATH)} = {MS_BUILD_PATH.AsToken()}");
			Add($"{nameof(MS_BUILD_VERSION)} = {MS_BUILD_VERSION.AsToken()}");
			Add($"{nameof(NUSPEC_FILE_PATH)} = {NUSPEC_FILE_PATH.AsToken()}");
			Add($"{nameof(OUTPUT_PACKAGE_TO)} = {OUTPUT_PACKAGE_TO.AsToken()}");
			Add($"{nameof(PACKAGE_ID)} = {PACKAGE_ID.AsToken()}");
			Add($"{nameof(PACKAGE_NAME)} = {PACKAGE_NAME.AsToken()}");
			Add($"{nameof(PACKAGE_PATH)} = {PACKAGE_PATH.AsToken()}");
			Add($"{nameof(PACKAGE_VERSION)} = {PACKAGE_VERSION.AsToken()}");
			Add($"{nameof(PROJECT_PATH)} = {PROJECT_PATH.AsToken()}");
			Add($"{nameof(PROPERTIES)} = {PROPERTIES.AsToken()}");
			Add($"{nameof(ROOT)} = {ROOT.AsToken()}");  // Equals Package Path for dotnet.exe
			Add($"{nameof(RUNTIME_IDENTIFIER)} = {RUNTIME_IDENTIFIER.AsToken()}");
			Add($"{nameof(SOURCE)} = {SOURCE.AsToken()}");
			Add($"{nameof(SYMBOL_SOURCE)} = {SYMBOL_SOURCE.AsToken()}");
			Add($"{nameof(SYMBOL_API_KEY)} = {SYMBOL_API_KEY.AsToken()}");
			Add($"{nameof(TIMEOUT)} = {TIMEOUT.AsToken()}");
			Add($"{nameof(VERBOSITY_NUGET)} = {VERBOSITY_NUGET.AsToken()}");
			Add($"{nameof(VERBOSITY_DOTNET)} = {VERBOSITY_DOTNET.AsToken()}");
			Add($"{nameof(VERSION)} = {VERSION.AsToken()}");
			Add($"{nameof(VERSION_SUFFIX_DOTNET)} = {VERSION_SUFFIX_DOTNET.AsToken()}");
			Add($"{nameof(VERSION_SUFFIX_NUGET)} = {VERSION_SUFFIX_NUGET.AsToken()}");
			Help.Add();
		}

		private static void OutputReplaceableTokenValues()
		{
			//Help.Add($"{nameof(AppSettings.ApiKey)} ({nameof(AppSettings.ApiKey).AsToken()}) = {HandleConfiguration.AppSettingsValues.ApiKey}");
			//Help.Add($"{nameof(AppSettings.BasePath)} ({nameof(AppSettings.BasePath).AsToken()}) = {HandleConfiguration.AppSettingsValues.BasePath}");
			//Help.Add($"{nameof(AppSettings.ConfigFile)} ({nameof(AppSettings.ConfigFile).AsToken()}) = {HandleConfiguration.AppSettingsValues.ConfigFile}");
			//Help.Add($"{nameof(AppSettings.VersionSuffixDotNet)} ({nameof(AppSettings.VersionSuffixDotNet).AsToken()}) = {HandleConfiguration.AppSettingsValues.VersionSuffixDotNet}");
			//Help.Add($"{nameof(AppSettings.Exclude)} ({nameof(AppSettings.Exclude).AsToken()}) = {HandleConfiguration.AppSettingsValues.Exclude}");
			//Help.Add($"{nameof(AppSettings.MinClientVersion)} ({nameof(AppSettings.MinClientVersion).AsToken()}) = {HandleConfiguration.AppSettingsValues.MinClientVersion}");
			//Help.Add($"{nameof(AppSettings.MSBuildPath)} ({nameof(AppSettings.MSBuildPath).AsToken()}) = {HandleConfiguration.AppSettingsValues.MSBuildPath}");
			//Help.Add($"{nameof(AppSettings.MSBuildVersion)} ({nameof(AppSettings.MSBuildVersion).AsToken()}) = {HandleConfiguration.AppSettingsValues.MSBuildVersion}");
			//Help.Add($"{nameof(AppSettings.VersionSuffixNuGet)} ({nameof(AppSettings.VersionSuffixNuGet).AsToken()}) = {HandleConfiguration.AppSettingsValues.VersionSuffixNuGet}");
			//Help.Add($"{nameof(AppSettings.Properties)} ({nameof(AppSettings.Properties).AsToken()}) = {HandleConfiguration.AppSettingsValues.Properties}");
			//Help.Add($"{nameof(AppSettings.Root)} ({nameof(AppSettings.Root).AsToken()}) = {HandleConfiguration.AppSettingsValues.Root}");
			//Help.Add($"{nameof(AppSettings.RuntimeIdentifier)} ({nameof(AppSettings.RuntimeIdentifier).AsToken()}) = {HandleConfiguration.AppSettingsValues.RuntimeIdentifier}");
			//Help.Add($"{nameof(AppSettings.Timeout)} ({nameof(AppSettings.Timeout).AsToken()}) = {HandleConfiguration.AppSettingsValues.Timeout}");
			//Help.Add("\n***** Replaceable Tokens calculated internally\n");
			//Help.Add($"{nameof(API_KEY)} ({API_KEY.AsToken()}) = {TokenSetContainer.ApiKey}");
			//Help.Add($"{nameof(ASSEMBLY_PATH)} ({ASSEMBLY_PATH.AsToken()}) = {TokenSetContainer.AssemblyPath}");
			//Help.Add($"{nameof(VERBOSITY_DOTNET)} ({VERBOSITY_DOTNET.AsToken()}) = {TokenSetContainer.VerbosityDotNet}");
			//Help.Add($"{nameof(VERBOSITY_NUGET)} ({VERBOSITY_NUGET.AsToken()}) = {TokenSetContainer.VerbosityNuGet}");
			//Help.Add($"{nameof(OUTPUT_PACKAGE_TO)} ({OUTPUT_PACKAGE_TO.AsToken()}) = {TokenSetContainer.PackagePath}");
			//Help.Add($"{nameof(PACKAGE_ID)} ({PACKAGE_ID.AsToken()}) = {TokenSetContainer.PackageId}");
			//Help.Add($"{nameof(PACKAGE_VERSION)} ({PACKAGE_VERSION.AsToken()}) = {TokenSetContainer.PackageVersion}");
			//Help.Add($"{nameof(SOURCE)} ({SOURCE.AsToken()}) = {TokenSetContainer.Source}");
			//Help.Add($"{nameof(SYMBOL_SOURCE)} ({SYMBOL_SOURCE.AsToken()}) = {TokenSetContainer.SymbolSource}");
			//Help.Add($"{nameof(SYMBOL_API_KEY)} ({SYMBOL_API_KEY.AsToken()}) = {TokenSetContainer.SymbolApiKey}");
			//Help.Add($"{nameof(PROJECT_PATH)} ({PROJECT_PATH.AsToken()}) = {TokenSetContainer.ProjectPath}");

			Add($"{nameof(API_KEY)} = {API_KEY.AsToken()} = {TokenSetContainer.ApiKey}");
			Add($"{nameof(ASSEMBLY_PATH)} = {ASSEMBLY_PATH.AsToken()} = {TokenSetContainer.AssemblyPath}");
			Add($"{nameof(BASE_PATH)} = {BASE_PATH.AsToken()} = {TokenSetContainer.BasePath}");
			Add($"{nameof(CONFIG_FILE)} = {CONFIG_FILE.AsToken()} = {TokenSetContainer.ConfigFile}");
			Add($"{nameof(CONFIGURATION_NAME)} = {CONFIGURATION_NAME.AsToken()} = {TokenSetContainer.ConfigurationName}");
			Add($"{nameof(EXCLUDE)} = {EXCLUDE.AsToken()} = {TokenSetContainer.Exclude}");
			Add($"{nameof(MIN_CLIENT_VERSION)} = {MIN_CLIENT_VERSION.AsToken()} = {TokenSetContainer.MinClientVersion}");
			Add($"{nameof(MS_BUILD_PATH)} = {MS_BUILD_PATH.AsToken()} = {TokenSetContainer.MSBuildPath}");
			Add($"{nameof(MS_BUILD_VERSION)} = {MS_BUILD_VERSION.AsToken()} = {TokenSetContainer.MSBuildVersion}");
			Add($"{nameof(NUSPEC_FILE_PATH)} = {NUSPEC_FILE_PATH.AsToken()} = {TokenSetContainer.NuSpecFilePath}");
			Add($"{nameof(OUTPUT_PACKAGE_TO)} = {OUTPUT_PACKAGE_TO.AsToken()} = {TokenSetContainer.PackagePath}");
			Add($"{nameof(PACKAGE_ID)} = {PACKAGE_ID.AsToken()} = {TokenSetContainer.PackageName}");
			Add($"{nameof(PACKAGE_NAME)} = {PACKAGE_NAME.AsToken()} = {TokenSetContainer.PackageName}");
			Add($"{nameof(PACKAGE_PATH)} = {PACKAGE_PATH.AsToken()} = {TokenSetContainer.PackagePath}");
			Add($"{nameof(PACKAGE_VERSION)} = {PACKAGE_VERSION.AsToken()} = {TokenSetContainer.PackageVersion}");
			Add($"{nameof(PROJECT_PATH)} = {PROJECT_PATH.AsToken()} = {TokenSetContainer.ProjectPath}");
			Add($"{nameof(PROPERTIES)} = {PROPERTIES.AsToken()} = {TokenSetContainer.Properties}");
			Add($"{nameof(ROOT)} = {ROOT.AsToken()} = {TokenSetContainer.Root}");  // Equals Package Path for dotnet.exe
			Add($"{nameof(RUNTIME_IDENTIFIER)} = {RUNTIME_IDENTIFIER.AsToken()} = {TokenSetContainer.RuntimeIdentifier}");
			Add($"{nameof(SOURCE)} = {SOURCE.AsToken()} = {TokenSetContainer.Source}");
			Add($"{nameof(SYMBOL_SOURCE)} = {SYMBOL_SOURCE.AsToken()} = {TokenSetContainer.SymbolSource}");
			Add($"{nameof(SYMBOL_API_KEY)} = {SYMBOL_API_KEY.AsToken()} = {TokenSetContainer.SymbolApiKey}");
			Add($"{nameof(TIMEOUT)} = {TIMEOUT.AsToken()} = {TokenSetContainer.Timeout}");
			Add($"{nameof(VERBOSITY_DOTNET)} = {VERBOSITY_DOTNET.AsToken()} = {TokenSetContainer.VerbosityDotNet}");
			Add($"{nameof(VERBOSITY_NUGET)} = {VERBOSITY_NUGET.AsToken()} = {TokenSetContainer.VerbosityNuGet}");
			Add($"{nameof(VERSION)} = {VERSION.AsToken()} = {TokenSetContainer.PackageVersion}");
			Add($"{nameof(VERSION_SUFFIX_DOTNET)} = {VERSION_SUFFIX_DOTNET.AsToken()} = {TokenSetContainer.VersionSuffixDotNet}");
			Add($"{nameof(VERSION_SUFFIX_NUGET)} = {VERSION_SUFFIX_NUGET.AsToken()} = {TokenSetContainer.VersionSuffixNuGet}");
		}

		private static void OutputAppSettings()
		{
			Add("***** App.config settings\n");
			Add($"{nameof(AppSettings.UseConfigOverride)} = {HandleConfiguration.AppSettingsValues.UseConfigOverride}");
			Add($"{nameof(AppSettings.ConfigOverrideDir)} = {HandleConfiguration.AppSettingsValues.ConfigOverrideDir}");
			Add($"{nameof(AppSettings.InjectDefaultReleaseNotes)} = {HandleConfiguration.AppSettingsValues.InjectDefaultReleaseNotes}");
			Add($"{nameof(AppSettings.DefaultReleaseNotes)} = {HandleConfiguration.AppSettingsValues.DefaultReleaseNotes}");
			Add($"{nameof(AppSettings.SuspendHandling)} = {HandleConfiguration.AppSettingsValues.SuspendHandling}");
			Add($"{nameof(AppSettings.AllowOptionalAppConfig)} = {HandleConfiguration.AppSettingsValues.AllowOptionalAppConfig}");
			Add($"{nameof(AppSettings.NuGetDir)} = {HandleConfiguration.AppSettingsValues.NuGetDir}");
			Add($"{nameof(AppSettings.NuGetExeName)} = {HandleConfiguration.AppSettingsValues.NuGetExeName}");
			Add($"{nameof(AppSettings.DotNetDir)} = {HandleConfiguration.AppSettingsValues.DotNetDir}");
			Add($"{nameof(AppSettings.DotNetName)} = {HandleConfiguration.AppSettingsValues.DotNetName}");
			Add($"{nameof(AppSettings.DotNetVerb)} = {HandleConfiguration.AppSettingsValues.DotNetVerb}");
			Add($"{nameof(AppSettings.DefaultVerbosity)} = {HandleConfiguration.AppSettingsValues.DefaultVerbosity}");
			Add($"{nameof(AppSettings.VerbosityNuGet)} = {HandleConfiguration.AppSettingsValues.VerbosityNuGet}");
			Add($"{nameof(AppSettings.VerbosityDotNet)} = {HandleConfiguration.AppSettingsValues.VerbosityDotNet}");
			Add($"{nameof(AppSettings.ForceVersionOverride)} = {HandleConfiguration.AppSettingsValues.ForceVersionOverride}");
			Add($"{nameof(AppSettings.VersionOverride)} = {HandleConfiguration.AppSettingsValues.VersionOverride}");
			Add($"{nameof(AppSettings.VersionSuffixNuGet)} = {HandleConfiguration.AppSettingsValues.VersionSuffixNuGet}");
			Add($"{nameof(AppSettings.VersionSuffixDotNet)} = {HandleConfiguration.AppSettingsValues.VersionSuffixDotNet}");
			Add($"{nameof(AppSettings.PackageHomeDir)} = {HandleConfiguration.AppSettingsValues.PackageHomeDir}");
			Add($"{nameof(AppSettings.RequireReleaseNotesFile)} = {HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile}");
			Add($"{nameof(AppSettings.ReleaseNotesFileName)} = {HandleConfiguration.AppSettingsValues.ReleaseNotesFileName}");
			Add($"{nameof(AppSettings.RequireSummaryFile)} = {HandleConfiguration.AppSettingsValues.RequireSummaryFile}");
			Add($"{nameof(AppSettings.SummaryFileName)} = {HandleConfiguration.AppSettingsValues.SummaryFileName}");
			Add($"{nameof(AppSettings.DefaultDeleteFileName)} = {HandleConfiguration.AppSettingsValues.DefaultDeleteFileName}");
			Add($"{nameof(AppSettings.PushToDestination)} = {HandleConfiguration.AppSettingsValues.PushToDestination}");
			Add($"{nameof(AppSettings.UseNuSpecFileIfAvailable)} = {HandleConfiguration.AppSettingsValues.UseNuSpecFileIfAvailable}");
			Help.Add();
			OutputReplaceableTokens();
			OutputReplaceableTokenValues();
			string vLine =
				HandleConfiguration.AppSettingsValues.SuspendHandling
					? $"NuGetHandling suspended via {HandleConfiguration.ConfigFileName}"
					: "NuGetHandling proceeding (Not suspended).";
			Add(vLine);
			Help.Add();
		}

		private static void OutputNuSpecValues()
		{
			Add("***** NuSpec values\n");
			Add($"{nameof(NuGetNuSpecValues.ForceAuthors)} = {HandleConfiguration.NuGetNuSpecSettings.ForceAuthors}");
			Add($"{nameof(NuGetNuSpecValues.Authors)} = {HandleConfiguration.NuGetNuSpecSettings.Authors}");
			Add($"{nameof(NuGetNuSpecValues.ForceCopyright)} = {HandleConfiguration.NuGetNuSpecSettings.ForceCopyright}");
			Add($"{nameof(NuGetNuSpecValues.Copyright)} = {HandleConfiguration.NuGetNuSpecSettings.Copyright}");
			Add($"{nameof(NuGetNuSpecValues.ForceDescription)} = {HandleConfiguration.NuGetNuSpecSettings.ForceDescription}");
			Add($"{nameof(NuGetNuSpecValues.Description)} = {HandleConfiguration.NuGetNuSpecSettings.Description}");
			Add($"{nameof(NuGetNuSpecValues.ForceIconUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceIconUrl}");
			Add($"{nameof(NuGetNuSpecValues.IconUrl)} = {HandleConfiguration.NuGetNuSpecSettings.IconUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceLicenseUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceLicenseUrl}");
			Add($"{nameof(NuGetNuSpecValues.LicenseUrl)} = {HandleConfiguration.NuGetNuSpecSettings.LicenseUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceOwners)} = {HandleConfiguration.NuGetNuSpecSettings.ForceOwners}");
			Add($"{nameof(NuGetNuSpecValues.Owners)} = {HandleConfiguration.NuGetNuSpecSettings.Owners}");
			Add($"{nameof(NuGetNuSpecValues.ForceProjectUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceProjectUrl}");
			Add($"{nameof(NuGetNuSpecValues.ProjectUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ProjectUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceReleaseNotes)} = {HandleConfiguration.NuGetNuSpecSettings.ForceReleaseNotes}");
			Add($"{nameof(NuGetNuSpecValues.ReleaseNotes)} = {HandleConfiguration.NuGetNuSpecSettings.ReleaseNotes}");
			Add($"{nameof(NuGetNuSpecValues.ForceRequireLicenseAcceptance)} = {HandleConfiguration.NuGetNuSpecSettings.ForceRequireLicenseAcceptance}");
			Add($"{nameof(NuGetNuSpecValues.RequireLicenseAcceptance)} = {HandleConfiguration.NuGetNuSpecSettings.RequireLicenseAcceptance}");
			Add($"{nameof(NuGetNuSpecValues.ForceSummary)} = {HandleConfiguration.NuGetNuSpecSettings.ForceSummary}");
			Add($"{nameof(NuGetNuSpecValues.Summary)} = {HandleConfiguration.NuGetNuSpecSettings.Summary}");
			Add($"{nameof(NuGetNuSpecValues.ForceTags)} = {HandleConfiguration.NuGetNuSpecSettings.ForceTags}");
			Add($"{nameof(NuGetNuSpecValues.Tags)} = {HandleConfiguration.NuGetNuSpecSettings.Tags}");
			Add($"{nameof(NuGetNuSpecValues.ForceTitle)} = {HandleConfiguration.NuGetNuSpecSettings.ForceTitle}");
			Add($"{nameof(NuGetNuSpecValues.Title)} = {HandleConfiguration.NuGetNuSpecSettings.Title}");
			//Help.Add($"{nameof(NuGetNuSpecValues.Id)} = {HandleConfiguration.NuGetNuSpecSettings.Id}");
			//Help.Add($"{nameof(NuGetNuSpecValues.Version)} = {HandleConfiguration.NuGetNuSpecSettings.Version}");
			Help.Add();
		}

		private static void OutputNuGetRepositories()
		{
			Add("***** Repositories\n");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.NuGetRepos.Repositories)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Help.Add();
		}

		private static void OutputNuGetPushDestinations()
		{
			Add("***** Push Destinations\n");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.NuGetDestinations.PushDestinations)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Help.Add();
		}

		private static void OutputRepoInfo(NuGetRepository aRepository, int aIndent = 0)
		{
			string vIndent =
				(aIndent > 0)
					? "\n" + new String('\t', aIndent)
					: "\n";
			string vHasSource =
				aRepository.HasSource
					? "Yep, it has a source."
					: "Nope, no source here.";
			string vIsNuGetServer =
				aRepository.IsNuGetServer
					? "Yep, it's a NuGetServer"
					: "Nope, it's a share";
			string vSource = $"Source: {aRepository.Source}";
			string vSourceApiKey =
				aRepository.IsNuGetServer
					? $", Source API Key: {aRepository.SourceApiKey}.{vIndent}"
					: $", Source is not a NuGet Server, just a share.{vIndent}";
			bool vTest =
				aRepository.HasSymbolSource
					&& !String.IsNullOrWhiteSpace(aRepository.SymbolSource);
			string vHasSymbolsSource =
				aRepository.HasSymbolSource
					? $"Yep, has a symbol source.{vIndent}"
					: $"Nope, no symbols source here.{vIndent}";
			string vSymbolSource =
				vTest
					? $"Symbol Source: {aRepository.SymbolSource}, "
					: "No Symbol Source. ";
			vTest =
				aRepository.HasSymbolSource
					&& !String.IsNullOrWhiteSpace(aRepository.SymbolSourceApiKey);
			string vSymbolSourceApiKey =
				vTest
					? $"Symbols Source API Key: {aRepository.SymbolSourceApiKey}."
					: "No Symbols Source API Key.";
			Add($"{vIndent}{aRepository.RepositoryName}{vIndent}{vHasSource}{vIndent}{vIsNuGetServer}{vIndent}{vSource}{vSourceApiKey}{vHasSymbolsSource}{vSymbolSource}{vSymbolSourceApiKey}");
			Help.Add();
		}

		private static void OutputRepositoryInfo()
		{
			Add("***** Processed NuGet Repository Information");
			foreach (NuGetRepository vItem in HandleConfiguration.Repositories)
			{
				OutputRepoInfo(vItem);
			}
		}

		private static void OutputPushDestinationInfo()
		{
			Add("***** Processed NuGet Push Destination Information\n");
			string vLine = new String('*', 80);
			foreach (KeyValuePair<string, List<NuGetRepository>> vItem in HandleConfiguration.Destinations)
			{
				Add($"{vItem.Key}");
				foreach (NuGetRepository vInnerItem in vItem.Value)
				{
					OutputRepoInfo(vInnerItem, 1);
				}
				Add($"{vLine}\n");
			}
			Help.Add();
		}

		private static void OutputFullFrameworkCommands()
		{
			Add("Full Framework Commands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.FullFrameworkCommands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Help.Add();
		}

		private static void OutputStandardCommands()
		{
			Add("Standard 2.0 Cmomands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.Standard_2_0_Commands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Help.Add();
		}

		private static void OutputCoreCommands()
		{
			Add("Core 2.0 Cmomands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.Core_2_0_Commands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Help.Add();
		}

		private static void OutputFrameworkInfoForProject()
		{
			Add("***** Project Framework Info\n");
			string vFrameworkInfo =
				$"General Target Framework: {FrameworkInformation.Framework}, Version: {FrameworkInformation.FrameworkVersion}";
			Add($"{vFrameworkInfo}");
			string vLine =
				FrameworkInformation.GeneratePackageOnBuild
					? "Yes, Generate Package on Build."
					: "Nope, No package generated here.";
			Add($"Generate package on build: {vLine}");
			Add($"Project Name From .dll: {FrameworkInformation.ProjectName}");
			Add($"Package Version: {FrameworkInformation.PackageVersion}");
			Add
				($"AssemblyVersion: {FrameworkInformation.AssemblyVersion}");
			Add
				($"AssemblyFileVersion: {FrameworkInformation.AssemblyFileVersion}");
			Help.Add();
		}

		private static void OutputConfigInfo()
		{
			string vHeader = "NuGetHandler configuration settings";
			_HelpContent.Clear();
			Add($"\n{vHeader}\n{new String('-', vHeader.Length)}\n");
			OutputCommandLineSettings();
			OutputAppSettings();
			OutputNuSpecValues();
			OutputNuGetRepositories();
			OutputNuGetPushDestinations();
			OutputRepositoryInfo();
			OutputPushDestinationInfo();
			OutputFullFrameworkCommands();
			OutputStandardCommands();
			OutputCoreCommands();
			OutputFrameworkInfoForProject();
			Add("************************************************************");
			Add("************************************************************");
			Add("FIX THIS GADGET SUCH THAT EACH TIME A CALL TO UPDATE TOKEN SET");
			Add("IS MADE, A COPY OF THE VALUES IS PUT IN A LIST AND THAT LIST IS");
			Add("PRINTED OUT HERE!");
			Add("************************************************************");
			Add("************************************************************");
			Add(String.Empty);
		}

		private static void HelpOnHelp()
		{

		}

		public static void Add(string aLine = "")
		{
			_HelpContent.AppendLine(aLine);
		}

		public static void Header(string aHeaderContent = "")
		{
			// Compensate for the spaces imbeddeed in an 80 character line
			const int LINE_LEN = 78;
			int vLen = LINE_LEN - aHeaderContent.Length;
			string vPrefix = new String('*', vLen >> 1);
			string vSuffix =
				vLen.IsOdd()
					? vPrefix + "*"
					: vPrefix;
			Add($"\n{vPrefix} {aHeaderContent} {vSuffix}\n");
		}

		public static void Footer()
		{
			Add();
		}

		public static void GenerateHelp()
		{
			switch (ShowHelp)
			{
				case HelpSections.ALL:
				{
					OutputConfigInfo();
					break;
				}
				case HelpSections.HELP:
				{
					HelpOnHelp();
					break;
				}
				case HelpSections.COMMAND_LINE:
				{
					OutputCommandLineSettings();
					break;
				}
				case HelpSections.SWITCHES:
				{
					break;
				}
				case HelpSections.ENVIRONMENT:
				{
					break;
				}
				case HelpSections.CONFIG_INFO:
				{
					OutputConfigInfo();
					break;
				}
				default:
				{
					HelpOnHelp();
					break;
				}
			}
			Write(_HelpContent.ToString());
		}

		public static IHandleConfiguration HandleConfiguration { get; set; }
	}
}
