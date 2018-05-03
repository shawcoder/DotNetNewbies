namespace NuGetHandler.Help
{
	using System;
	using static AppConfigHandling.CommandLineSettings;
	using static Help;

	public static class HelpCommandLine
	{
		public static void CommandLineSwitches()
		{

		}

		public static void OutputCommandLineSettings()
		{
			Header("Command Line Settings");
			Add($"{nameof(SolutionPath)} = {SolutionPath}");
			Add($"{nameof(SolutionDir)} = {SolutionDir}");
			Add($"{nameof(SolutionExt)} = {SolutionExt}");
			Add($"{nameof(SolutionFileName)} = {SolutionFileName}");
			Add($"{nameof(SolutionName)} = {SolutionName}\n");
			Add($"{nameof(ProjectPath)} = {ProjectPath}");
			Add($"{nameof(ProjectDir)} = {ProjectDir}");
			Add($"{nameof(ProjectExt)} = {ProjectExt}");
			Add($"{nameof(ProjectFileName)} = {ProjectFileName}");
			Add($"{nameof(ProjectName)} = {ProjectName}\n");
			Add($"{nameof(TargetPath)} = {TargetPath}");
			Add($"{nameof(TargetDir)} = {TargetDir}");
			Add($"{nameof(TargetExt)} = {TargetExt}");
			Add($"{nameof(TargetFileName)} = {TargetFileName}");
			Add($"{nameof(TargetName)} = {TargetName}\n");
			Add($"{nameof(ConfigurationName)} = {ConfigurationName}");
			Add($"{nameof(Verbosity)} = {Verbosity}");
			Add($"{nameof(NoOp)} = {NoOp}");
			string vLine =
				Wait
					? "Yep, wait until a key is pressed!"
					: "Nope, just keep going.";
			Add($"{nameof(Wait)} = {vLine}");
			vLine =
				!String.IsNullOrEmpty(ShowHelp)
					? "Yep, Show Help Switch is Set"
					: "Nope, Show Help Switch is NOT Set!";
			Add($"{nameof(GenerateHelp)} = {vLine}");
			vLine =
				ShowEnvironment
					? "Yep, show the environment (these settings)"
					: "Nope, just be quiet.";
			Add($"{nameof(ShowEnvironment)} = {vLine}");
			//Help.Add($"{nameof(HelpSection)} = {HelpSection}");
			Footer();
		}

	}
}
