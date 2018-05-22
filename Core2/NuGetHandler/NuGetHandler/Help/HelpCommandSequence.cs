namespace NuGetHandler.Help
{
	using System.Collections.Generic;
	using AppConfigHandling;
	using static Help;

	public static class HelpCommandSequence
	{
		private static void OutputCommandSequenceFullFrameworkCommandValues()
		{
			Add("Full Framework Commands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.FullFrameworkCommands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Add();
		}

		private static void OutputCommandSequenceDotNetCoreCommandValues()
		{
			Add("Core 2.0 Cmomands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.Core_2_0_Commands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Add();
		}

		private static void OutputCommandSequenceDotNetStandardCommandValues()
		{
			Add("Standard 2.0 Cmomands");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.Standard_2_0_Commands.Commands)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Add();
		}

		public static void OutputCommandSequenceFullFramework()
		{
			Add(@"DotNetFullFrameworkCommandSequence");
			Add();
			Add(@"- The following are the definitions of the various command line sequences to be");
			Add(@"  used with the NuGet.exe program. The ""key"" portion will be used as the ");
			Add(@"  NuGet ""verb"" aka command, e.g. nuget pack, to be executed. The ""value"" portion");
			Add(@"  represents the actual command line before token substitution.");
			Add();
			Add(@"  As a for instance, the ""spec"" command contains the following replaceable ");
			Add(@"  tokens:");
			Add(@"    $AssemblyPath$");
			Add(@"    $VerbosityNuget$");
			Add();
			Add(@"  The $AssemblyPath$ is the full path to the .csproj file.");
			Add(@"  The $VerbosityNuget$ is the verbosity level of the nuget.exe program.");
			Add();
			Add(@"  See the respective help files for NuGet.exe for additional details on the ");
			Add(@"  structure and content of each command that is supported by nuget.exe.");
			Add();
			Add(@"  Change these values as appropriate for new versions of NuGet.exe. The current");
			Add(@"  version as of the writing of this help file is 4.5.1.4879.");
			Add();
			Add(@"<add key=""spec"" value='-AssemblyPath ""$AssemblyPath$"" - force - NonInteractive - verbosity $VerbosityNuGet$'/>");
			Add(@"<add key=""pack"" value='""$ProjectPath$"" - ExcludeEmptyDirectories - IncludeReferencedProjects - OutputDirectory ""$OutputPackageTo$"" -Verbosity $VerbosityNuGet$ -Version $PackageVersion$'/>");
			Add(@"<add key=""push"" value='$PackagePath$ -Source $Source$ -ApiKey $ApiKey$ -Verbosity $VerbosityNuGet$ -NonInteractive'/>");
			Add(@"<add key=""add"" value='$PackagePath$ -Source $Source$ -ApiKey $ApiKey$ -Verbosity $VerbosityNuGet$ -NonInteractive'/>");
			Add(@"<add key=""delete"" value='$PackageId$ $PackageVersion$ -Source $Source$ -ApiKey $ApiKey$ -NonInteractive-Verbosity $VerbosityNuGet$'/> ");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				OutputCommandSequenceFullFrameworkCommandValues();
			}
		}

		/*
		Apparently, the following command line works just as well. I had wondered 
		about the loss of setting the ""version"" like one can in NuGet.exe. This 
		allows for that.

		Note that ""publish"" can be replaced by ""pack"".

				dotnet publish /property:Version=1.2.3.4

		Read further on this page: https://docs.microsoft.com/en-us/dotnet/standard/frameworks
		*/

		public static void OutputCommandSequenceDotNetCore()
		{
			Add(@"DotNetCore_2_0_CommandSequence");
			Add();
			Add(@"<!--All keys must be expressed as lower case values e.g. ""pack""-->");
			Add(@"<add key=""pack"" value='""$ProjectPath$""--configuration $ConfigurationName$ --force--no - build--no - restore--output ""$OutputPackageTo$"" --serviceable --verbosity $VerbosityDotNet$'/>");
			Add(@"<add key=""push"" value='""$PackagePath$""--source $Source$ --api - key $ApiKey$'/>");
			Add(@"<add key=""delete"" value='$PackageId$ $PackageVersion$ --source $Source$ --non-interactive --api-key $ApiKey$'/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				OutputCommandSequenceDotNetCoreCommandValues();
			}
		}

		public static void OutputCommandSequenceDotNetStandard()
		{
			Add(@"DotNetStandard_2_0_CommandSequence");
			Add();
			Add(@"<!--All keys must be expressed as lower case values e.g. ""pack""-->");
			Add(@"<add key=""pack"" value='""$ProjectPath$"" --configuration $ConfigurationName$ --force --no-build --no-restore --output ""$OutputPackageTo$"" --serviceable --verbosity $VerbosityDotNet$'/>");
			Add(@"<add key=""push"" value='""$PackagePath$"" --source $Source$ --api-key $ApiKey$'/>");
			Add(@"<add key=""delete"" value='$PackageId$ $PackageVersion$ --source $Source$ --non-interactive --api-key $ApiKey$'/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				OutputCommandSequenceDotNetStandardCommandValues();
			}
		}

		public static void OutputCommandSequence()
		{
			OutputCommandSequenceFullFramework();
			SectionBreak();
			OutputCommandSequenceDotNetCore();
			SectionBreak();
			OutputCommandSequenceDotNetStandard();
		}

	}
}
