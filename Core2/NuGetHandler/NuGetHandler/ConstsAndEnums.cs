namespace NuGetHandler
{
	using System;
	using System.Text;
	using Infrastructure;
	using Microsoft.Extensions.DependencyInjection;
	using static Consts;

	public enum DotNetFramework
	{
		Unknown
		, Full
		, UniversalWindows
		, Standard_2_0
		, Core_2_0
		, Core_2_1
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

	public enum InternalVersionSelectorE
	{
		Unknown
		, AssemblyVersion
		, AssemblyFileVersion
		, PackageVersion
	}

	public delegate void ConfigureDIService(IServiceCollection aCollection);

	public static class Consts
	{
		public const StringComparison COMPARISON =
			StringComparison.InvariantCultureIgnoreCase;

		public const string KEY_START = "<";
		public const string KEY_END_START = KEY_START + "/";
		public const string KEY_END = ">";

		public const string LEFT_PAREN = "(";
		public const string RIGHT_PAREN = ")";

		public const string LEFT_BRACKET = "[";
		public const string RIGHT_BRACKET = "]";

		public const string RELEASE_NOTES_FILE_NAME = "ReleaseNotes";
		public const string SUMMARY_FILE_NAME = "Summary";
		public const string DOT = ".";
		public const string TXT_EXT = DOT + "txt";
		public const string EXE_EXT = DOT + "exe";
		public const string DLL_EXT = DOT + "dll";

		public const string DELETE_FILE_EXT = DOT + "del";
		public const string PACKAGE_EXT = DOT + "nupkg";
		public const string NUSPEC_EXT = DOT + "nuspec";
		public const string SEMICOLON = ";";
		public const string HTTP = "http";

		public const string DEFAULT_VERSION = "1.0.0.1";
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

		public const string INTERNAL_VERSION_SELECTOR = "InternalVersionSelector";
		public const string INTERNAL_VERSION_SELECTOR_SHORTCUT = "I";
		public const string INTERNAL_VERSION_SELECTOR_VARIANTS =
			INTERNAL_VERSION_SELECTOR + PIPE + INTERNAL_VERSION_SELECTOR_SHORTCUT;
		public const string INTERNAL_VERSION_SELECTOR_VALUES =
			"AssemblyVersion"
				+ PIPE
				+ "AssemblyFileVersion"
				+ PIPE
				+ "PackageVersion";

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

		public const string OVERRIDE_VERSION = "OverrideVersion";
		public const string OVERRIDE_SHORTCUT = "O";
		public const string OVERRIDE_VERSION_VARIANTS =
			OVERRIDE_VERSION + PIPE + OVERRIDE_SHORTCUT;

		public const string PROJECT_PATH = "ProjectPath";
		public const string PROJECT_PATH_SHORTCUT = "P";
		public const string PROJECT_PATH_VARIANTS =
			PROJECT_PATH + PIPE + PROJECT_PATH_SHORTCUT;

		public const string SHOW_ENVIRONMENT = "ShowEnvironment";
		public const string SHOW_ENVIRONMENT_SHORTCUT = "E";
		public const string SHOW_ENVIRONMENT_VARIANTS =
			SHOW_ENVIRONMENT + PIPE + SHOW_ENVIRONMENT_SHORTCUT;

		public const string HELP = "Help";
		public const string HELP_SHORTCUT_1 = "H";
		public const string HELP_SHORTCUT_2 = "?";
		public const string HELP_VARIANTS =
			HELP
				+ PIPE
				+ HELP_SHORTCUT_1
				+ PIPE
				+ HELP_SHORTCUT_2;

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
		public const string VERBOSITY_DOTNET = "VerbosityDotNet";
		public const string VERBOSITY_NUGET = "VerbosityNuGet";
		public const string VERSION = "Version";
		public const string VERSION_SUFFIX_DOTNET = "VersionSuffixDotNet";
		public const string VERSION_SUFFIX_NUGET = "VersionSuffixNuget";

		private static string Token(string aToken)
		{
			string vResult = $"{aToken} = {aToken.AsToken()}";
			return vResult;
		}

		public static string TokenWithValue(string aToken, string aValue)
		{
			string vResult = Token(aToken) + $" = {aValue}.";
			return vResult;
		}

		public new static string ToString()
		{
			StringBuilder vResult = new StringBuilder();
			vResult.AppendLine(Token(API_KEY));
			vResult.AppendLine(Token(ASSEMBLY_PATH));
			vResult.AppendLine(Token(BASE_PATH));
			vResult.AppendLine(Token(CONFIG_FILE));
			vResult.AppendLine(Token(CONFIGURATION_NAME));
			vResult.AppendLine(Token(EXCLUDE));
			vResult.AppendLine(Token(MIN_CLIENT_VERSION));
			vResult.AppendLine(Token(MS_BUILD_PATH));
			vResult.AppendLine(Token(MS_BUILD_VERSION));
			vResult.AppendLine(Token(NUSPEC_FILE_PATH));
			vResult.AppendLine(Token(OUTPUT_PACKAGE_TO));
			vResult.AppendLine(Token(PACKAGE_ID));
			vResult.AppendLine(Token(PACKAGE_NAME));
			vResult.AppendLine(Token(PACKAGE_PATH));
			vResult.AppendLine(Token(PACKAGE_VERSION));
			vResult.AppendLine(Token(PROJECT_PATH));
			vResult.AppendLine(Token(PROPERTIES));
			vResult.AppendLine(Token(ROOT));  // Equals Package Path for dotnet.exe
			vResult.AppendLine(Token(RUNTIME_IDENTIFIER));
			vResult.AppendLine(Token(SOURCE));
			vResult.AppendLine(Token(SYMBOL_SOURCE));
			vResult.AppendLine(Token(SYMBOL_API_KEY));
			vResult.AppendLine(Token(TIMEOUT));
			vResult.AppendLine(Token(VERBOSITY_DOTNET));
			vResult.AppendLine(Token(VERBOSITY_NUGET));
			vResult.AppendLine(Token(VERSION));
			vResult.AppendLine(Token(VERSION_SUFFIX_DOTNET));
			vResult.AppendLine(Token(VERSION_SUFFIX_NUGET));
			return vResult.ToString();
		}
	}

	public static class NuGetNuSpecKeys
	{
		public const string METADATA = "metadata";
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

		public const string DESCRIPTION_KEY_START =
			KEY_START + DESCRIPTION + KEY_END;
		public const string DESCRIPTION_KEY_END =
			KEY_END_START + DESCRIPTION + KEY_END;
		public const string DEFAULT_DESCRIPTION = "Package description";
		public const string METADATA_END = KEY_END_START + METADATA + KEY_END;
		public const string ICON_URL_KEY = KEY_START + ICON_URL + KEY_END;
		public const string LICENSE_URL_KEY = KEY_START + LICENSE_URL + KEY_END;
		public const string PROJECT_URL_KEY = KEY_START + PROJECT_URL + KEY_END;
		public const string ICON_URL_KEY_END = KEY_END_START + ICON_URL + KEY_END;
		public const string LICENSE_URL_KEY_END =
			KEY_END_START + LICENSE_URL + KEY_END;
		public const string PROJECT_URL_KEY_END =
			KEY_END_START + PROJECT_URL + KEY_END;
		public const string ICON_URL_OR_DELETE_THIS =
			"http://ICON_URL_HERE_OR_DELETE_THIS_LINE";
		public const string LICENSE_URL_OR_DELETE_THIS =
			"http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE";
		public const string PROJECT_URL_OR_DELETE_THIS =
			"http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE";
		public const string RELEASE_NOTES_KEY =
			KEY_START + RELEASE_NOTES + KEY_END;
		public const string RELEASE_NOTES_KEY_END =
			KEY_END_START + RELEASE_NOTES + KEY_END;
		public const string DEFAULT_RELEASE_NOTES =
			"Summary of changes made in this release of the package.";
		public const string TAGS_KEY_START = KEY_START + TAGS + KEY_END;
		public const string TAGS_KEY_END = KEY_END_START + TAGS + KEY_END;

		public const string DEFAULT_COPYRIGHT = COPYRIGHT + " "; // Insert the year
		public const string DEFAULT_TAGS = "Tag1 Tag2";
		public const string AUTHOR_KEY = KEY_START + AUTHORS + KEY_END;
		public const string OWNER_KEY = KEY_START + OWNERS + KEY_END;
		public const string DEFAULT_REQUIRE_LICENSE_ACCEPTANCE = "False";
		public const string REQUIRE_LICENSE_ACCEPTANCE_KEY =
			KEY_START + REQUIRE_LICENSE_ACCEPTANCE + KEY_END;
		public const string SUMMARY_KEY = KEY_START + SUMMARY + KEY_END;
		public const string SUMMARY_KEY_END = KEY_END_START + SUMMARY + KEY_END;

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
		private const string _APP_CONFIG = "app-config";
		public const string ALL = "all";
		public const string APP_CONFIG_SETTINGS = _APP_CONFIG + "-settings";
		public const string APP_CONFIG_NUSPEC_DOTNET =
			_APP_CONFIG + "-nuspec-dotnet";
		public const string APP_CONFIG_NUSPEC_NUGET = _APP_CONFIG + "-nuspec-nuget";
		public const string COMMAND_LINE = "command-line";
		public const string COMMAND_LINE_SWITCHES = COMMAND_LINE + "-switches";
		public const string COMMAND_LINE_NAMED_VALUES =
			COMMAND_LINE + "-named-values";
		public const string COMMAND_SEQUENCE = "command-sequence";
		public const string COMMAND_SEQUENCE_DOTNET_CORE =
			COMMAND_SEQUENCE + "-core";
		public const string COMMAND_SEQUENCE_NUGET = COMMAND_SEQUENCE + "-nuget";
		public const string COMMAND_SEQUENCE_DOTNET_STANDARD =
			COMMAND_SEQUENCE + "-standard";
		public const string CONFIG_INFO = "config-info";
		public const string ENVIRONMENT = "environment";
		public const string HELP = "help";
		public const string NUGET_PUSH = "nuget-push";
		public const string NUGET_PUSH_REPOS = NUGET_PUSH + "-repos";
		public const string NUGET_PUSH_DESTINATIONS =
			NUGET_PUSH + "-destinations";
		public const string POSTBUILD = "post-build";
		public const string SETUP = "setup";
		public const string SUMMARY = "summary";
		public const string TOKENS = "tokens";

		public new static string ToString()
		{
			const bool INSERT_NEW_LINE = false;
			StringBuilder vResult = new StringBuilder();
			vResult.AppendLine(ALL.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(_APP_CONFIG.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(APP_CONFIG_SETTINGS.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(APP_CONFIG_NUSPEC_DOTNET.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(APP_CONFIG_NUSPEC_NUGET.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(COMMAND_SEQUENCE.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine
				(COMMAND_SEQUENCE_DOTNET_CORE.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine
				(COMMAND_SEQUENCE_DOTNET_STANDARD.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(COMMAND_SEQUENCE_NUGET.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(COMMAND_LINE.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(CONFIG_INFO.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(ENVIRONMENT.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(HELP.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(NUGET_PUSH.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(NUGET_PUSH_REPOS.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(NUGET_PUSH_DESTINATIONS.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(POSTBUILD.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(SETUP.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(COMMAND_LINE_SWITCHES.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(SUMMARY.PadItLeft(INSERT_NEW_LINE));
			vResult.AppendLine(TOKENS.PadItLeft(INSERT_NEW_LINE));
			return vResult.ToString();
		}

	}
}