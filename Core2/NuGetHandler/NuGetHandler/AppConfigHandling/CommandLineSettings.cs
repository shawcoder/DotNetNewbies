namespace NuGetHandler.AppConfigHandling
{
	using System;
	using static CommandLineSettings;

	public static class CommandLineSettings
	{
		public static string TargetPath { get; set; }
		public static string TargetDir { get; set; }
		public static string TargetExt { get; set; }
		public static string TargetFileName { get; set; }  // Name + Ext
		public static string TargetName { get; set; }

		public static string SolutionPath { get; set; }
		public static string SolutionDir { get; set; }
		public static string SolutionExt { get; set; }
		public static string SolutionFileName { get; set; }  // Name + Ext
		public static string SolutionName { get; set; }

		public static string ProjectPath { get; set; }
		public static string ProjectDir { get; set; }
		public static string ProjectExt { get; set; }
		public static string ProjectFileName { get; set; }  // Name + Ext
		public static string ProjectName { get; set; }

		public static string ConfigurationName { get; set; }

		public static string InternalVersionSelector { get; set; }
		public static InternalVersionSelectorE SelectedVersion { get; set; }
		public static string OverrideVersion { get; set; }
		public static string PushToDestination { get; set; }
		public static string Verbosity { get; set; }
		public static VerbosityE VerbosityLevel { get; set; }
		public static bool MergeDeletions { get; set; }
		public static bool PerformDeletes { get; set; }
		//public static string HelpSection { get; set; }

		// Switches (Flags)
		/// <summary>
		/// The "W" switch
		/// Turn off the "ReadKey" that stops the command window from exiting. 
		/// </summary>
		public static bool Wait { get; set; }
		/// <summary>
		/// The "H" switch
		/// </summary>
		//public static bool Help { get; set; }
		public static string Help { get; set; }
		/// <summary>
		/// The "O" switch
		/// If set to true, process the project, but do NOT spawn the command set.
		/// </summary>
		public static bool NoOp { get; set; }
		/// <summary>
		/// Perform all the activities, including updating the ReleaseNotes file,
		/// updating the .csproj or .nuspec file, etc. In short do *EVERYTHING
		/// EXCEPT* spwan the nuget/dotnet nuget processes.
		/// </summary>
		public static bool NoSpawn { get; set; }
		/// <summary>
		/// The "E" switch
		/// </summary>
		public static bool ShowEnvironment { get; set; }

	}

	public class CommandLineValues
	{
		public string TargetPath { get; set; }
		public string TargetDir { get; set; }
		public string TargetExt { get; set; }
		public string TargetFileName { get; set; }  // Name + Ext
		public string TargetName { get; set; }

		public string SolutionPath { get; set; }
		public string SolutionDir { get; set; }
		public string SolutionExt { get; set; }
		public string SolutionFileName { get; set; }  // Name + Ext
		public string SolutionName { get; set; }

		public string ProjectPath { get; set; }
		public string ProjectDir { get; set; }
		public string ProjectExt { get; set; }
		public string ProjectFileName { get; set; }  // Name + Ext
		public string ProjectName { get; set; }

		public string ConfigurationName { get; set; }

		public string InternalVersionSelector { get; set; }
		public InternalVersionSelectorE SelectedVersion { get; set; }
		public string OverrideVersion { get; set; }
		public string PushToDestination { get; set; }
		public string Verbosity { get; set; }
		public VerbosityE VerbosityLevel { get; set; }
		public bool MergeDeletes { get; set; }
		public bool PerformDeletes { get; set; }

		// Switches (Flags)
		/// <summary>
		/// The "W" switch
		/// Turn off the "ReadKey" that stops the command window from exiting. 
		/// </summary>
		public bool Wait { get; set; }
		/// <summary>
		/// The "H" switch
		/// </summary>
		//public bool Help { get; set; }
		public string Help { get; set; }
		/// <summary>
		/// The "O" switch
		/// If set to true, process the project, but do NOT spawn the command set.
		/// </summary>
		public bool NoOp { get; set; }
		/// <summary>
		/// Perform all the activities, including updating the ReleaseNotes file,
		/// updating the .csproj or .nuspec file, etc. In short do *EVERYTHING
		/// EXCEPT* spwan the nuget/dotnet nuget processes.
		/// </summary>
		public bool NoSpawn { get; set; }
		/// <summary>
		/// The "E" switch
		/// </summary>
		public bool ShowEnvironment { get; set; }

	}

	public static class CommandLineValuesHelper
	{
		private static void FromTo(CommandLineValues aCommandLineValues)
		{
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.TargetPath))
			{
				TargetPath = aCommandLineValues.TargetPath;
				TargetDir = aCommandLineValues.TargetDir;
				TargetExt = aCommandLineValues.TargetExt;
				TargetFileName = aCommandLineValues.TargetFileName;
				TargetName = aCommandLineValues.TargetName;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.SolutionPath))
			{
				SolutionPath = aCommandLineValues.SolutionPath;
				SolutionDir = aCommandLineValues.SolutionDir;
				SolutionExt = aCommandLineValues.SolutionExt;
				SolutionFileName = aCommandLineValues.SolutionFileName;
				SolutionName = aCommandLineValues.SolutionName;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.ProjectPath))
			{
				ProjectPath = aCommandLineValues.ProjectPath;
				ProjectDir = aCommandLineValues.ProjectDir;
				ProjectExt = aCommandLineValues.ProjectExt;
				ProjectFileName = aCommandLineValues.ProjectFileName;
				ProjectName = aCommandLineValues.ProjectName;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.ConfigurationName))
			{
				ConfigurationName = aCommandLineValues.ConfigurationName;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.PushToDestination))
			{
				PushToDestination = aCommandLineValues.PushToDestination;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.Verbosity))
			{
				Verbosity = aCommandLineValues.Verbosity;
				VerbosityLevel = aCommandLineValues.VerbosityLevel;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.InternalVersionSelector))
			{
				InternalVersionSelector = aCommandLineValues.InternalVersionSelector;
				SelectedVersion = aCommandLineValues.SelectedVersion;
			}
			if (!String.IsNullOrWhiteSpace(aCommandLineValues.OverrideVersion))
			{
				OverrideVersion = aCommandLineValues.OverrideVersion;
			}
			MergeDeletions = aCommandLineValues.MergeDeletes;
			PerformDeletes = aCommandLineValues.PerformDeletes;
			Wait = aCommandLineValues.Wait;
			CommandLineSettings.Help = aCommandLineValues.Help;
			//HelpSection = aCommandLineValues.HelpSection;
			NoOp = aCommandLineValues.NoOp;
			NoSpawn = aCommandLineValues.NoSpawn;
			ShowEnvironment = aCommandLineValues.ShowEnvironment;
		}

		public static void AssignToStatic(this CommandLineValues aCommandLineValues)
		{
			FromTo(aCommandLineValues);
		}

		public static void AssignDefaultsToStatic()
		{
			TargetPath = String.Empty;
			TargetDir = String.Empty;
			TargetExt = String.Empty;
			TargetFileName = String.Empty;
			TargetName = String.Empty;
			SolutionPath = String.Empty;
			SolutionDir = String.Empty;
			SolutionExt = String.Empty;
			SolutionFileName = String.Empty;
			SolutionName = String.Empty;
			ProjectPath = String.Empty;
			ProjectDir = String.Empty;
			ProjectExt = String.Empty;
			ProjectFileName = String.Empty;
			ProjectName = String.Empty;
			ConfigurationName = String.Empty;
			InternalVersionSelector = String.Empty;
			SelectedVersion = InternalVersionSelectorE.Unknown;
			OverrideVersion = String.Empty;
			Verbosity = "detailed";
			VerbosityLevel = VerbosityE.Detailed;
			MergeDeletions = false;
			OverrideVersion = String.Empty;
			PerformDeletes = false;
			Wait = true;
			CommandLineSettings.Help = HelpSections.ALL;
			//HelpSection = aCommandLineValues.HelpSection;
			NoOp = true;
			NoSpawn = true;
			ShowEnvironment = true;
		}

	}
}
