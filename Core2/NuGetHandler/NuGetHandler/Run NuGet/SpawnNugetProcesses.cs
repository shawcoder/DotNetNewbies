namespace NuGetHandler.Run_NuGet
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml.Linq;
	using AppConfigHandling;
	using ConfigurationHandler;
	using Infrastructure;
	using static System.Console;
	using static AppConfigHandling.CommandLineSettings;
	using static Consts;
	using static ProjectFileProcessing.FrameworkInformation;
	using static TokenSetContainer;

	public interface ISpawnNugetProcesses
	{
		void Spawn(IHandleConfiguration aHandleConfiguration);
	}

	public partial class SpawnNuGetProcesses: ISpawnNugetProcesses
	{
		private const string _PREFIX_SUFFIX = "//";
		private const string _HEADER =
			_PREFIX_SUFFIX + " Build Event took place on";
		private const string _NO_API_KEY = "No Api Key";

		private IHandleConfiguration _HandleConfiguration;
		private readonly Dictionary<string, Tuple<string, string>> _CommandsToExecute
			= new Dictionary<string, Tuple<string, string>>();
		private (string Command, string Simulator) _Command;
		private string _CommandLine;
		private XDocument _XDoc;

		private (string, string) ValidateDotNetNuGetGadget()
		{
			(string, string) vResult;
			string vPath =
				Environment.ExpandEnvironmentVariables
					(_HandleConfiguration.AppSettingsValues.DotNetDir).AsDir();
			vPath +=
				Path.ChangeExtension
					(_HandleConfiguration.AppSettingsValues.DotNetName, EXE_EXT);
			if (!File.Exists(vPath))
			{
				throw new Exception($"Missing .NET Core host: {vPath}.");
			}
			if (_HandleConfiguration.AppSettingsValues.UseSimulator)
			{
				string vSimulator =
					Environment.ExpandEnvironmentVariables
						(_HandleConfiguration.AppSettingsValues.DotNetSimulatorDir).AsDir()
						+ Path.ChangeExtension
							(
								_HandleConfiguration.AppSettingsValues.DotNetSimulatorExeName
								, DLL_EXT
							);
				if (!File.Exists(vSimulator))
				{
					throw new Exception($"Missing dotnet simulator: {vPath}");
				}
				vResult = (vPath, vSimulator.AsSpacePrefixed());
			}
			else
			{
				vResult = (vPath, String.Empty);
			}
			return vResult;
		}

		private (string, string) SetupNuGetCommand()
		{
			(string, string) vResult;
			// We are building JUST the portion of the command that will be called
			// e.g. NuGet.exe or dotnet.exe, NOTHING ELSE!
			switch (Framework)
			{
				case DotNetFramework.Unknown:
				{
					vResult = (String.Empty, String.Empty); // Don't do anything and don't throw an exception.
					break;
				}
				case DotNetFramework.Core_2_0:
				{
					vResult = ValidateDotNetNuGetGadget();
					break;
				}
				case DotNetFramework.Core_2_1:
				{
					vResult = ValidateDotNetNuGetGadget();
					break;
				}
				case DotNetFramework.Standard_2_0:
				{
					vResult = ValidateDotNetNuGetGadget();
					break;
				}
				case DotNetFramework.Full:
				{
					string vPath =
						_HandleConfiguration.AppSettingsValues.UseSimulator
							? Environment.ExpandEnvironmentVariables
									(_HandleConfiguration.AppSettingsValues.NuGetSimulatorDir).AsDir()
							: Environment.ExpandEnvironmentVariables
									(_HandleConfiguration.AppSettingsValues.NuGetDir).AsDir();
					vResult =
						(
							vPath
								+ (
										_HandleConfiguration.AppSettingsValues.UseSimulator
												? Path.ChangeExtension
														(_HandleConfiguration.AppSettingsValues.NuGetSimulatorExeName, EXE_EXT)
												: Path.ChangeExtension
														(_HandleConfiguration.AppSettingsValues.NuGetExeName, EXE_EXT)
									)
							, String.Empty
						);
					if (!File.Exists(vResult.Item1))
					{
						string vError =
							_HandleConfiguration.AppSettingsValues.UseSimulator
								? $"Missing NuGet simulator: {vResult.Item1}."
								: $"Missing NuGet program: {vResult.Item1}";
						throw new Exception(vError);
					}
					break;
				}
				default:
				{
					vResult = (String.Empty, String.Empty); // Don't do anything and don't throw an exception.
					break;
				}
			}
			return vResult;
		}

		/// <summary>
		/// Update the following:
		/// Source
		/// ApiKey
		/// SymbolSource
		/// SymbolApiKey
		/// </summary>
		/// <param name="aRepository"></param>
		private void UpdateTokenSet(NuGetRepository aRepository)
		{
			Source = aRepository.Source;
			ApiKey = aRepository.SourceApiKey;
			SymbolSource = aRepository.SymbolSource;
			SymbolApiKey = aRepository.SymbolSourceApiKey;
		}

		private void ProcessElement
			(XElement aParent, XElement aElement, XName aName, string aValue)
		{
			if (aElement != null)
			{
				aElement.Value = aValue;
			}
			else
			{
				aParent.Nodes().Append(new XElement(aName, aValue));
			}
		}

		private string ExtractReleaseNotes()
		{
			string vPath =
				Path.Combine
				(
					ProjectDir
					, RELEASE_NOTES_FILE_NAME + TXT_EXT
				);
			string vResult;
			if (File.Exists(vPath))
			{
				StringBuilder vContent = new StringBuilder(File.ReadAllText(vPath));
				string vLine = $"{_HEADER} {DateTime.Now:D}.\n";
				vContent.Insert(0, vLine);
				vContent.Insert(1, "\n");
				vResult = vContent.ToString();
				File.WriteAllText(vPath, vResult);
			}
			else
			{
				if (_HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile)
				{
					if (_HandleConfiguration.AppSettingsValues.InjectDefaultReleaseNotes)
					{
						vResult =
							_HandleConfiguration.AppSettingsValues.DefaultReleaseNotes
								+ $"{CommandLineSettings.ProjectName} on "
								+ $"{DateTime.Now:D}.";
					}
					else
					{
						WriteLine
							($"Missing {RELEASE_NOTES_FILE_NAME} file for project {CommandLineSettings.ProjectName}.");
						vResult = String.Empty;
					}
				}
				else
				{
					vResult = String.Empty;
				}
			}
			return vResult;
		}

		private string ExtractNuGetSummary()
		{
			string vPath =
				Path.Combine
				(
					ProjectDir
					, SUMMARY_FILE_NAME + TXT_EXT
				);
			string vResult;
			if (File.Exists(vPath))
			{
				vResult = File.ReadAllText(vPath);
			}
			else
			{
				vResult =
					_HandleConfiguration.AppSettingsValues.RequireSummaryFile
						? _HandleConfiguration.NuGetNuSpecSettings.ForceSummary
								? _HandleConfiguration.NuGetNuSpecSettings.Summary
								: String.Empty
						: String.Empty;
			}
			return vResult;
		}

		private string ExtractDotNetSummary()
		{
			string vPath =
				Path.Combine
				(
					ProjectDir
					, SUMMARY_FILE_NAME + TXT_EXT
				);
			string vResult;
			if (File.Exists(vPath))
			{
				vResult = File.ReadAllText(vPath);
			}
			else
			{
				vResult =
					_HandleConfiguration.AppSettingsValues.RequireSummaryFile
						? _HandleConfiguration.DotNetNuSpecSettings.ForceProduct
								? _HandleConfiguration.DotNetNuSpecSettings.Product
								: String.Empty
						: String.Empty;
			}
			return vResult;
		}

		private bool SpawnAProcess
			(string aCommand, string aCommandParameters, NuGetCommand aNuGetCommand)
		{
			bool vResult;
			ProcessStartInfo vInfo = new ProcessStartInfo();
			vInfo.WindowStyle =
				ProcessWindowStyle
					.Normal; // Change this to hidden when done fiddling with the app
			vInfo.FileName = aCommand;
			vInfo.Arguments = aCommandParameters;
			vInfo.CreateNoWindow = false;
			vInfo.UseShellExecute = false;
			vInfo.LoadUserProfile = true;
			bool vTest =
				(VerbosityLevel == VerbosityE.Detailed)
					|| (VerbosityLevel == VerbosityE.Normal);
			if (vTest)
			{
				WriteLine($"\n{new String('*', 80)}\n");
				WriteLine($"\nCommand: {aNuGetCommand}");
				WriteLine($"Parameters: {vInfo.Arguments}\n");
			}
			switch (aNuGetCommand)
			{
				case NuGetCommand.NuGetSpec:
				{
					vInfo.WorkingDirectory = ProjectDir;
					break;
				}
				case NuGetCommand.NuGetPack:
				{
					vInfo.WorkingDirectory = ProjectDir;
					break;
				}
				case NuGetCommand.DotNetPack:
				{
					vInfo.WorkingDirectory = ProjectDir;
					break;
				}
			}
			if (!NoSpawn)
			{
				try
				{
					Process vProcess = Process.Start(vInfo);
					vProcess?.WaitForExit();
					vResult = vProcess?.ExitCode == 0;
				}
				catch (Exception)
				{
					ErrorContainer.Errors.Add($"Command: {vInfo.FileName}");
					ErrorContainer.Errors.Add($"Arguments: {vInfo.Arguments}");
					throw;
				}
			}
			else
			{
				WriteLine
					($"Process would be spawned. Command: {vInfo.FileName}, \nParameters: {vInfo.Arguments}\n");
				vResult = true;
			}
			return vResult;
		}

		private void LaunchTheProcesses(NuGetRepository aNuGetRepository)
		{
			if (NoOp)
			{
				WriteLine("\n***** Begin Spawning Processes *****\n");
			}
			string vCommand;
			string vCommandLine;
			// The following defined the sequence in which the now-ready-to-execute
			// commands should be called.
			bool vContinue;
			switch (Framework)
			{
				case DotNetFramework.Unknown:
				{
					return; // Don't do anything and don't throw an exception.
				}
				case DotNetFramework.Core_2_0:
				{
					ProcessTheCoreProjectFileForPack();
					vContinue = _CommandsToExecute.ContainsKey(DotNetCommands.PACK);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PACK].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PACK].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.DotNetPack);
					}
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(DotNetCommands.PUSH);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PUSH].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PUSH].Item2;
						vContinue =
							SpawnAProcess
								(vCommand, vCommandLine, NuGetCommand.DotNetNuGetPush);
					}
					break;
				}
				case DotNetFramework.Core_2_1:
				{
					ProcessTheCoreProjectFileForPack();
					vContinue = _CommandsToExecute.ContainsKey(DotNetCommands.PACK);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PACK].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PACK].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.DotNetPack);
					}
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(DotNetCommands.PUSH);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PUSH].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PUSH].Item2;
						vContinue =
							SpawnAProcess
								(vCommand, vCommandLine, NuGetCommand.DotNetNuGetPush);
					}
					break;
				}
				case DotNetFramework.Standard_2_0:
				{
					ProcessTheStandardProjectFileForPack();
					vContinue = _CommandsToExecute.ContainsKey(DotNetCommands.PACK);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PACK].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PACK].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.DotNetPack);
					}
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(DotNetCommands.PUSH);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[DotNetCommands.PUSH].Item1;
						vCommandLine = _CommandsToExecute[DotNetCommands.PUSH].Item2;
						vContinue =
							SpawnAProcess
								(vCommand, vCommandLine, NuGetCommand.DotNetNuGetPush);
					}
					break;
				}
				case DotNetFramework.Full:
				{
					vContinue = _CommandsToExecute.ContainsKey(NuGetCommands.SPEC);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[NuGetCommands.SPEC].Item1;
						vCommandLine = _CommandsToExecute[NuGetCommands.SPEC].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.NuGetSpec);
					}
					ProcessFullFrameworkNuSpecFileForPack();
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(NuGetCommands.PACK);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[NuGetCommands.PACK].Item1;
						vCommandLine = _CommandsToExecute[NuGetCommands.PACK].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.NuGetPack);
					}
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(NuGetCommands.PUSH);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[NuGetCommands.PUSH].Item1;
						vCommandLine = _CommandsToExecute[NuGetCommands.PUSH].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.NuGetPush);
					}
					else
					{
						vContinue = _CommandsToExecute.ContainsKey(NuGetCommands.ADD);
						if (vContinue)
						{
							vCommand = _CommandsToExecute[NuGetCommands.ADD].Item1;
							vCommandLine = _CommandsToExecute[NuGetCommands.ADD].Item2;
							vContinue =
								SpawnAProcess(vCommand, vCommandLine, NuGetCommand.NuGetAdd);
						}
					}
					if (_HandleConfiguration.AppSettingsValues.DeleteNuSpecFileAfterProcessing)
					{
						if (File.Exists(NuSpecFilePath))
						{
							File.Delete(NuSpecFilePath);
						}
					}
					break;
				}
				default:
				{
					return; // Don't do anything and don't throw an exception.
				}
			}
			// The command sequence was successful so, write the values for the
			// destination, package name and version to the .del file
			if (vContinue)
			{
				WriteDeletionEntry(aNuGetRepository.IsNuGetServer);
			}
			bool vTest =
				(VerbosityLevel == VerbosityE.Detailed)
					|| (VerbosityLevel == VerbosityE.Normal);
			if (vTest)
			{
				string vLine =
					vContinue
						? "Spawning process completed successfully."
						: "Spawning process failed miserably.";
				WriteLine($"{vLine}");
			}

			if (NoOp)
			{
				WriteLine("\n***** End Spawning Processes *****\n");
			}
		}

		private void ProcessTheFrameworkCommands()
		{
			List<NuGetRepository> vList =
				_HandleConfiguration.SelectedDestination.Repositories;
			switch (Framework)
			{
				case DotNetFramework.Unknown:
				{
					return; // Don't do anything and don't throw an exception.
				}
				case DotNetFramework.Core_2_0:
				{
					foreach (NuGetRepository vRepository in vList)
					{
						UpdateTokenSet(vRepository);
						PrepareForCoreProcess(vRepository);
						WriteLine($"\nRepository: {vRepository.RepositoryName}\n");
						LaunchTheProcesses(vRepository);
					}
					break;
				}
				case DotNetFramework.Core_2_1:
				{
					foreach (NuGetRepository vRepository in vList)
					{
						UpdateTokenSet(vRepository);
						PrepareForCoreProcess(vRepository);
						WriteLine($"\nRepository: {vRepository.RepositoryName}\n");
						LaunchTheProcesses(vRepository);
					}
					break;
				}
				case DotNetFramework.Standard_2_0:
				{
					foreach (NuGetRepository vRepository in vList)
					{
						UpdateTokenSet(vRepository);
						PrepareForStandardProcess(vRepository);
						WriteLine($"\nRepository: {vRepository.RepositoryName}\n");
						LaunchTheProcesses(vRepository);
					}
					break;
				}
				case DotNetFramework.Full:
				{
					foreach (NuGetRepository vRepository in vList)
					{
						UpdateTokenSet(vRepository);
						PrepareForFullFrameworkProcess(vRepository);
						WriteLine($"\nRepository: {vRepository.RepositoryName}\n");
						LaunchTheProcesses(vRepository);
					}
					break;
				}
				default:
				{
					return; // Don't do anything and don't throw an exception.
				}
			}
		}

		public void Spawn(IHandleConfiguration aHandleConfiguration)
		{
			_HandleConfiguration =
				aHandleConfiguration
					?? throw new ArgumentNullException(nameof(_HandleConfiguration));
			if (MergeDeletions)
			{
				MergeDeletionEntries();
			}
			else
			{
				if (PerformDeletes)
				{
					PerformSelectedDeletions();
				}
				else
				{
					ProcessTheFrameworkCommands();
				}
			}
		}

	}
}
