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
			AssemblyPath = CommandLineSettings.TargetPath;
			ProjectPath = CommandLineSettings.ProjectPath;
			BasePath = _HandleConfiguration.AppSettingsValues.BasePath;
			ConfigFile = _HandleConfiguration.AppSettingsValues.ConfigFile;
			ConfigurationName = CommandLineSettings.ConfigurationName;
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
			PackageId = CommandLineSettings.TargetName;
			PackageName = CommandLineSettings.TargetName;

			// Calculated settings

			PackageVersion =
				_HandleConfiguration.AppSettingsValues.ForceVersionOverride
					? _HandleConfiguration.AppSettingsValues.VersionOverride
					: FrameworkInformation.PackageVersion;

			PackageFileName =
				Path.ChangeExtension(CommandLineSettings.TargetName, PackageVersion)
					+ Consts.PACKAGE_EXT;

			PackageDir =
				Environment.ExpandEnvironmentVariables
					(_HandleConfiguration.AppSettingsValues.PackageHomeDir).AsDir()
						+ CommandLineSettings.SolutionName.AsDir()
						+ CommandLineSettings.TargetName.AsDir()
						+ PackageVersion.AsDir();
			PackagePath = PackageDir + PackageFileName;

			NuSpecFilePath = Path.ChangeExtension(ProjectPath, Consts.NUSPEC_EXT);
		}

		public void Execute(string[] aArgs)
		{
			_HandleConfiguration.ProcessConfiguration(aArgs);
			InitializeTokenSet();
			ProcessProjectFileContent();
			bool vTest = !String.IsNullOrEmpty(CommandLineSettings.ShowHelp);
			if (vTest)
			{
				Help.Help.HandleConfiguration = _HandleConfiguration;
				GenerateHelp();
				Wait.Pause();
				return;
			}
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