namespace NuGetHandler
{
	using Microsoft.Extensions.DependencyInjection;

	public enum KeyValueMode
	{
		Strict
		, Greedy
		, Expand
	}

	public enum DotNetFramework
	{
		Unknown
		, Full
		, Standard
		, Core
	}

	public enum VerbosityE
	{
		Unknown
		, Quiet
		, Normal
		, Detailed
	}

	public enum NuGetCommand
	{
		Unknown
		, NuGetSpec
		, NuGetPack
		, NuGetPush
		, NuGetAdd
		, NuGetDelete
		, DotNetPack
		, DotNetNuGetPush
		, DotNetNuGetDelete
	}

	public delegate void ConfigureDIService(IServiceCollection aCollection);

	public static class Consts
	{
		public const string RELEASE_NOTES_FILE_NAME = "ReleaseNotes";
		public const string SUMMARY_FILE_NAME = "Summary";
		public const string TXT_EXT = ".txt";
		public const string EXE_EXT = ".exe";
		public const string DLL_EXT = ".dll";

		public const string DELETE_FILE_EXT = ".del";
		public const string PACKAGE_EXT = ".nupkg";
		public const string NUSPEC_EXT = ".nuspec";
		public const string SEMICOLON = ";";
		public const string HTTP = "http";
	}

	public static class CommandLineSwitches
	{
		public const string PIPE = "|";

		public const string CONFIGURATION_NAME = "ConfigurationName";
		public const string CONFIGURATION_SHORTCUT = "C";
		public const string CONFIGURATION_NAME_VARIANTS =
			CONFIGURATION_NAME + PIPE + CONFIGURATION_SHORTCUT;

		public const string DELETE_FROM_NUGET = "DeleteFromNuGet";
		public const string DELETE_FROM_SHORTCUT = "D";
		public const string DELETE_FROM_NUGET_VARIANTS =
			DELETE_FROM_NUGET + PIPE + DELETE_FROM_SHORTCUT;

		public const string MERGE_DELETIONS = "MergeDeletions";
		public const string MERGE_DELETIONS_SHORTCUT = "M";
		public const string MERGE_DELETIONS_VARIANTS =
			MERGE_DELETIONS + PIPE + MERGE_DELETIONS_SHORTCUT;

		public const string NO_OP = "NoOp";
		public const string NO_OP_SHORTCUT = "N";
		public const string NO_OP_VARIANTS =
			NO_OP + PIPE + NO_OP_SHORTCUT;

		public const string NO_SPAWN = "NoSpawn";
		public const string NO_SPAWN_SHORTCUT = "Q";
		public const string NO_SPAWN_VARIANTS =
			NO_SPAWN + PIPE + NO_SPAWN_SHORTCUT;

		public const string PROJECT_PATH = "ProjectPath";
		public const string PROJECT_PATH_SHORTCUT = "P";
		public const string PROJECT_PATH_VARIANTS =
			PROJECT_PATH + PIPE + PROJECT_PATH_SHORTCUT;

		public const string SHOW_ENVIRONMENT = "ShowEnvironment";
		public const string SHOW_ENVIRONMENT_SHORTCUT = "E";
		public const string SHOW_ENVIRONMENT_VARIANTS =
			SHOW_ENVIRONMENT + PIPE + SHOW_ENVIRONMENT_SHORTCUT;

		public const string SHOW_HELP = "ShowHelp";
		public const string SHOW_HELP_SHORTCUT_1 = "Help";
		public const string SHOW_HELP_SHORTCUT_2 = "H";
		public const string SHOW_HELP_SHORTCUT_3 = "?";
		public const string SHOW_HELP_VARIANTS =
			SHOW_HELP
				+ PIPE
				+ SHOW_HELP_SHORTCUT_1
				+ PIPE
				+ SHOW_HELP_SHORTCUT_2
				+ PIPE
				+ SHOW_HELP_SHORTCUT_3;

		public const string SOLUTION_PATH = "SolutionPath";
		public const string SOLUTION_PATH_SHORTCUT = "S";
		public const string SOLUTION_PATH_VARIANTS =
			SOLUTION_PATH + PIPE + SOLUTION_PATH_SHORTCUT;

		public const string TARGET_PATH = "TargetPath";
		public const string TARGET_PATH_SHORTCUT = "T";
		public const string TARGET_PATH_VARIANTS =
			TARGET_PATH + PIPE + TARGET_PATH_SHORTCUT;

		public const string VERBOSITY = "Verbosity";
		public const string VERBOSITY_SHORTCUT = "V";
		public const string VERBOSITY_VARIANTS =
			VERBOSITY + PIPE + VERBOSITY_SHORTCUT;
		public const string VERBOSITY_VALUES = "quiet|normal|detailed";

		public const string PUSH_TO_DESTINATION = "PushToDestination";
		public const string PUSH_TO_DESTINATION_SHORTCUT = "U";
		public const string PUSH_TO_DESTINATION_VARIANTS =
			PUSH_TO_DESTINATION + PIPE + PUSH_TO_DESTINATION_SHORTCUT;

		public const string WAIT = "Wait";
		public const string WAIT_SHORTCUT = "W";
		public const string WAIT_VARIANTS = WAIT + PIPE + WAIT_SHORTCUT;

	}

	public static class NuGetCommands
	{
		public const string SPEC = "spec";
		public const string PACK = "pack";
		public const string PUSH = "push";
		public const string ADD = "add";
		public const string DELETE = "delete";
	}

