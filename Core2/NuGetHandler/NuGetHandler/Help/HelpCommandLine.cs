namespace NuGetHandler.Help
{
	using System;
	using AppConfigHandling;
	using static AppConfigHandling.CommandLineSettings;
	using static Help;

	public static class HelpCommandLine
	{
		public static void OutputCommandLineHelp()
		{
			Add("Command line:");
			Add();
			Add("Command line values come in three types:");
			Add();
			Add("  1.  Switches: Switches are command line values which are preceded by");
			Add("                either a double dash (--) or a slash (/).");
			Add();
			Add("      Switches are defined as indicators which are either present or");
			Add("      not present on the command line. If present, then the strongly-typed");
			Add("      container value whose name matches the switch name will be set to");
			Add("      true, indicating that the switch was found as part of the command line.");
			Add("        ");
			Add("        --NoOp");
			Add();
			Add("        - or its variant -");
			Add();
			Add("        --N");
			Add();
			Add("      COMMAND_LINE_SWITCHES ARE CASE-**IN**-SENSITIVE!");
			Add();
			Add("  2.  Named Values: Named values are command line values which come in");
			Add("                    pairs. First the owning name of the named value,");
			Add("                    which is preceded by a SINGLE dash (-), followed by");
			Add(@"                    the ""value"" portion of the named value pair, separated by");
			Add("                    a space.");
			Add();
			Add(@"        -ProjectPath ""C:\Some\Project\Path\File""");
			Add();
			Add("      Note that values that contain a space *MUST* be enclosed in double quotes.");
			Add();
			Add("  3.  Values. Values are POSITION DEPENDENT arguments whose postion in");
			Add("      the command line argument string determines their function within");
			Add(@"      the application e.g. If the value ""C:\Dir"" is in position 1, it");
			Add("      may tell the program where to look for a config file, if in");
			Add("      position 2, it might tell where to find a source file, if it follows a");
			Add("      named value indicator, then it's the value to be assigned to the named");
			Add("      value.");
		}

		public static void OutputCommandLineSwitches()
		{
			Add("Switches:");
			Add();
			Add("--D:  DeleteFromNuGet. This switch tells NuGetHandler to engage the deletion");
			Add("      function in that the file in the PackageHomeDir named (by default, the");
			Add("      user can change this) DeleteFromNuGet.del will be opened, scanned and");
			Add("      each entry applied as a request to delete the package from the requisite");
			Add("      NuGet server.");
			Add();
			Add("--E:  ShowEnvironment. If included, this switch will cause the NuGetHandler");
			Add("      program to show the content of the command line, various final values");
			Add("      as brought in from the various app.*.config files, etc. Normally used");
			Add("      for debugging.");
			Add();
			Add("--M:  MergeDeletions. As each package is successfully pushed to a NuGet server,");
			Add("      the program creates a .del file which contains the package name, version,");
			Add("      location sent to, etc. If the user wishes to clean up one or more");
			Add("      NuGet servers, then this command must be executed prior to calling ");
			Add(@"      NuGetHandler with the aforementioned ""D"" switch. The function of this");
			Add("      switch is to cause NuGetHandler to open all of the .del files, scan the");
			Add("      content and append it to the DeleteFromNuGet.del file. This file can then");
			Add("      be edited, by the user in any text editor, to select which of the desired");
			Add("      package instances should be deleted. If an entry is deleted from the ");
			Add("      DeleteFromNuGet.del file prior to execution, that file will not be");
			Add("      deleted nor will the package be removed from the NuGet server. The format");
			Add("      of the .del file name is <some GUID>.del. The GUID portion of the name");
			Add("      will be included in the DeleteFromNuGet.del file so that when the package");
			Add("      is successfully deleted from the NuGet server, the corresponding .del ");
			Add("      file will also be deleted.");
			Add();
			Add("--N:  NoOp. Basically, tell the NuGetHandler to not do anything.");
			Add();
			Add("--Q:  NoSpawn. Tell NuGetHandler to process the entire project file as if it");
			Add("      were going to pack/push, etc. the designated project but to not actually");
			Add("      process the project. Mainly used for debugging.");
			Add();
			Add("--C:  ConfigurationName e.g. Release | Debug | etc.");
			Add();
			Add("--W:  Wait (Show a message at execution conclusion and wait for a keypress).");
		}

		public static void OutputCommandLineNamedValues()
		{
			Add("Command Line Named Values");
			Add();
			Add("-P:   ProjectPath. The full path to the project including the project name and");
			Add("      extension.");
			Add();
			Add("-H:   Show Help. This must be followed by a help topic. If none is supplied then");
			Add("      the help on help section will be displayed.");
			Add();
			Add("-S:   SolutionPath. Full path to the solution including the solution name and");
			Add("      extension.");
			Add();
			Add("-T:   TargetPath. Full path to the destination. This is normally a directory set");
			Add(@"      up in the %APPDATA%\NuGet directory (at least, that's the default) that");
			Add("      will contain the output NuGet packages. The packages themselves are");
			Add("      separated into their respective solution directories, each of which contains");
			Add("      the project package which, in turn, is stored in a sub-directory of the");
			Add("      package name as a version. e.g.");
			Add();
			Add(@"      %APPDATA%\NuGet\PackageHome\<solution>\<project>\<version>");
			Add();
			Add("-I    Internal Version Selector. Use this value to select the version as");
			Add("      set inside the project. There are three values to choose from:");
			Add();
			Add("        - AssemblyVersion");
			Add("        - AssemblyFileVersion");
			Add("        - PackageVersion");
			Add();
			Add("      Each package has all three values defined (if the developer was paying");
			Add("      attention and did their job correctly) so one need only select");
			Add("       the desired internal version using one of the values specified");
			Add();
			Add("-O    If the user simply wishes to override the version regardless of the");
			Add("      content of the package, then the user may override the version by");
			Add("      utilizing this option. Simply set the version as desired e.g. 1.2.3.4");
			Add();
			Add("-V:   Verbosity. Allowed verbosity values: quiet, normal, detailed.");
			Add();
			Add("-U:   PushToDestination. The name of the repository set to push to. This value,");
			Add("      if used, will override ALL values provided in the various app.*.config");
			Add("      files.");
		}

		public static void OutputCommandLineSettings()
		{
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
			Add($"{nameof(InternalVersionSelector)} = {InternalVersionSelector}");
			Add($"{nameof(SelectedVersion)} = {SelectedVersion}");
			Add($"{nameof(OverrideVersion)} = {OverrideVersion}");
			Add($"{nameof(Verbosity)} = {Verbosity}");
			Add($"{nameof(NoOp)} = {NoOp}");
			string vLine =
				Wait
					? "Yep, wait until a key is pressed!"
					: "Nope, just keep going.";
			Add($"{nameof(Wait)} = {vLine}");
			vLine =
				!String.IsNullOrEmpty(CommandLineSettings.Help)
					? "Yep, Show Help Switch is Set"
					: "Nope, Show Help Switch is NOT Set!";
			Add($"{nameof(GenerateHelp)} = {vLine}");
			vLine =
				ShowEnvironment
					? "Yep, show the environment (these settings)"
					: "Nope, just be quiet.";
			Add($"{nameof(ShowEnvironment)} = {vLine}");
		}

		public static void OutputCommandLine()
		{
			OutputCommandLineHelp();
			SectionBreak("Command Line Switches");
			OutputCommandLineSwitches();
			SectionBreak("Command Line Named Values");
			OutputCommandLineNamedValues();
			if (VerbosityLevel == VerbosityE.Detailed)
			{
				SectionBreak("Command Line Settings");
				OutputCommandLineSettings();
				SectionBreak();
			}
		}

	}
}
