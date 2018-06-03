﻿namespace NuGetHandler.Help
{
	using AppConfigHandling;
	using static Help;

	public static class HelpAppConfig
	{
		public static void OutputAppConfigValues()
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
			Add($"{nameof(AppSettings.DeleteNuSpecFileAfterProcessing)} = {HandleConfiguration.AppSettingsValues.DeleteNuSpecFileAfterProcessing}");
			Add();
			string vLine =
				HandleConfiguration.AppSettingsValues.SuspendHandling
					? $"NuGetHandling suspended via {HandleConfiguration.ConfigFileName}"
					: "NuGetHandling proceeding (Not suspended).";
			Add(vLine);
			Add();
		}

		public static void OutputAppConfig()
		{
			Add(@"appSettings");
			Add();
			Add(@"<add key=""SuspendHandling"" value=""false""/>");
			Add();
			Add(@"- When set to ""true"" causes the program to do nothing. It starts, reads the ");
			Add(@"  config file, notices this setting as set to ""true"" and quits.");
			Add();
			Add(@"<add key=""PushToDestination"" value=""HomeNew""/>");
			Add();
			Add(@"- The designated set of NuGet servers (repositories) the package will be");
			Add(@"  uploaded to (see elsewhere for an explanation of named repositories).");
			Add();
			Add(@"<add key=""AllowOptionalAppConfig"" value=""true""/>");
			Add();
			Add(@"- If set to ""true"", then the app.config as found in the NuGetHandler home");
			Add(@"  directory is only the first of a possible series of app.optional.config");
			Add(@"  files that will be processed when the program first starts. ");
			Add(@"  ");
			Add(@"  The initial App.config file will be processed and stored, then, if this ");
			Add(@"  setting is ""true"", the next location searched will be the solution directory.");
			Add(@"  The file sought is named App.optional.config and will be loaded next and its");
			Add(@"  values applied to those already stored from the initial App.config load. If");
			Add(@"  a third app.optional.config file is found in the project directory, it, too,");
			Add(@"  will be loaded and its values applied to the current configuration.");
			Add();
			Add(@"  Basically, as long as""AllowOptionalAppConfig"" is set to ""true"", the next");
			Add(@"  tier will be processed and applied.");
			Add(@"  ");
			Add(@"  This allows the system to have a ""general"" configuration, one tailored to a");
			Add(@"  solution and finally a configuration tailored to a particuolar project.");
			Add();
			Add(@"  It used to be possible to game the system in Full Framework by breaking the");
			Add(@"  AssemblyInfo.cs file into two pieces - those that are common to all projects");
			Add(@"  and those that are particular to a specific project. The nice part is that ");
			Add(@"  things like ""Version"" could be brought into the ""common"" section and entire");
			Add(@"  solutions or even multiple solutions all given the same version number by");
			Add(@"  adding the comnmon AssemblyInfo.cs file via a link vs adding a copy of the");
			Add(@"  file to the project. With the advent of Standard/Core, this is no longer");
			Add(@"  possible as the entire AssemblyInfo.cs file is now missing and its content");
			Add(@"  handled as a section within the .csproj file.");
			Add();
			Add(@"  To overcome this limitation, the user may opt to use the following two");
			Add(@"  configuration override commands to alter the configuration of NuGetHandler.");
			Add();
			Add(@"<add key=""UseConfigOverride"" value=""true""/>");
			Add();
			Add(@"- If set to ""true"", then an alternate app.optional.config file located in a");
			Add(@"  specified location will be used in place of the tiered app.optional.config");
			Add(@"  setting set as described elsewhere.");
			Add();
			Add(@"<add key=""ConfigOverrideDir"" value=""""/>");
			Add();
			Add(@"- If UseConfigOverride is set to ""true"", then the location of the ");
			Add(@"  app.optional.config file to be used will be in the directory specified by this");
			Add(@"  key. The value should be in the form of an actual directory, meaning that the");
			Add(@"  value should end in a trailing path delimiter character.");
			Add();
			Add(@"<add key=""ForceVersionOverride"" value=""true""/>");
			Add();
			Add(@"- If set to ""true"", then the version that is found in the AssemblyInfo.cs");
			Add(@"  file (Full Framework) or the version that is found in the .csproj file for");
			Add(@"  Standard/Core will be overridden with the value specifed.");
			Add();
			Add(@"  It should be noted that if this value is set to ""true"" it also overrides any");
			Add(@"  value found in the respective ""XXXNuSpec"" values sections as outlined ");
			Add(@"  elsewhere.");
			Add(@"  Each type of project can have its own version override if desired, however, if");
			Add(@"  this value is set to ""true"", then it overrides the overrides. IOW, this value");
			Add(@"  wins regardless of .NET environment.");
			Add();
			Add(@"<add key=""VersionOverride"" value=""<some 4 part version number>""/>");
			Add();
			Add(@"- If ForceVersionOverride is set to ""true"", then use the provided value in the");
			Add(@"  app.config file vice what is found in the respective project or the sections");
			Add(@"  definded elsewhere.");
			Add();
			Add(@"<add key=""UseDateBasedVersion"" value=""true""/>");
			Add();
			Add(@"- If UseDateBasedVersion is set to ""true"", then when creating a version to apply");
			Add("  to the project, use the following template:");
			Add();
			Add("    <Major>.<Current Year>.<MonthDay in mdd form>.<Build Number>");
			Add();
			Add("  The Major value will NOT be changed automatically. This MUST be set by the");
			Add("  developer. The Current year is, of course, the year when the project was");
			Add("  compiled. The MonthDay is the current month and day, the month being");
			Add("  NON-leading zero-based e.g. May is encoded as 5, not 05.");
			Add("  The build number is an every incrementing value either set by the developer");
			Add("  OR, if the AutoIncrementBuildNumber value is set to true, then NuGetHandler");
			Add("  will fetch the current value out, increment it, then use that value as the");
			Add("  Build Number.");
			Add();
			Add(@"<add key=""AutoIncrementBuildNumber"" value=""true"" />");
			Add();
			Add(@"- Presuming a version that is either SemVer 2.0 form (Major.Minor.Patch.Build)");
			Add("  or date based (Major.Year.MonthDay.Build), then setting the");
			Add("  AutoIncrementBuildNumber value to true will cause NuGetHandler to fetch out");
			Add("  the Build Number, increment it, then apply the result as the version when");
			Add("  the packageis created and pushed.");
			Add();
			Add(@"<add key=""NuGetDir"" value=""%APPDATA%\NuGet\"" /> ");
			Add();
			Add(@"- Location directory of the ""NuGet.exe"" program. Notice that the directory");
			Add(@"  entry ENDS WITH A PATH DELIMITER! This is REQUIRED.");
			Add();
			Add(@"<add key=""NuGetExeName"" value=""NuGet""/>");
			Add();
			Add(@"- Name of the NuGet.exe program. Notice that the name DOES NOT end with "".exe"".");
			Add();
			Add(@"<add key=""DotNetDir"" value=""%PROGRAMFILES%\dotnet""/>");
			Add();
			Add(@"-  The location directory of the ""dotnet.exe"" program. Notice that the directory");
			Add(@"  entry ENDS WITH A PATH DELIMITER! This is REQUIRED.");
			Add();
			Add(@"<add key=""DotNetName"" value=""dotnet""/>");
			Add();
			Add(@"-  Name of the ""dotnet.exe"" program. Notice that the name DOES NOT end with ");
			Add(@"  "".exe"".");
			Add();
			Add(@"<add key=""DotNetVerb"" value=""nuget""/>");
			Add();
			Add(@"- The ""dotnet.exe"" program is goofy when it comes to NuGet handling. The ");
			Add(@"  ""dotnet"" program uses ""pack"" directly e.g. dotnet pack ... whereas to ""push""");
			Add(@"  or ""delete"", the program uses the ""nuget"" verb e.g. ""dotnet nuget push ..."".");
			Add(@"  Therefore, this verb is used to add to the command line where appropriate.");
			Add(@"  See below for command line definitions and this will become clearer.");
			Add(@"  ");
			Add(@"<add key=""UseSimulator"" value=""false""/>");
			Add();
			Add(@"- Instead of actually calling the NuGet.exe or dotnet.exe program, call a");
			Add(@"  simulator instead. The solution for this application contains a simulator");
			Add(@"  for both NuGet.exe (a Full Framework console application) and a dotnet");
			Add(@"  simulator for dotnet.exe. If this value is set to ""true"", then those");
			Add(@"  simulators will be invoked instead of the actual programs.");
			Add();
			Add(@"<add key=""DotNetSimulatorDir"" value=""%APPDATA%\NuGet\NuGetSimulator\"" /> ");
			Add();
			Add(@"- The directory in which the dotnet.exe simulator lives.");
			Add();
			Add(@"<add key=""DotNetSimulatorExeName"" value=""NuGetSimulator.dll""/>");
			Add(@"- The name of the dotnet.exe simulator.");
			Add();
			Add(@"<add key=""NuGetSimulatorDir"" value=""%APPDATA%\NuGet\NuGetSimulatorFF\"" /> ");
			Add();
			Add(@"- The directory of the NuGet.exe simulator.");
			Add();
			Add(@"<add key=""NuGetSimulatorExeName"" value=""NuGetSimulator""/>");
			Add();
			Add(@"- The name of the NuGet.exe simulator.");
			Add();
			Add(@"<add key=""DefaultVerbosity"" value=""quiet""/>");
			Add();
			Add(@"- Level of verbosity exhibited by this program. Valid values are quiet, ");
			Add(@"  normal and detailed.");
			Add();
			Add(@"<add key=""PackageHomeDir"" value=""%APPDATA%\NuGet\PackageHome\"" /> ");
			Add();
			Add(@"- Once generated, the location in which the resultant package is stored.");
			Add();
			Add(@"<add key=""RequireReleaseNotesFile"" value=""false""/>");
			Add();
			Add(@"- If set to ""true"", then a Release Notes file containing text concerning");
			Add(@"  the release of the particular version of the package will be a required");
			Add(@"  item. The ReleaseNotes.txt file (or whatever the chosen name is) should be");
			Add(@"  located in the same directory as the project file.");
			Add();
			Add(@"<add key=""ReleaseNotesFileName"" value=""ReleaseNotes.txt""/>");
			Add();
			Add(@"- Name of the release notes file.");
			Add();
			Add(@"<add key=""InjectDefaultReleaseNotes"" value=""true""/>");
			Add();
			Add(@"- If no release notes are found for a particular build, then insert a standard");
			Add(@"  release note if this value is set to ""true"".");
			Add();
			Add(@"<add key=""DefaultReleaseNotes"" value=""No Custom Release Notes for""/>");
			Add();
			Add(@"- The value to be inserted if InjectDefaultReleaseNotes is set to ""true"".");
			Add();
			Add(@"<add key=""RequireSummaryFile"" value=""false""/>");
			Add();
			Add(@"- Similiar to ReleaseNotes, the NuGet.exe program allows the inclusion of");
			Add(@"  a package summary statement. If this value is set to ""true"", then a text");
			Add(@"  file with the package summary will be required.");
			Add();
			Add(@"<add key=""SummaryFileName"" value=""Summary.txt""/>");
			Add();
			Add(@"- The name of the summary file to use.");
			Add();
			Add(@"<!--The DefaultDeleteFileName file will be stored in the PackageHomeDir-->");
			Add(@"<add key=""DefaultDeleteFileName"" value=""DeleteFromNuGet.del""/>");
			Add();
			Add(@"- The name of the Delete file. See the --D flag concerning deletions for further");
			Add(@"  information.");
			Add();
			Add(@"<add key=""UseNuSpecFileIfAvailable"" value=""true""/>");
			Add();
			Add(@"- NuGet.exe will create a .nuspec file on demand that extracts the various");
			Add(@"  values as found in the AssemblyInfo.cs file and places them in an external");
			Add(@"  .nuspec file on demand. This file can then be used by NuGet.exe to create");
			Add(@"  the desired package. If this value is set to ""true"", it is presumed that");
			Add(@"  a Nuget spec command will be issued (this program assumes that one will be ");
			Add(@"  used and acts accordingly).");
			Add();
			Add(@"<!--Replaceable Token Values-->");
			Add();
			Add(@"- See the various command line parameter definitions for NuGet.exe and ");
			Add(@"  dotnet.exe for the meaning and use of the following values:");
			Add();
			Add(@"  The command line definitions follow in the app.config file and allow");
			Add(@"  replaceable tokens whose value is assigned here to be placed on the specified");
			Add(@"  command line and later replaced with the value assigned here when the ");
			Add(@"  particular command is invoked.");
			Add();
			Add(@"  Each token is in the form of $<token name>$ where <token name> is one of the");
			Add(@"  following tokens e.g. ConfigFile => $ConfigFile$.");
			Add();
			Add(@"<add key=""BasePath"" value=""""/>");
			Add(@"<add key=""ConfigFile"" value=""""/>");
			Add(@"<add key=""VerbosityDotNet"" value=""minimal""/>");
			Add(@"<add key=""VersionSuffixDotNet"" value=""""/>");
			Add(@"<add key=""Exclude"" value=""""/>");
			Add(@"<add key=""MinBuildVersion"" value=""""/>");
			Add(@"<add key=""MinClientVersion"" value=""""/>");
			Add(@"<add key=""MSBuildPath"" value=""""/>");
			Add(@"<add key=""MSBuildVersion"" value=""""/>");
			Add(@"<add key=""VerbosityNuGet"" value=""normal""/>");
			Add(@"<add key=""VersionSuffixNuGet"" value=""""/>");
			Add(@"<add key=""Properties"" value=""""/>");
			Add(@"<add key=""Root"" value=""""/>");
			Add(@"<add key=""RuntimeIdentifier"" value=""""/>");
			Add(@"<add key=""Timeout"" value=""""/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				Add();
				OutputAppConfigValues();
			}
		}

	}
}
