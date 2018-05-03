// ReSharper disable InconsistentNaming
namespace NuGetHandler.ConfigurationHandler
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using AppConfigConfigurationBuilder;
	using AppConfigHandling;
	using HandlerCommandLine;
	using Infrastructure;
	using Microsoft.Extensions.Configuration;

	using static AppConfigHandling.CommandLineSettings;
	using static CommandLineSwitches;

	public interface IHandleConfiguration
	{
		void ProcessConfiguration(string[] aArgs);
		AppSettings AppSettingsValues { get; set; }
		NuGetNuSpecValues NuGetNuSpecSettings { get; set; }
		DotNetNuSpecValues DotNetNuSpecSettings { get; set; }
		NuGetRepositories NuGetRepos { get; set; }
		NuGetPushDestinations NuGetDestinations { get; set; }
		DotNetFullFrameworkCommandSequence FullFrameworkCommands { get; set; }
		DotNetStandard_2_0_CommandSequence Standard_2_0_Commands { get; set; }
		DotNetCore_2_0_CommandSequence Core_2_0_Commands { get; set; }
		List<NuGetRepository> Repositories { get; }
		Dictionary<string, List<NuGetRepository>> Destinations { get; }
		NuGetDestination SelectedDestination { get; }
		string ConfigFileName { get; }
	}

	public class HandleConfiguration
		: IHandleConfiguration
	{
		// App.config section names
		private const string _NUGET_REPOSITORIES = "NuGetRepos";
		private const string _NUGET_PUSH_DESTINATIONS = "NuGetDestinations";
		private const string _DOT_NET_FULL_FRAMEWORK_COMMAND_SEQUENCE =
			"DotNetFullFrameworkCommandSequence";
		private const string _DOT_NET_STANDARD_2_0_COMMAND_SEQUENCE =
			"DotNetStandard_2_0_CommandSequence";
		private const string _DOT_NET_CORE_2_0_COMMAND_SEQUENCE =
			"DotNetCore_2_0_CommandSequence";

		private const string _CONFIG_FILE_NAME = "App.config";
		private const string _OPTIONAL_CONFIG_FILE_NAME = "App.optional.config";

		private IConfiguration _Configuration;

		private void ValidateStringValue(string aValueName, string aValue)
		{
			bool vTest = !String.IsNullOrWhiteSpace(aValue);
			if (!vTest)
			{
				throw new ArgumentNullException
					(aValueName, $"{aValueName} cannot be null or whitespace!");
			}
		}

		private void LoadCommandLine(string[] aArgs)
		{
			Dictionary<string, string> vMappings =
				new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			vMappings.Add(CONFIGURATION_NAME, CONFIGURATION_NAME_VARIANTS);
			vMappings.Add(DELETE_FROM_NUGET, DELETE_FROM_NUGET_VARIANTS);
			vMappings.Add(MERGE_DELETIONS, MERGE_DELETIONS_VARIANTS);
			vMappings.Add(NO_OP, NO_OP_VARIANTS);
			vMappings.Add(NO_SPAWN, NO_SPAWN_VARIANTS);
			vMappings.Add(PROJECT_PATH, PROJECT_PATH_VARIANTS);
			vMappings.Add(PUSH_TO_DESTINATION, PUSH_TO_DESTINATION_VARIANTS);
			vMappings.Add(SHOW_ENVIRONMENT, SHOW_ENVIRONMENT_VARIANTS);
			vMappings.Add(SHOW_HELP, SHOW_HELP_VARIANTS);
			vMappings.Add(SOLUTION_PATH, SOLUTION_PATH_VARIANTS);
			vMappings.Add(TARGET_PATH, TARGET_PATH_VARIANTS);
			vMappings.Add(VERBOSITY, VERBOSITY_VARIANTS);
			vMappings.Add(WAIT, WAIT_VARIANTS);
			_Configuration =
				new ConfigurationBuilder()
					.AddCommandLineIsolated(aArgs, vMappings)
					.Build();
			CommandLineValues vCommandLineValues =
				_Configuration.Get<CommandLineValues>();
			if (vCommandLineValues != null)
			{
				vCommandLineValues.AssignToStatic();
			}
			else
			{
				CommandLineValuesHelper.AssignDefaultsToStatic();
			}
		}

		private void ValidateConfiguration()
		{
			ValidateStringValue(nameof(TargetPath), TargetPath);
			ValidateStringValue(nameof(TargetDir), TargetDir);
			ValidateStringValue(nameof(TargetExt), TargetExt);
			ValidateStringValue(nameof(TargetFileName), TargetFileName);
			ValidateStringValue(nameof(TargetName), TargetName);
			ValidateStringValue(nameof(SolutionPath), SolutionPath);
			ValidateStringValue(nameof(SolutionDir), SolutionDir);
			ValidateStringValue(nameof(SolutionExt), SolutionExt);
			ValidateStringValue(nameof(SolutionFileName), SolutionFileName);
			ValidateStringValue(nameof(SolutionName), SolutionName);
			ValidateStringValue(nameof(ProjectPath), ProjectPath);
			ValidateStringValue(nameof(ProjectDir), ProjectDir);
			ValidateStringValue(nameof(ProjectExt), ProjectExt);
			ValidateStringValue(nameof(ProjectFileName), ProjectFileName);
			ValidateStringValue(nameof(ProjectName), ProjectName);
		}

		private void ProcessCommandLine()
		{
			if (!String.IsNullOrWhiteSpace(Verbosity))
			{
				bool vTest =
					VERBOSITY_VALUES.IndexOf
						(Verbosity, StringComparison.OrdinalIgnoreCase) >= 0;
				if (vTest)
				{
					Enum.TryParse(Verbosity, true, out VerbosityE vVerbosity);
					VerbosityLevel = vVerbosity;
				}
			}
			string vPath;
			bool vDoConfiguration = !String.IsNullOrWhiteSpace(TargetPath);
			if (vDoConfiguration)
			{
				vPath = TargetPath;
				TargetDir = Path.GetDirectoryName(vPath);
				TargetFileName = Path.GetFileName(vPath);
				TargetName = Path.GetFileNameWithoutExtension(vPath);
				TargetExt = Path.GetExtension(vPath);
			}
			vDoConfiguration =
				vDoConfiguration && !String.IsNullOrWhiteSpace(SolutionPath);
			if (vDoConfiguration)
			{
				vPath = SolutionPath;
				SolutionDir = Path.GetDirectoryName(vPath);
				SolutionFileName = Path.GetFileName(vPath);
				SolutionName = Path.GetFileNameWithoutExtension(vPath);
				SolutionExt = Path.GetExtension(vPath);
			}
			vDoConfiguration =
				vDoConfiguration && !String.IsNullOrWhiteSpace(ProjectPath);
			if (vDoConfiguration)
			{
				vPath = ProjectPath;
				ProjectDir = Path.GetDirectoryName(vPath);
				ProjectFileName = Path.GetFileName(vPath);
				ProjectName = Path.GetFileNameWithoutExtension(vPath);
				ProjectExt = Path.GetExtension(vPath);
			}
			if (vDoConfiguration)
			{
				ValidateConfiguration();
			}
		}

		private void LoadInitialConfiguration()
		{
			string vAppConfigFileName =
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).AsDir()
					+ _CONFIG_FILE_NAME;
			if (!File.Exists(vAppConfigFileName))
			{
				throw new FileNotFoundException
					($"Missing config file:\n{vAppConfigFileName}");
			}
			_Configuration =
				new ConfigurationBuilder()
					.AddAppConfig(vAppConfigFileName, false, true)
					.Build();
		}

		private void ExtractConfigValues()
		{
			AppSettingsValues = _Configuration.Get<AppSettings>();
			NuGetNuSpecSettings = _Configuration.Get<NuGetNuSpecValues>();
			DotNetNuSpecSettings = _Configuration.Get<DotNetNuSpecValues>();
			if (DotNetNuSpecSettings.UseNuGetNuSpecValues)
			{
				DotNetNuSpecSettings.AssignFrom(NuGetNuSpecSettings);
			}
			NuGetRepos =
				_Configuration.Get<NuGetRepositories>(_NUGET_REPOSITORIES);
			NuGetDestinations =
				_Configuration.Get<NuGetPushDestinations>
					(_NUGET_PUSH_DESTINATIONS);
			FullFrameworkCommands =
				_Configuration.Get<DotNetFullFrameworkCommandSequence>
					(_DOT_NET_FULL_FRAMEWORK_COMMAND_SEQUENCE);
			Standard_2_0_Commands =
				_Configuration.Get<DotNetStandard_2_0_CommandSequence>
					(_DOT_NET_STANDARD_2_0_COMMAND_SEQUENCE);
			Core_2_0_Commands =
				_Configuration.Get<DotNetCore_2_0_CommandSequence>
					(_DOT_NET_CORE_2_0_COMMAND_SEQUENCE);
		}

		private void LoadOptionalConfiguration(string aConfigPath)
		{
			if (File.Exists(aConfigPath))
			{
				_Configuration =
				new ConfigurationBuilder()
					.AddAppConfig(aConfigPath, true, true)
					.Build();
				NuGetNuSpecValues vNuGetNuSpecSettings = _Configuration.Get<NuGetNuSpecValues>();
				AppSettings vAppSettingsValues = _Configuration.Get<AppSettings>();
				NuGetNuSpecSettings.AssignFrom(vNuGetNuSpecSettings);
				DotNetNuSpecSettings = _Configuration.Get<DotNetNuSpecValues>();
				if (DotNetNuSpecSettings.UseNuGetNuSpecValues)
				{
					DotNetNuSpecSettings.AssignFrom(NuGetNuSpecSettings);
				}
				AppSettingsValues.AssignFrom(vAppSettingsValues);
			}
		}

		private void LoadOptionalConfigurationOverride()
		{
			if (String.IsNullOrWhiteSpace(AppSettingsValues.ConfigOverrideDir))
			{
				return;
			}
			string vPath =
				Environment.ExpandEnvironmentVariables
					(AppSettingsValues.ConfigOverrideDir).AsDir();
			string vOptionalConfigFileName =
				Path.Combine(vPath, _OPTIONAL_CONFIG_FILE_NAME);
			if (File.Exists(vOptionalConfigFileName))
			{
				LoadOptionalConfiguration(vOptionalConfigFileName);
			}
		}

		private void LoadOptionalSolutionConfiguration()
		{
			string vOptionalConfigFileName =
				Path.Combine(SolutionDir, _OPTIONAL_CONFIG_FILE_NAME);
			LoadOptionalConfiguration(vOptionalConfigFileName);
		}

		private void LoadOptionalProjectConfiguration()
		{
			string vOptionalConfigFileName =
				Path.Combine(ProjectDir, _OPTIONAL_CONFIG_FILE_NAME);
			LoadOptionalConfiguration(vOptionalConfigFileName);
		}

		private void ProcessTheConfiguration()
		{
			// Allow the command line override to override the configuared
			// destination.
			if (!String.IsNullOrWhiteSpace(PushToDestination))
			{
				AppSettingsValues.PushToDestination = PushToDestination;
			}
			if (String.IsNullOrWhiteSpace(AppSettingsValues.PackageHomeDir))
			{
				ErrorContainer.Errors.Add
					($"Missing {nameof(AppSettingsValues.PackageHomeDir)} value.");
			}
			AppSettingsValues.PackageHomeDir =
				Environment.ExpandEnvironmentVariables
					(AppSettingsValues.PackageHomeDir).AsDir();
			Directory.CreateDirectory(AppSettingsValues.PackageHomeDir);
			if (String.IsNullOrWhiteSpace(AppSettingsValues.NuGetDir))
			{
				ErrorContainer.Errors.Add
					($"Missing {nameof(AppSettingsValues.NuGetDir)} value.");
			}
			AppSettingsValues.NuGetDir =
				Environment.ExpandEnvironmentVariables
					(AppSettingsValues.NuGetDir).AsDir();
			Directory.CreateDirectory(AppSettingsValues.NuGetDir);
			if (String.IsNullOrWhiteSpace(AppSettingsValues.DotNetDir))
			{
				ErrorContainer.Errors.Add
					($"Missing {nameof(AppSettingsValues.DotNetDir)} value.");
			}
			AppSettingsValues.DotNetDir =
				Environment.ExpandEnvironmentVariables
					(AppSettingsValues.DotNetDir).AsDir();
			Directory.CreateDirectory(AppSettingsValues.DotNetDir);
		}

		private void FetchTheConfiguration()
		{
			ExtractConfigValues();
			if (AppSettingsValues.AllowOptionalAppConfig)
			{
				if (AppSettingsValues.UseConfigOverride)
				{
					LoadOptionalConfigurationOverride();
				}
				else
				{
					LoadOptionalSolutionConfiguration();
					if (AppSettingsValues.UseConfigOverride)
					{
						LoadOptionalConfigurationOverride();
					}
					else
					{
						if (AppSettingsValues.AllowOptionalAppConfig)
						{
							LoadOptionalProjectConfiguration();
							if (AppSettingsValues.UseConfigOverride)
							{
								LoadOptionalConfigurationOverride();
							}
						}
					}
				}
			}
			ProcessTheConfiguration();
		}

		private void ProcessRepositories()
		{
			string[] vInfo;
			foreach (KeyValuePair<string, string> vRepository in NuGetRepos.Repositories)
			{
				NuGetRepository vNuGetRepository = new NuGetRepository();
				vNuGetRepository.RepositoryName = vRepository.Key;
				vInfo =
					vRepository.Value.Split
						(Consts.SEMICOLON, StringSplitOptions.RemoveEmptyEntries);
				if ((vInfo.Length != 2) && (vInfo.Length != 4) && (vInfo.Length != 1))
				{
					continue; // Just ignore the bad entry
				}
				vNuGetRepository.HasSource = (vInfo.Length > 1);
				vNuGetRepository.HasSymbolSource = vInfo.Length == 4;
				vNuGetRepository.Source = vInfo[0];
				if (vInfo.Length > 1)
				{
					vNuGetRepository.SourceApiKey = vInfo[1];
				}
				if (vInfo.Length > 2)
				{
					vNuGetRepository.SymbolSource = vInfo[2];
					vNuGetRepository.SymbolSourceApiKey = vInfo[3];
				}
				vNuGetRepository.IsNuGetServer =
					vInfo[0].StartsWith(Consts.HTTP, StringComparison.OrdinalIgnoreCase);
				Repositories.Add(vNuGetRepository);
			}
		}

		private void ProcessDestinations()
		{
			string[] vDestinations;
			foreach (KeyValuePair<string, string> vDestination in NuGetDestinations.PushDestinations)
			{
				vDestinations =
					vDestination.Value.Split
						(Consts.SEMICOLON, StringSplitOptions.RemoveEmptyEntries);
				if (vDestinations.Length == 0)
				{
					continue;
				}
				List<NuGetRepository> vRepos = new List<NuGetRepository>();
				foreach (string vRepo in vDestinations)
				{
					NuGetRepository vFoundRepository =
						Repositories
							.FirstOrDefault
							(
								r =>
									r.RepositoryName.Equals
											(vRepo, StringComparison.OrdinalIgnoreCase)
							);
					if (vFoundRepository == null)
					{
						continue;
					}
					vRepos.Add(vFoundRepository);
				}
				Destinations.Add(vDestination.Key, vRepos);
			}
		}

		private void AssignDestination()
		{
			bool vTest =
				Destinations.Any
				(
					r =>
						r.Key.Equals
						(
							AppSettingsValues.PushToDestination
							, StringComparison.OrdinalIgnoreCase
						)
				);
			NoOp = !vTest || NoOp;
			if (vTest)
			{
				KeyValuePair<string, List<NuGetRepository>> vDestination =
					Destinations.FirstOrDefault
					(
						r =>
							r.Key.Equals
							(
								AppSettingsValues.PushToDestination
								, StringComparison.OrdinalIgnoreCase
							)
					);
				SelectedDestination.DestinationName = vDestination.Key;
				SelectedDestination.Repositories = vDestination.Value;
			}
			else
			{
				ErrorContainer.Errors.Add
					($"Invalid destination: {AppSettingsValues.PushToDestination}");
				throw new Exception("Invalid configuration.");
			}
		}

		private void ProcessNuGetServers()
		{
			ProcessRepositories();
			ProcessDestinations();
			AssignDestination();
		}

		public void ProcessConfiguration(string[] aArgs)
		{
			if (aArgs.Length == 0)
			{
				ShowHelp = HelpSections.HELP_ON_HELP;
			}
			else
			{
				LoadCommandLine(aArgs);
				if (!NoOp)
				{
					ProcessCommandLine();
					LoadInitialConfiguration();
					FetchTheConfiguration();
					if (ErrorContainer.Errors.Count > 0)
					{
						throw new Exception("Invalid configuration.");
					}
					ProcessNuGetServers();
				}
			}
		}

		public AppSettings AppSettingsValues { get; set; }
		public NuGetNuSpecValues NuGetNuSpecSettings { get; set; }
		public DotNetNuSpecValues DotNetNuSpecSettings { get; set; }
		public NuGetRepositories NuGetRepos { get; set; }
		public NuGetPushDestinations NuGetDestinations { get; set; }
		public DotNetFullFrameworkCommandSequence FullFrameworkCommands { get; set; }
		public DotNetStandard_2_0_CommandSequence Standard_2_0_Commands { get; set; }
		public DotNetCore_2_0_CommandSequence Core_2_0_Commands { get; set; }

		public List<NuGetRepository> Repositories { get; }
			= new List<NuGetRepository>();
		public Dictionary<string, List<NuGetRepository>> Destinations { get; }
			= new Dictionary<string, List<NuGetRepository>>();
		public NuGetDestination SelectedDestination { get; }
			= new NuGetDestination();

		public string ConfigFileName { get; } = _CONFIG_FILE_NAME;

	}
}