	public static class DotNetCommands
	{
		public const string PACK = "pack";
		public const string PUSH = "push";
		public const string DELETE = "delete";
	}

	public static class ReplaceableTokens // These tokens are found in the command line definitions of each command.
	{
		public const string API_KEY = "ApiKey";
		public const string ASSEMBLY_PATH = "AssemblyPath";
		public const string BASE_PATH = "BasePath";
		public const string CONFIG_FILE = "ConfigFile";
		public const string CONFIGURATION_NAME = "ConfigurationName";
		public const string EXCLUDE = "Exclude";
		public const string MIN_CLIENT_VERSION = "MinClientVersion";
		public const string MS_BUILD_PATH = "MSBuildPath";
		public const string MS_BUILD_VERSION = "MSBuildVersion";
		public const string NUSPEC_FILE_PATH = "NuSpecFilePath";
		public const string OUTPUT_PACKAGE_TO = "OutputPackageTo";
		public const string PACKAGE_ID = "PackageId";
		public const string PACKAGE_NAME = "PackageName";
		public const string PACKAGE_PATH = "PackagePath";
		public const string PACKAGE_VERSION = "PackageVersion";
		public const string PROJECT_PATH = "ProjectPath";
		public const string PROPERTIES = "Properties";
		public const string ROOT = "Root";  // Equals Package Path for dotnet.exe
		public const string RUNTIME_IDENTIFIER = "RuntimeIdentifier";
		public const string SOURCE = "Source";
		public const string SYMBOL_SOURCE = "SymbolSource";
		public const string SYMBOL_API_KEY = "SymbolApiKey";
		public const string TIMEOUT = "Timeout";
		public const string VERBOSITY_NUGET = "VerbosityNuGet";
		public const string VERBOSITY_DOTNET = "VerbosityDotNet";
		public const string VERSION = "Version";
		public const string VERSION_SUFFIX_DOTNET = "VersionSuffixDotNet";
		public const string VERSION_SUFFIX_NUGET = "VersionSuffixNuget";

	}

	public static class NuGetNuSpecKeys
	{
		public const string METADATA_END = "</metadata>";
		public const string ICON_URL_KEY = "<" + ICON_URL + ">";
		public const string LICENSE_URL_KEY = "<" + LICENSE_URL + ">";
		public const string PROJECT_URL_KEY = "<" + PROJECT_URL + ">";
		public const string ICON_URL_KEY_END = "</" + ICON_URL + ">";
		public const string LICENSE_URL_KEY_END = "</" + LICENSE_URL + ">";
		public const string PROJECT_URL_KEY_END = "</" + PROJECT_URL + ">";
		public const string DELETE_THIS =
			"http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE";
		public const string DEFAULT_RELEASE_NOTES =
			"Summary of changes made in this release of the package.";
		public const string DEFAULT_COPYRIGHT = COPYRIGHT + " "; // Insert the year
		public const string DEFAULT_TAGS = "Tag1 Tag2";
		public const string AUTHOR_KEY = "<" + AUTHORS + ">";
		public const string OWNER_KEY = "<" + OWNERS + ">";
		public const string DEFAULT_REQUIRE_LICENSE_ACCEPTANCE = "False";
		public const string REQUIRE_LICENSE_ACCEPTANCE_KEY =
			"<" + REQUIRE_LICENSE_ACCEPTANCE + ">";
		public const string SUMMARY_KEY = "<" + SUMMARY + ">";
		public const string SUMMARY_KEY_END = "</" + SUMMARY + ">";

		public const string ID = "id";
		public const string VERSION = "version";
		public const string TITLE = "title";
		public const string AUTHORS = "authors";
		public const string OWNERS = "owners";
		public const string LICENSE_URL = "licenseUrl";
		public const string PROJECT_URL = "projectUrl";
		public const string ICON_URL = "iconUrl";
		public const string REQUIRE_LICENSE_ACCEPTANCE = "requireLicenseAcceptance";
		public const string SUMMARY = "Summary";
		public const string DESCRIPTION = "description";
		public const string RELEASE_NOTES = "releaseNotes";
		public const string COPYRIGHT = "copyright";
		public const string TAGS = "tags";

	}

	public static class DotNetCSProjKeys
	{
		public const string ASSEMBLY_VERSION = "AssemblyVersion";
		public const string AUTHORS = "Authors";
		public const string COMPANY = "Company";
		public const string COPYRIGHT = "Copyright";
		public const string DESCRIPTION = "Description";
		public const string FILE_VERSION = "FileVersion";
		public const string PACKAGE_ICON_URL = "PackageIconUrl";
		public const string PACKAGE_LICENSE_URL = "PackageLicenseUrl";
		public const string PACKAGE_PROJECT_URL = "PackageProjectUrl";
		public const string PACKAGE_RELEASE_NOTES = "PackageReleaseNotes";
		public const string PACKAGE_REQUIRE_LICENSE_ACCEPTANCE =
			"PackageRequireLicenseAcceptance";
		public const string PACKAGE_TAGS = "PackageTags";
		public const string PRODUCT = "Product";
		public const string VERSION = "Version";

	}

	public static class HelpSections
	{
		public const string ALL = "all";
		public const string HELP = "help";
		public const string HELP_ON_HELP = "helponhelp";
		public const string COMMAND_LINE = "commandLine";
		public const string SWITCHES = "switches";
		public const string ENVIRONMENT = "environment";
		public const string CONFIG_INFO = "ConfigInfo";
		public const string TOKENS = "tokens";

	}
}