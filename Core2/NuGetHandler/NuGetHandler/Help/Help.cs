namespace NuGetHandler.Help
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Text;
	using AppConfigHandling;
	using ConfigurationHandler;
	using Infrastructure;
	using ProjectFileProcessing;
	using static System.Console;
	using static HelpAppConfig;
	using static HelpCommandLine;
	using static HelpCommandSequence;
	using static HelpNuGetRepos;
	using static HelpNuSpecDotNet;
	using static HelpNuSpecNuGet;
	using static HelpReplaceableTokens;
	using static HelpSections;
	using static HelpSetup;
	using static HelpSummary;

	public static class Help
	{
		private static readonly StringBuilder _HelpContent = new StringBuilder();

		private static void HelpOnHelp()
		{
			Add("The command syntax for requesting additional help with NuGetHandler is:");
			Add();
			Add("  NuGetHandler -h <subject>");
			Add();
			Add("where <subject> is the desired section of help to be acquired. Given that the");
			Add("length of some of the subject matter is quite long, it is recommended that the");
			Add("output of the help system be piped to a text file for later review.");
			Add();
			Add("Some subjects will also yield the actual settings as derived from values");
			Add("present on the command line and from the various app.*.config files.");
			Add(@"To cause those values to be displayed, a verbosity value of ""detailed""");
			Add("to the command line e.g. -v detailed");
			Add();
			Add("The list of subjects follows:");
			Add(HelpSections.ToString());
			SectionBreak("Content of help sections");
			Add($"{"<none>".PadItRight()}The program summary plus this help description.");
			Add();
			Add($"{ALL.PadItRight()}Generate the entire help content (best");
			Add($"{String.Empty.PadItLeft()}redirected to a file for further perusal).");
			Add();
			Add($"{APP_CONFIG_SETTINGS.PadItRight()}Just the information as found in the appSettings");
			Add($"{String.Empty.PadItLeft()}portion of the app.config file and its possible");
			Add($"{String.Empty.PadItLeft()}app.optional.config files.");
			Add();
			Add($"{APP_CONFIG_NUSPEC_DOTNET.PadItRight()}Just the information as found in the");
			Add($"{String.Empty.PadItLeft()}DotNetNuSpecSettings section of the app.config");
			Add($"{String.Empty.PadItLeft()}file and its possible app.optional.config files.");
			Add();
			Add($"{APP_CONFIG_NUSPEC_NUGET.PadItRight()}");
			Add($"{String.Empty.PadItLeft()}NuGetNuSpecSettings section of the app.config file");
			Add($"{String.Empty.PadItLeft()}and its possible app.optional.config files.");
			Add();
			Add($"{COMMAND_LINE.PadItRight()}Help on the command line switches and named value");
			Add($"{String.Empty.PadItLeft()}pairs plus the command line introduction.");
			Add();
			Add($"{COMMAND_LINE_SWITCHES.PadItRight()}Help on just the command line switches.");
			Add();
			Add($"{COMMAND_LINE_NAMED_VALUES.PadItRight()}Help on just the command line named values.");
			Add();
			Add($"{COMMAND_SEQUENCE.PadItRight()}Complete list of commands and their respective");
			Add($"{String.Empty.PadItLeft()}command line parameters as supported in");
			Add($"{String.Empty.PadItLeft()}NuGetHandler");
			Add();
			Add($"{COMMAND_SEQUENCE_DOTNET_CORE.PadItRight()}Just the command sequence that supports .net Core");
			Add($"{String.Empty.PadItLeft()}projects (dotnet.exe)");
			Add();
			Add($"{COMMAND_SEQUENCE_NUGET.PadItRight()}Just the command sequence that supports full");
			Add($"{String.Empty.PadItLeft()}framework projects (nuget.exe)");
			Add();
			Add($"{COMMAND_SEQUENCE_DOTNET_STANDARD.PadItRight()}Just the comnmand sequence that supports .net");
			Add($"{String.Empty.PadItLeft()}standard projects (dotnet.exe)");
			Add();
			Add($"{CONFIG_INFO.PadItRight()}Display the settings for the final app.config,");
			Add($"{String.Empty.PadItLeft()}command line calculated values and other");
			Add($"{String.Empty.PadItLeft()}information as determined by the program when");
			Add($"{String.Empty.PadItLeft()}executed. If a project was designated,");
			Add($"{String.Empty.PadItLeft()}the processed project information will be");
			Add($"{String.Empty.PadItLeft()}displayed.");
			Add();
			Add($"{ENVIRONMENT.PadItRight()}Help and content for the program environment.");
			Add();
			Add($"{HELP.PadItRight()}This help description.");
			Add();
			Add($"{NUGET_PUSH.PadItRight()}The combined information concerning the locations");
			Add($"{String.Empty.PadItLeft()}(Push Repositories) of the various nuget servers");
			Add($"{String.Empty.PadItLeft()}and the corresponding destinations (Push");
			Add($"{String.Empty.PadItLeft()}Destinations) that match the repositories.");
			Add();
			Add($"{NUGET_PUSH_REPOS.PadItRight()}Just the information on the nuget server");
			Add($"{String.Empty.PadItLeft()}repositories");
			Add();
			Add($"{NUGET_PUSH_DESTINATIONS.PadItRight()}Just the information on the nuget server");
			Add($"{String.Empty.PadItLeft()}repository destinations.");
			Add();
			Add($"{SETUP.PadItRight()}How to prepare to install the NuGetHandler");
			Add($"{String.Empty.PadItLeft()}program.");
			Add();
			Add($"{SUMMARY.PadItRight()}Just the summary information for the NuGetHandler");
			Add($"{String.Empty.PadItLeft()}program.");
			Add();
			Add($"{TOKENS.PadItRight()}Help on the replaceable tokens as found in the");
			Add($"{String.Empty.PadItLeft()}app.config file.");
			Add();
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
			Add();
		}

		private static void OutputAllinfo()
		{
			OutputSummary();
			SectionBreak("Help on Help");
			HelpOnHelp();
			SectionBreak(" Setup");
			OutputSetup();
			SectionBreak("App Config Info");
			OutputAppConfig();
			SectionBreak("NuSpec DotNet");
			OutputNuSpecDotNet();
			SectionBreak("NuSpec NuGet");
			OutputNuSpecNuGet();
			SectionBreak("Command Line");
			OutputCommandLine();
			SectionBreak("Command Line Sequences");
			OutputCommandSequence();
			SectionBreak("NuGet Server");
			OutputNuGetPush();
			SectionBreak("Replaceable Tokens");
			OutputReplaceableTokens();
			SectionBreak("Project Info");
			OutputFrameworkInfoForProject();
			SectionBreak("Additional Instructions");
			Add("************************************************************");
			Add("************************************************************");
			Add("FIX THIS GADGET SUCH THAT EACH TIME A CALL TO UPDATE TOKEN SET");
			Add("IS MADE, A COPY OF THE VALUES IS PUT IN A LIST AND THAT LIST IS");
			Add("PRINTED OUT HERE!");
			Add("************************************************************");
			Add("************************************************************");
		}

		private static void AddSeparatorLine()
		{
			const int STANDARD_LINE_LENGTH = 80;
			string vLine = new String('*', STANDARD_LINE_LENGTH);
			Add(vLine);
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
			Add($"{vPrefix} {aHeaderContent} {vSuffix}");
		}

		public static void HeaderWithInfo(string aHeaderContent = "")
		{
			Header(aHeaderContent);
			Assembly vAssembly = typeof(Program).Assembly;
			string vFileVersion =
				vAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version
					?? "No File Version";
			string vPackageVersion =
				FileVersionInfo.GetVersionInfo(vAssembly.Location).ProductVersion
					?? "No Package Version";
			string vAssemblyVersion = vAssembly.GetName().Version.ToString();
			string[] vNamePieces = Assembly.GetExecutingAssembly().FullName.Split(',');
			string vName = vNamePieces[0];
			Add();
			Add(vName);
			Add();
			Add($"File Version: {vFileVersion}");
			Add();
			Add($"Package Version: {vPackageVersion}");
			Add();
			Add($"Assembly Version: {vAssemblyVersion}");
			Add();
			AddSeparatorLine();
			Add();
		}

		public static void SectionBreak(string aHeaderContent = "")
		{
			if (String.IsNullOrWhiteSpace(aHeaderContent))
			{
				Add();
				AddSeparatorLine();
			}
			else
			{
				Add();
				Header(aHeaderContent);
			}
			AddSeparatorLine();
			Add();
		}

		public static void Footer()
		{
			Add();
			AddSeparatorLine();
		}

		public static void GenerateHelp()
		{
			if (CommandLineSettings.ShowEnvironment)
			{
				OutputCommandLineSettings();
			}
			switch (CommandLineSettings.Help)
			{
				case ALL:
				{
					HeaderWithInfo("NuGetHandler Help");
					OutputAllinfo();
					Footer();
					break;
				}
				case APP_CONFIG_SETTINGS:
				{
					HeaderWithInfo("App config settihgs");
					OutputAppConfig();
					Footer();
					break;
				}
				case APP_CONFIG_NUSPEC_DOTNET:
				{
					HeaderWithInfo("App config NuSpec DotNet");
					OutputNuSpecDotNet();
					Footer();
					break;
				}
				case APP_CONFIG_NUSPEC_NUGET:
				{
					HeaderWithInfo("App config NuSpec NuGet");
					OutputNuSpecNuGet();
					Footer();
					break;
				}
				case COMMAND_LINE:
				{
					HeaderWithInfo("Command Line");
					OutputCommandLine();
					Footer();
					break;
				}
				case COMMAND_LINE_SWITCHES:
				{
					HeaderWithInfo("Command Line Switches");
					OutputCommandLineSwitches();
					Footer();
					break;
				}
				case COMMAND_LINE_NAMED_VALUES:
				{
					HeaderWithInfo("Command Line Named Values");
					OutputCommandLineNamedValues();
					Footer();
					break;
				}
				case COMMAND_SEQUENCE:
				{
					HeaderWithInfo("Command Sequence");
					OutputCommandSequence();
					Footer();
					break;
				}
				case COMMAND_SEQUENCE_DOTNET_CORE:
				{
					HeaderWithInfo("Command Sequence DotNet");
					OutputCommandSequenceDotNetCore();
					Footer();
					break;
				}
				case COMMAND_SEQUENCE_DOTNET_STANDARD:
				{
					HeaderWithInfo("Command Sequence DotNet Standard");
					OutputCommandSequenceDotNetStandard();
					Footer();
					break;
				}
				case COMMAND_SEQUENCE_NUGET:
				{
					HeaderWithInfo("Command Sequence NuGet");
					OutputCommandSequenceFullFramework();
					Footer();
					break;
				}
				case CONFIG_INFO:
				{
					HeaderWithInfo("Configuration Information");
					OutputAllinfo();
					Footer();
					break;
				}
				case ENVIRONMENT:
				{
					HeaderWithInfo("Environment");
					SectionBreak("App Config Values");
					OutputAppConfigValues();
					SectionBreak("Command Line Values");
					OutputCommandLineSettings();
					SectionBreak("NuGet Server Repository Info");
					OutputNuGetRepositoryInfo();
					OutputPushDestinationInfo();
					SectionBreak("NuSpec Info DotNet");
					OutputNuSpecDotNetValues();
					SectionBreak("NuSpec Info NuGet");
					OutputNuSpecNuGetValues();
					SectionBreak("Token Values");
					OutputReplaceableTokenValues();
					Footer();
					break;
				}
				case HELP:
				{
					HeaderWithInfo("Help");
					HelpOnHelp();
					Footer();
					break;
				}
				case NUGET_PUSH:
				{
					HeaderWithInfo("NuGet Push");
					OutputNuGetPush();
					Footer();
					break;
				}
				case NUGET_PUSH_DESTINATIONS:
				{
					HeaderWithInfo("NuGet Push Destinations");
					OutputNuGetPushDestinations();
					Footer();
					break;
				}
				case NUGET_PUSH_REPOS:
				{
					HeaderWithInfo("NuGet Push Repositories");
					OutputNuGetRepositories();
					Footer();
					break;
				}
				case SETUP:
				{
					HeaderWithInfo("Setup");
					OutputSetup();
					Footer();
					break;
				}
				case SUMMARY:
				{
					HeaderWithInfo("Summary");
					OutputSummary();
					Footer();
					break;
				}
				case TOKENS:
				{
					HeaderWithInfo("Tokens");
					OutputTokens();
					Footer();
					break;
				}
				default:
				{
					HeaderWithInfo("Help");
					HelpOnHelp();
					Footer();
					break;
				}
			}
			Write(_HelpContent.ToString());
		}

		public static IHandleConfiguration HandleConfiguration { get; set; }
	}
}
