// ReSharper disable InconsistentNaming
namespace NuGetHandler
{
	using System;
	using System.IO;
	using AppConfigHandling;
	using ConfigurationHandler;
	using Infrastructure;
	using ProjectFileProcessing;
	using Run_NuGet;
	using static Consts;
	using static Help.Help;
	using static ProjectFileProcessing.ProcessProjectFile;
	using static Run_NuGet.TokenSetContainer;

	public interface ILaunchPoint
	{
		void Execute(string[] aArgs);
	}

	public class LaunchPoint: ILaunchPoint
	{
		private readonly IHandleConfiguration _HandleConfiguration;
		private readonly ISpawnNugetProcesses _SpawnNugetProcesses;

		public LaunchPoint
		(
			IHandleConfiguration aHandleConfiguration
			, ISpawnNugetProcesses aSpawnNugetProcesses
		)
		{
			_HandleConfiguration =
				aHandleConfiguration
					?? throw new NullReferenceException(nameof(aHandleConfiguration));
			_SpawnNugetProcesses =
				aSpawnNugetProcesses
					?? throw new NullReferenceException(nameof(aSpawnNugetProcesses));
		}

		private void InitializeTokenSet()
		{
			// Initialize from App.config and command line settings
			AssemblyPath = CommandLineSettings.TargetPath ?? String.Empty;
			ProjectPath = CommandLineSettings.ProjectPath ?? String.Empty;
			BasePath = _HandleConfiguration.AppSettingsValues.BasePath;
			ConfigFile = _HandleConfiguration.AppSettingsValues.ConfigFile;
			ConfigurationName = CommandLineSettings.ConfigurationName ?? String.Empty;
			VerbosityDotNet = _HandleConfiguration.AppSettingsValues.VerbosityDotNet;
			VersionSuffixDotNet =
				_HandleConfiguration.AppSettingsValues.VersionSuffixDotNet;
			Exclude = _HandleConfiguration.AppSettingsValues.Exclude;
			MinClientVersion =
				_HandleConfiguration.AppSettingsValues.MinClientVersion;
			MSBuildPath = _HandleConfiguration.AppSettingsValues.MSBuildPath;
			MSBuildVersion = _HandleConfiguration.AppSettingsValues.MSBuildVersion;
			VerbosityNuGet = _HandleConfiguration.AppSettingsValues.VerbosityNuGet;
			VersionSuffixNuGet =
				_HandleConfiguration.AppSettingsValues.VersionSuffixNuGet;
			Properties = _HandleConfiguration.AppSettingsValues.Properties;
			Root = _HandleConfiguration.AppSettingsValues.Root;
			RuntimeIdentifier =
				_HandleConfiguration.AppSettingsValues.RuntimeIdentifier;
			Timeout = _HandleConfiguration.AppSettingsValues.Timeout;
			PackageId = CommandLineSettings.TargetName ?? String.Empty;
			PackageName = CommandLineSettings.TargetName ?? String.Empty;

			// Calculated settings

			NuSpecFilePath = Path.ChangeExtension(ProjectPath, NUSPEC_EXT);
		}

		private void WriteVersionBackToProject(string aVersion)
		{
			if (FrameworkInformation.Framework == DotNetFramework.Full)
			{
				return; // It's too hard to fix the full framework version of version.
			}
			WritePackageVersion(aVersion);
		}

		private void SetVersion()
		{
			if (CommandLineSettings.SelectedVersion != InternalVersionSelectorE.Unknown)
			{
				switch (CommandLineSettings.SelectedVersion)
				{
					case InternalVersionSelectorE.AssemblyFileVersion:
					{
						PackageVersion = FrameworkInformation.AssemblyFileVersion;
						break;
					}
					case InternalVersionSelectorE.AssemblyVersion:
					{
						PackageVersion = FrameworkInformation.AssemblyVersion;
						break;
					}
					case InternalVersionSelectorE.PackageVersion:
					{
						PackageVersion = FrameworkInformation.PackageVersion;
						break;
					}
					default:
					{
						PackageVersion = FrameworkInformation.AssemblyFileVersion;
						break;
					}
				}
			}
			else
			{
				PackageVersion =
					_HandleConfiguration.AppSettingsValues.ForceVersionOverride
						? _HandleConfiguration.AppSettingsValues.VersionOverride
						: FrameworkInformation.PackageVersion;
			}
			if (String.IsNullOrWhiteSpace(PackageVersion))
			{
				PackageVersion = DEFAULT_VERSION;
			}
			string[] vPieces =
				PackageVersion.Split(DOT, StringSplitOptions.RemoveEmptyEntries);
			if (vPieces.Length != 4)
			{
				PackageVersion = DEFAULT_VERSION;
				vPieces =
					PackageVersion.Split(DOT, StringSplitOptions.RemoveEmptyEntries);
			}
			if (_HandleConfiguration.AppSettingsValues.UseDateBasedVersion)
			{
				DateTime vNow = DateTime.Now;
				vPieces[1] = vNow.Year.ToString();
				vPieces[2] = vNow.Date.ToString("Mdd");
				PackageVersion = $"{vPieces[0]}.{vPieces[1]}.{vPieces[2]}.{vPieces[3]}";
			}
			if (_HandleConfiguration.AppSettingsValues.AutoIncrementBuildNumber)
			{
				vPieces =
					PackageVersion.Split(DOT, StringSplitOptions.RemoveEmptyEntries);
				if (short.TryParse(vPieces[3], out short vBuildNumber))
				{
					vBuildNumber++;
					vPieces[3] = vBuildNumber.ToString();
					PackageVersion =
						$"{vPieces[0]}.{vPieces[1]}.{vPieces[2]}.{vPieces[3]}";
					WriteVersionBackToProject(PackageVersion);
				}
			}
		}

		private void SetFrameworkInfo()
		{
			SetVersion();
			PackageFileName =
				CommandLineSettings.TargetName
					+ DOT
					+ PackageVersion
					+ PACKAGE_EXT;

			PackageDir =
				Environment.ExpandEnvironmentVariables
					(_HandleConfiguration.AppSettingsValues.PackageHomeDir).AsDir()
				+ (CommandLineSettings.SolutionName?.AsDir() ?? "".AsDir())
				+ (CommandLineSettings.TargetName?.AsDir() ?? "".AsDir())
				+ PackageVersion.AsDir();
			PackagePath = PackageDir + PackageFileName;

		}

		public void Execute(string[] aArgs)
		{
			_HandleConfiguration.ProcessConfiguration(aArgs);
			InitializeTokenSet();
			bool vTest = !String.IsNullOrEmpty(CommandLineSettings.Help);
			if (vTest)
			{
				Help.Help.HandleConfiguration = _HandleConfiguration;
				CommandLineSettings.Help = CommandLineSettings.Help.ToLower();
				GenerateHelp();
				Wait.Pause();
				return;
			}
			ProcessProjectFileContent();
			SetFrameworkInfo();
			vTest =
				CommandLineSettings.NoOp
					|| (_HandleConfiguration.AppSettingsValues?.SuspendHandling ?? true);
			if (!vTest)
			{
				_SpawnNugetProcesses.Spawn(_HandleConfiguration);
				Wait.Pause();
			}
		}

	}
}