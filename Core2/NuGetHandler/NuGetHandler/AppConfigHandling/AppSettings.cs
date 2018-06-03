namespace NuGetHandler.AppConfigHandling
{
	public class AppSettings
	{
		public bool SuspendHandling { get; set; }
		public bool AllowOptionalAppConfig { get; set; }
		public bool UseConfigOverride { get; set; }
		public string ConfigOverrideDir { get; set; }
		public string NuGetDir { get; set; }
		public string NuGetExeName { get; set; }
		public string DotNetDir { get; set; }
		public string DotNetName { get; set; }
		/// <summary>
		/// The DotNetVerb is normally "nuget" but dotnet has the "pack" variant 
		/// which is NOT a "nuget" command hence the need for an additional "verb".
		/// </summary>
		public string DotNetVerb { get; set; }
		public bool UseSimulator { get; set; }
		public string NuGetSimulatorDir { get; set; }
		public string NuGetSimulatorExeName { get; set; }
		public string DotNetSimulatorDir { get; set; }
		public string DotNetSimulatorExeName { get; set; }
		public string DefaultVerbosity { get; set; }
		public bool ForceVersionOverride { get; set; }
		public string VersionOverride { get; set; }
		public bool UseDateBasedVersion { get; set; }
		public bool AutoIncrementBuildNumber { get; set; }

		public string PackageHomeDir { get; set; }
		public bool RequireReleaseNotesFile { get; set; }
		public string ReleaseNotesFileName { get; set; }
		public bool RequireSummaryFile { get; set; }
		public string SummaryFileName { get; set; }
		// Full path with extension of the file used to log and perform deletes.
		public string DefaultDeleteFileName { get; set; }
		public string PushToDestination { get; set; }
		public bool UseNuSpecFileIfAvailable { get; set; }
		public bool DeleteNuSpecFileAfterProcessing { get; set; }
		public bool InjectDefaultReleaseNotes { get; set; }
		public string DefaultReleaseNotes { get; set; }

		//--------------------------------------------------------------------------
		// Token Keys
		//--------------------------------------------------------------------------

		public string BasePath { get; set; }
		public string ConfigFile { get; set; }
		public string VerbosityDotNet { get; set; }
		public string VersionSuffixDotNet { get; set; }
		public string Exclude { get; set; }
		public string MinClientVersion { get; set; }
		public string MSBuildPath { get; set; }
		public string MSBuildVersion { get; set; }
		public string VerbosityNuGet { get; set; }
		public string VersionSuffixNuGet { get; set; }
		public string Properties { get; set; }
		public string Root { get; set; }
		public string RuntimeIdentifier { get; set; }
		public string Timeout { get; set; }

	}

	public static class AppSettingsHelper
	{
		private static void FromTo(AppSettings aFrom, AppSettings aTo)
		{
			aTo.SuspendHandling = aFrom.SuspendHandling;
			aTo.AllowOptionalAppConfig = aFrom.AllowOptionalAppConfig;
			aTo.UseConfigOverride = aFrom.UseConfigOverride;
			aTo.ConfigOverrideDir = aFrom.ConfigOverrideDir;
			aTo.NuGetDir = aFrom.NuGetDir;
			aTo.NuGetExeName = aFrom.NuGetExeName;
			aTo.DotNetDir = aFrom.DotNetDir;
			aTo.DotNetName = aFrom.DotNetName;
			aTo.DotNetVerb = aFrom.DotNetVerb;
			aTo.UseSimulator = aFrom.UseSimulator;
			aTo.DotNetSimulatorDir = aFrom.DotNetSimulatorDir;
			aTo.DotNetSimulatorExeName = aFrom.DotNetSimulatorExeName;
			aTo.NuGetSimulatorDir = aFrom.NuGetSimulatorDir;
			aTo.NuGetSimulatorExeName = aFrom.NuGetSimulatorExeName;
			aTo.DefaultVerbosity = aFrom.DefaultVerbosity;
			aTo.VersionSuffixNuGet = aFrom.VersionSuffixNuGet;
			aTo.BasePath = aFrom.BasePath;
			aTo.ForceVersionOverride = aFrom.ForceVersionOverride;
			aTo.VersionOverride = aFrom.VersionOverride;
			aTo.UseDateBasedVersion = aFrom.UseDateBasedVersion;
			aTo.AutoIncrementBuildNumber = aFrom.AutoIncrementBuildNumber;
			aTo.PackageHomeDir = aFrom.PackageHomeDir;
			aTo.RequireReleaseNotesFile = aFrom.RequireReleaseNotesFile;
			aTo.ReleaseNotesFileName = aFrom.ReleaseNotesFileName;
			aTo.RequireSummaryFile = aFrom.RequireSummaryFile;
			aTo.SummaryFileName = aFrom.SummaryFileName;
			aTo.DefaultDeleteFileName = aFrom.DefaultDeleteFileName;
			aTo.PushToDestination = aFrom.PushToDestination;
			aTo.UseNuSpecFileIfAvailable = aFrom.UseNuSpecFileIfAvailable;
			aTo.DeleteNuSpecFileAfterProcessing =
				aFrom.DeleteNuSpecFileAfterProcessing;
			aTo.InjectDefaultReleaseNotes = aFrom.InjectDefaultReleaseNotes;
			aTo.InjectDefaultReleaseNotes = aFrom.InjectDefaultReleaseNotes;

			// Replaceable Tokens
			aTo.BasePath = aFrom.BasePath;
			aTo.ConfigFile = aFrom.ConfigFile;
			aTo.VerbosityDotNet = aFrom.VerbosityDotNet;
			aTo.VersionSuffixDotNet = aFrom.VersionSuffixDotNet;
			aTo.Exclude = aFrom.Exclude;
			aTo.MinClientVersion = aFrom.MinClientVersion;
			aTo.MSBuildPath = aFrom.MSBuildPath;
			aTo.MSBuildVersion = aFrom.MSBuildVersion;
			aTo.VerbosityNuGet = aFrom.VerbosityNuGet;
			aTo.VersionSuffixNuGet = aFrom.VersionSuffixNuGet;
			aTo.Properties = aFrom.Properties;
			aTo.Root = aFrom.Root;
			aTo.RuntimeIdentifier = aFrom.RuntimeIdentifier;
			aTo.Timeout = aFrom.Timeout;
		}

		public static void AssignFrom
			(this AppSettings aAppSettings, AppSettings aFrom)
		{
			FromTo(aFrom, aAppSettings);
		}

		public static void AssignTo(this AppSettings aAppSettings, AppSettings aTo)
		{
			FromTo(aTo, aAppSettings);
		}

	}

}