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
	using static ApplyTokenValuesToCommandLine;
	using static Consts;
	using static ProjectFileProcessing.FrameworkInformation;
	using static TokenSetContainer;

	public interface ISpawnNugetProcesses
	{
		void Spawn(IHandleConfiguration aHandleConfiguration);
	}

	public class SpawnNugetProcesses: ISpawnNugetProcesses
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
						(_HandleConfiguration.AppSettingsValues.DotNetSimulatorExeName, DLL_EXT);
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
				case DotNetFramework.Core:
				{
					vResult = ValidateDotNetNuGetGadget();
					break;
				}
				case DotNetFramework.Standard:
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
					if (!File.Exists(vPath))
					{
						string vError =
							_HandleConfiguration.AppSettingsValues.UseSimulator
								? $"Missing NuGet simulator: {vPath}."
								: $"Missing NuGet program: {vPath}";
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

		private void PrepareForCoreProcess(NuGetRepository aNuGetRepository)
		{
			_CommandsToExecute.Clear();
			if (!aNuGetRepository.IsNuGetServer)
			{
				return;
			}
			_Command = SetupNuGetCommand();
			Dictionary<string, string> vCommands =
				_HandleConfiguration.Standard_2_0_Commands.Commands;
			foreach (KeyValuePair<string, string> vTokenizedParamLine in vCommands)
			{
				switch (vTokenizedParamLine.Key.ToLower())
				{
					case DotNetCommands.PACK:
					{
						_CommandLine =
							DotNetCommands.PACK.AsSpaceEncapsulated()
								+ BuildDotNetNuGetPack
										(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						_CommandsToExecute.Add
						(
							DotNetCommands.PACK
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case DotNetCommands.PUSH:
					{
						_CommandLine =
							_HandleConfiguration.AppSettingsValues.DotNetVerb.AsSpacePrefixed()
								+ DotNetCommands.PUSH.AsSpaceEncapsulated()
								+ BuildDotNetNuGetPush
										(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						_CommandsToExecute.Add
						(
							DotNetCommands.PUSH
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case DotNetCommands.DELETE:
					{
						_CommandLine =
							_HandleConfiguration.AppSettingsValues.DotNetVerb.AsSpacePrefixed()
								+ DotNetCommands.DELETE.AsSpaceEncapsulated()
								+ BuildDotNetNuGetDelete(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						if (aNuGetRepository.IsNuGetServer)
						{
							_CommandsToExecute.Add
							(
								DotNetCommands.DELETE
								, new Tuple<string, string>(_Command.Command, _CommandLine)
							);
						}
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException
							($"Unknown switch value: {vTokenizedParamLine.Key}");
					}
				}
			}
		}

		private void PrepareForStandardProcess(NuGetRepository aNuGetRepository)
		{
			_CommandsToExecute.Clear();
			if (!aNuGetRepository.IsNuGetServer)
			{
				return;
			}
			_Command = SetupNuGetCommand();
			Dictionary<string, string> vCommands =
				_HandleConfiguration.Standard_2_0_Commands.Commands;
			foreach (KeyValuePair<string, string> vTokenizedParamLine in vCommands)
			{
				switch (vTokenizedParamLine.Key.ToLower())
				{
					case DotNetCommands.PACK:
					{
						_CommandLine =
							DotNetCommands.PACK.AsSpaceEncapsulated()
								+ BuildDotNetNuGetPack
										(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						_CommandsToExecute.Add
						(
							DotNetCommands.PACK
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case DotNetCommands.PUSH:
					{
						_CommandLine =
							_HandleConfiguration.AppSettingsValues.DotNetVerb.AsSpacePrefixed()
								+ DotNetCommands.PUSH.AsSpaceEncapsulated()
								+ BuildDotNetNuGetPush
										(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						_CommandsToExecute.Add
						(
							DotNetCommands.PUSH
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case DotNetCommands.DELETE:
					{
						_CommandLine =
							_HandleConfiguration.AppSettingsValues.DotNetVerb.AsSpacePrefixed()
								+ DotNetCommands.DELETE.AsSpaceEncapsulated()
								+ BuildDotNetNuGetDelete
										(vTokenizedParamLine.Value);
						if (_HandleConfiguration.AppSettingsValues.UseSimulator)
						{
							_CommandLine =
								_Command.Simulator.AsSpaceEncapsulated() + _CommandLine;
						}
						if (aNuGetRepository.IsNuGetServer)
						{
							_CommandsToExecute.Add
							(
								DotNetCommands.DELETE
								, new Tuple<string, string>(_Command.Command, _CommandLine)
							);
						}
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException
							($"Unknown switch value: {vTokenizedParamLine.Key}");
					}
				}
			}
		}

		/// <summary>
		/// Even though all commands in the App.config list will be processed for
		/// content (basically assign the appropriate values for all replaceable
		/// tokens in the command line as configured), the only commands placed
		/// in the list to be spawned will be those that are appropriate to the
		/// destination (Add for a share, push for an actual NuGet server) and
		/// appropriate to the desired task e.g. a "delete" command will not be
		/// issued for a push-type request set and vice-versa.
		/// </summary>
		/// <param name="aNuGetRepository"></param>
		private void PrepareForFullFrameworkProcess
			(NuGetRepository aNuGetRepository)
		{
			_CommandsToExecute.Clear();
			_Command = SetupNuGetCommand();
			Dictionary<string, string> vCommands =
				_HandleConfiguration.FullFrameworkCommands.Commands;
			foreach (KeyValuePair<string, string> vTokenizedParamLine in vCommands)
			{
				switch (vTokenizedParamLine.Key.ToLower())
				{
					case NuGetCommands.SPEC:
					{
						_CommandLine =
							NuGetCommands.SPEC.AsSpaceEncapsulated()
								+ BuildNuGetSpec
										(vTokenizedParamLine.Value);
						_CommandsToExecute.Add
						(
							NuGetCommands.SPEC
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case NuGetCommands.PACK:
					{
						_CommandLine =
							NuGetCommands.PACK.AsSpaceEncapsulated()
								+ BuildNuGetPack
									(
										vTokenizedParamLine.Value
										, _HandleConfiguration.AppSettingsValues.UseNuSpecFileIfAvailable)
									;
						_CommandsToExecute.Add
						(
							NuGetCommands.PACK
							, new Tuple<string, string>(_Command.Command, _CommandLine)
						);
						break;
					}
					case NuGetCommands.PUSH:
					{
						_CommandLine =
							NuGetCommands.PUSH.AsSpaceEncapsulated()
								+ BuildNuGetPush
										(vTokenizedParamLine.Value);
						if (aNuGetRepository.IsNuGetServer)
						{
							_CommandsToExecute.Add
							(
								NuGetCommands.PUSH
								, new Tuple<string, string>(_Command.Command, _CommandLine)
							);
						}
						break;
					}
					case NuGetCommands.ADD:
					{
						_CommandLine =
							NuGetCommands.ADD.AsSpaceEncapsulated()
								+ BuildNuGetAdd
										(vTokenizedParamLine.Value);
						if (!aNuGetRepository.IsNuGetServer)
						{
							_CommandsToExecute.Add
							(
								NuGetCommands.ADD
								, new Tuple<string, string>(_Command.Command, _CommandLine)
							);
						}
						break;
					}
					case NuGetCommands.DELETE:
					{
						_CommandLine =
							NuGetCommands.DELETE.AsSpaceEncapsulated()
								+ BuildNuGetDelete
										(vTokenizedParamLine.Value);
						if (aNuGetRepository.IsNuGetServer)
						{
							_CommandsToExecute.Add
							(
								NuGetCommands.DELETE
								, new Tuple<string, string>(_Command.Command, _CommandLine)
							);
						}
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException
							($"Unknown switch value: {vTokenizedParamLine.Key}");
					}
				}
			}
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

		private string ExtractSummary()
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
						? CommandLineSettings.ProjectName
						: String.Empty;
			}
			return vResult;
		}

		/// <summary>
		/// In comments below this method is a method that will attempt to load the
		/// .nuspec file as an XDocument and process it as such. This seems to be a
		/// bit of overkill in that MS has very nicely included a default .nuspec
		/// file if the nuget "spec" command is executed in the same directory as
		/// the target .csproj file. Since this is the case AND said .nuspec file
		/// contains replaceable tokens, it's really easier to simply load the 
		/// entire file into a StringBuilder, replace the desired tokens with the
		/// established values and then write the thing back out to the drive, thus
		/// this method.
		/// </summary>
		private void ProcessFullFrameworkNuSpecFileForPack()
		{
			const StringComparison COMPARISON =
				StringComparison.InvariantCultureIgnoreCase;
			NoOp = !File.Exists(NuSpecFilePath) || NoOp;
			if (NoOp)
			{
				return;
			}
			string vPath = NuSpecFilePath;
			StringBuilder vContent = new StringBuilder(File.ReadAllText(vPath));
			bool vChanged = false;
			string vLookFor;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceAuthors)
			{
				vLookFor =
					NuGetNuSpecKeys.AUTHOR_KEY + NuGetNuSpecKeys.AUTHORS.AsToken();
				vContent.Replace
				(
					vLookFor
					, NuGetNuSpecKeys.AUTHOR_KEY
							+ _HandleConfiguration.NuGetNuSpecSettings.Authors
				);
				vChanged = true;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceCopyright)
			{
				vLookFor = $"{NuGetNuSpecKeys.COPYRIGHT} {DateTime.Now.Year}";
				vContent.Replace
					(vLookFor.AsToken(), _HandleConfiguration.NuGetNuSpecSettings.Copyright);
				vChanged = true;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceDescription)
			{
				vLookFor = NuGetNuSpecKeys.DESCRIPTION.AsToken();
				vContent.Replace
					(vLookFor.AsToken(), _HandleConfiguration.NuGetNuSpecSettings.Description);
				vChanged = true;
			}
			vLookFor = NuGetNuSpecKeys.ICON_URL_KEY + NuGetNuSpecKeys.DELETE_THIS;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceIconUrl)
			{
				vContent.Replace(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.IconUrl);
				vChanged = true;
			}
			else
			{
				string vFind =
					NuGetNuSpecKeys.ICON_URL_KEY
						+ NuGetNuSpecKeys.DELETE_THIS
						+ NuGetNuSpecKeys.ICON_URL_KEY_END;
				vContent.Replace(vFind, String.Empty);
			}
			vLookFor = NuGetNuSpecKeys.LICENSE_URL;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceLicenseUrl)
			{
				vContent.Replace
					(vLookFor.AsToken(), _HandleConfiguration.NuGetNuSpecSettings.LicenseUrl);
				vChanged = true;
			}
			else
			{
				string vFind =
					NuGetNuSpecKeys.LICENSE_URL_KEY
					+ NuGetNuSpecKeys.DELETE_THIS
					+ NuGetNuSpecKeys.LICENSE_URL_KEY_END;
				vContent.Replace(vFind, String.Empty);
			}
			vLookFor = NuGetNuSpecKeys.PROJECT_URL;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceProjectUrl)
			{
				vContent.Replace
					(vLookFor.AsToken(), _HandleConfiguration.NuGetNuSpecSettings.ProjectUrl);
				vChanged = true;
			}
			else
			{
				string vFind =
					NuGetNuSpecKeys.PROJECT_URL_KEY
					+ NuGetNuSpecKeys.DELETE_THIS
					+ NuGetNuSpecKeys.PROJECT_URL_KEY_END;
				vContent.Replace(vFind, String.Empty);
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceOwners)
			{
				vLookFor =
					NuGetNuSpecKeys.OWNER_KEY + NuGetNuSpecKeys.AUTHORS.AsToken();
				vContent.Replace
				(
					vLookFor
					, NuGetNuSpecKeys.OWNER_KEY
							+ _HandleConfiguration.NuGetNuSpecSettings.Owners
				);
				vChanged = true;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceReleaseNotes)
			{
				vLookFor = NuGetNuSpecKeys.DEFAULT_RELEASE_NOTES;
				if (_HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile)
				{
					vContent.Replace(vLookFor, ExtractReleaseNotes());
					vChanged = true;
				}
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceRequireLicenseAcceptance)
			{
				vLookFor =
					NuGetNuSpecKeys.REQUIRE_LICENSE_ACCEPTANCE_KEY
						+ NuGetNuSpecKeys.DEFAULT_REQUIRE_LICENSE_ACCEPTANCE;
				vContent.Replace
					(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.RequireLicenseAcceptance);
				vChanged = true;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceSummary)
			{
				vLookFor = NuGetNuSpecKeys.SUMMARY_KEY;
				string vContentAsString = vContent.ToString();
				int vIndex = vContentAsString.IndexOf(vLookFor, COMPARISON);
				if (vIndex < 0)
				{
					vIndex = vContentAsString.IndexOf
						(NuGetNuSpecKeys.METADATA_END, COMPARISON);
					string vNewKey =
						NuGetNuSpecKeys.SUMMARY_KEY
							+ ExtractSummary()
							+ NuGetNuSpecKeys.SUMMARY_KEY_END;
					vContent.Insert(vIndex, vNewKey);
				}
				else
				{
					// This shouldn't ever have to be executed, but just in case MS 
					// changes how nuget.exe functions...
					int vEndIndex =
						vContentAsString.IndexOf
							(NuGetNuSpecKeys.SUMMARY_KEY_END, COMPARISON);
					vIndex = vIndex + NuGetNuSpecKeys.SUMMARY_KEY.Length;
					int vHowMuch = vEndIndex - vIndex + 1;
					vContentAsString = vContentAsString.Remove(vIndex, vHowMuch);
					vContentAsString = vContentAsString.Insert(vIndex, ExtractSummary());
					vContent.Clear();
					vContent.Append(vContentAsString);
				}
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceTags)
			{
				vLookFor = NuGetNuSpecKeys.DEFAULT_TAGS;
				vContent.Replace(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.Tags);
				vChanged = true;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceTitle)
			{
				vLookFor = NuGetNuSpecKeys.TITLE.AsToken();
				vContent.Replace(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.Title);
				vChanged = true;
			}
			if (vChanged)
			{
				File.WriteAllText(vPath, vContent.ToString());
			}
		}

		//private void ProcessTheFullFrameworkNuSpecFileForPack()
		//{
		//	CommandLineSettings.NoOp =
		//		!File.Exists(_HandleConfiguration.TokenSet.NuSpecFilePath)
		//			|| CommandLineSettings.NoOp;
		//	if (CommandLineSettings.NoOp)
		//	{
		//		return;
		//	}
		//	string vPath = _HandleConfiguration.TokenSet.NuSpecFilePath;
		//	XDocument vDoc = vPath.LoadAsXDocument();
		//	bool vChanged = false;
		//	vDoc.Changed += (sender, args) => vChanged = true;
		//	XElement vParent = FrameworkInformation.NodeParent;
		//	StringComparison vComparison = StringComparison.OrdinalIgnoreCase;
		//	XElement vElement;
		//	string vLookFor;
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceAuthors)
		//	{
		//		vLookFor = NuGetNuSpecKeys.AUTHORS;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.AUTHORS
		//			, _HandleConfiguration.NuGetNuSpecSettings.Authors
		//		);
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceCopyright)
		//	{
		//		vLookFor = NuGetNuSpecKeys.COPYRIGHT;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.COPYRIGHT
		//			, _HandleConfiguration.NuGetNuSpecSettings.Copyright
		//		);
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceDescription)
		//	{
		//		vLookFor = NuGetNuSpecKeys.DESCRIPTION;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.DESCRIPTION
		//			, _HandleConfiguration.NuGetNuSpecSettings.Description
		//		);
		//	}
		//	vLookFor = NuGetNuSpecKeys.ICON_URL;
		//	vElement = FindElement(vDoc, vLookFor);
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceIconUrl)
		//	{
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.ICON_URL
		//			, _HandleConfiguration.NuGetNuSpecSettings.IconUrl
		//		);
		//	}
		//	else
		//	{
		//		bool vTest =
		//			(vElement != null)
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.DELETE_THIS, vComparison);
		//		if (vTest)
		//		{
		//			vElement.Remove();
		//		}
		//	}
		//	vLookFor = NuGetNuSpecKeys.LICENSE_URL;
		//	vElement = FindElement(vDoc, vLookFor);
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceLicenseUrl)
		//	{
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.LICENSE_URL
		//			, _HandleConfiguration.NuGetNuSpecSettings.LicenseUrl
		//		);
		//	}
		//	else
		//	{
		//		bool vTest =
		//			(vElement != null)
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.DELETE_THIS, vComparison);
		//		if (vTest)
		//		{
		//			vElement.Remove();
		//		}
		//	}
		//	vLookFor = NuGetNuSpecKeys.PROJECT_URL;
		//	vElement = FindElement(vDoc, vLookFor);
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceProjectUrl)
		//	{
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.AUTHORS
		//			, _HandleConfiguration.NuGetNuSpecSettings.ProjectUrl
		//		);
		//	}
		//	else
		//	{
		//		bool vTest =
		//			(vElement != null)
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.DELETE_THIS, vComparison);
		//		if (vTest)
		//		{
		//			vElement.Remove();
		//		}
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceOwners)
		//	{
		//		vLookFor = NuGetNuSpecKeys.OWNERS;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.OWNERS
		//			, _HandleConfiguration.NuGetNuSpecSettings.Owners
		//		);
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceReleaseNotes)
		//	{
		//		vLookFor = NuGetNuSpecKeys.RELEASE_NOTES;
		//		vElement = FindElement(vDoc, vLookFor);
		//		if (_HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile)
		//		{
		//			_HandleConfiguration.NuGetNuSpecSettings.ReleaseNotes = ExtractReleaseNotes();
		//			ProcessElement
		//			(
		//				vParent
		//				, vElement
		//				, NuGetNuSpecKeys.RELEASE_NOTES
		//				, _HandleConfiguration.NuGetNuSpecSettings.ReleaseNotes
		//			);
		//		}
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceRequireLicenseAcceptance)
		//	{
		//		vLookFor = NuGetNuSpecKeys.REQUIRE_LICENSE_ACCEPTANCE;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.REQUIRE_LICENSE_ACCEPTANCE
		//			, _HandleConfiguration.NuGetNuSpecSettings.RequireLicenseAcceptance
		//		);
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceSummary)
		//	{
		//		vLookFor = NuGetNuSpecKeys.SUMMARY;
		//		vElement = FindElement(vDoc, vLookFor);
		//		if (_HandleConfiguration.AppSettingsValues.RequireSummaryFile)
		//		{
		//			_HandleConfiguration.NuGetNuSpecSettings.Summary = ExtractSummary();
		//			ProcessElement
		//			(
		//				vParent
		//				, vElement
		//				, NuGetNuSpecKeys.SUMMARY
		//				, _HandleConfiguration.NuGetNuSpecSettings.Summary
		//			);
		//		}
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceTags)
		//	{
		//		vLookFor = NuGetNuSpecKeys.TAGS;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.TAGS
		//			, _HandleConfiguration.NuGetNuSpecSettings.Tags
		//		);
		//	}
		//	if (_HandleConfiguration.NuGetNuSpecSettings.ForceTitle)
		//	{
		//		vLookFor = NuGetNuSpecKeys.TITLE;
		//		vElement = FindElement(vDoc, vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, NuGetNuSpecKeys.TITLE
		//			, _HandleConfiguration.NuGetNuSpecSettings.Title
		//		);
		//	}
		//	if (vChanged)
		//	{
		//		vDoc.Save(vPath);
		//	}
		//}

		private void ProcessTheStandardProjectFileForPack()
		{
			if (NoOp)
			{
				return;
			}
			string vPath = CommandLineSettings.ProjectPath;
			//_XDoc = XDocument.Load(CommandLineSettings.ProjectPath);
			_XDoc = NodeDocument;
			bool vChanged = false;
			_XDoc.Changed += (sender, args) => vChanged = true;
			XElement vParent = NodeParent;
			string vLookFor;
			XElement vElement;
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceAssemblyVersion)
			{
				vLookFor = DotNetCSProjKeys.ASSEMBLY_VERSION;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.ASSEMBLY_VERSION
					, _HandleConfiguration.DotNetNuSpecSettings.AssemblyVersion
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceAuthors)
			{
				vLookFor = DotNetCSProjKeys.AUTHORS;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.AUTHORS
					, _HandleConfiguration.DotNetNuSpecSettings.Authors
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceCompany)
			{
				vLookFor = DotNetCSProjKeys.COMPANY;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.COMPANY
					, _HandleConfiguration.DotNetNuSpecSettings.Company
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceCopyright)
			{
				vLookFor = DotNetCSProjKeys.COPYRIGHT;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.COPYRIGHT
					, _HandleConfiguration.DotNetNuSpecSettings.Copyright
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceDescription)
			{
				vLookFor = DotNetCSProjKeys.DESCRIPTION;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.DESCRIPTION
					, _HandleConfiguration.DotNetNuSpecSettings.Description
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceFileVersion)
			{
				vLookFor = DotNetCSProjKeys.FILE_VERSION;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.FILE_VERSION
					, _HandleConfiguration.DotNetNuSpecSettings.FileVersion
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageIconUrl)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_ICON_URL;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PACKAGE_ICON_URL
					, _HandleConfiguration.DotNetNuSpecSettings.PackageIconUrl
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageLicenseUrl)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_LICENSE_URL;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PACKAGE_LICENSE_URL
					, _HandleConfiguration.DotNetNuSpecSettings.PackageLicenseUrl
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageProjectUrl)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_PROJECT_URL;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PACKAGE_PROJECT_URL
					, _HandleConfiguration.DotNetNuSpecSettings.PackageProjectUrl
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageReleaseNotes)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_RELEASE_NOTES;
				vElement = _XDoc.FindElement(vLookFor);
				if (_HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile)
				{
					_HandleConfiguration.DotNetNuSpecSettings.PackageReleaseNotes =
						ExtractReleaseNotes();
					ProcessElement
					(
						vParent
						, vElement
						, DotNetCSProjKeys.PACKAGE_RELEASE_NOTES
						, _HandleConfiguration.DotNetNuSpecSettings.PackageReleaseNotes
					);
				}
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageRequireLicenseAcceptance)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_REQUIRE_LICENSE_ACCEPTANCE;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PACKAGE_REQUIRE_LICENSE_ACCEPTANCE
					, _HandleConfiguration.DotNetNuSpecSettings.PackageRequireLicenseAcceptance
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForcePackageTags)
			{
				vLookFor = DotNetCSProjKeys.PACKAGE_TAGS;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PACKAGE_TAGS
					, _HandleConfiguration.DotNetNuSpecSettings.PackageTags
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceProduct)
			{
				vLookFor = DotNetCSProjKeys.PRODUCT;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PRODUCT
					, _HandleConfiguration.DotNetNuSpecSettings.Product
				);
			}
			if (_HandleConfiguration.DotNetNuSpecSettings.ForceVersion)
			{
				vLookFor = DotNetCSProjKeys.VERSION;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.VERSION
					, _HandleConfiguration.DotNetNuSpecSettings.Version
				);
			}
			if (vChanged)
			{
				_XDoc.Save(vPath);
			}
		}

		private void ProcessTheCoreProjectFileForPack()
		{
			// As of this writing, the two methods of handling the different core
			// and standard projects are identical. This method is provided only as a
			// place to start should the two ever diverge.
			ProcessTheStandardProjectFileForPack();
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
					ErrorContainer.Errors.Add($"ARguments: {vInfo.Arguments}");
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

		/// <summary>
		/// Layout of deletion entry is:
		///		Guid, Source, ApiKey, PackageName, PackageVersion
		///
		///		- Guid is the name of the file without extension.
		///		- Source is the location (NuGetServer or Local Share) that the package
		///			was uploaded to.
		///		- ApiKey is the API key applied to the NuGetServer. If the destination
		///			is a Local Share, this entry will be "No API Key".
		///		- Package name is the name of the package with no extension.
		///		- Package Version is the version of the package as uploaded to the
		///			destination.
		/// </summary>
		private void WriteDeletionEntry(bool aIsNuGetServer)
		{
			Guid vGuid = Guid.NewGuid();
			string vApiKey =
				aIsNuGetServer
					? ApiKey
					: _NO_API_KEY;
			string vLine =
				$"{vGuid}\t{Source}\t{vApiKey}\t{PackageName}\t{TokenSetContainer.PackageVersion}";
			string vPath =
				_HandleConfiguration.AppSettingsValues.PackageHomeDir
					+ vGuid.ToString("N")
					+ DELETE_FILE_EXT;
			File.WriteAllLines(vPath, new[] { vLine });
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
				case DotNetFramework.Core:
				{
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
				case DotNetFramework.Standard:
				{
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
					vContinue =
						vContinue && _CommandsToExecute.ContainsKey(NuGetCommands.ADD);
					if (vContinue)
					{
						vCommand = _CommandsToExecute[NuGetCommands.ADD].Item1;
						vCommandLine = _CommandsToExecute[NuGetCommands.ADD].Item2;
						vContinue =
							SpawnAProcess(vCommand, vCommandLine, NuGetCommand.NuGetAdd);
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

		private void MergeDeletionEntries()
		{
			Dictionary<string, string> vDuplicates = new Dictionary<string, string>();
			string vPath =
				PackageDir
					+ _HandleConfiguration.AppSettingsValues.DefaultDeleteFileName;
			if (File.Exists(vPath))
			{
				File.Delete(vPath);
			}
			IEnumerable<string> vFiles =
				Directory.EnumerateFiles(PackageDir, "*" + DELETE_FILE_EXT);
			StringBuilder vContent = new StringBuilder();
			string vInfo;
			foreach (string vFile in vFiles)
			{
				vInfo = File.ReadLines(vFile).FirstOrDefault();
				if (String.IsNullOrWhiteSpace(vInfo))
				{
					continue;
				}
				string[] vPieces = vInfo.Split('\t');
				// Skip the duplicate entries. Shouldn't normally happen
				// but when debugging it's possible to get multiple entries
				// with the same package name and version so...skip 'em.
				bool vTest =
					vDuplicates.ContainsKey(vPieces[3])
						&& vDuplicates.ContainsKey(vPieces[4]);
				if (vTest)
				{
					continue;
				}
				vContent.AppendLine(vInfo);
			}
			if (vContent.Length > 0)
			{
				File.WriteAllText(vPath, vContent.ToString());
			}
		}

		private void PerformSelectedDeletions()
		{
			// Do Something different here to process the .del file list.
			/*
			 For each spawned process, record the necessary info (package name, version, destination, etc.) in a file named <some GUID>.del.
			 When launching the deletion process, consolidate all of the entries into a single "DeleteThese.del" file via a switch to select consolidation.
			 Then, after the user has a chance to edit the file, run the content of the .del file (contains the guid, package name, package version and the destination)
			 upon successful deletion, delete the corresponding <guid>.del file to remove it from future consolidations, etc. This remotes the necessity
			 for file locking, etc. It should "Just Work".
			 */
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
				case DotNetFramework.Core:
				{
					ProcessTheCoreProjectFileForPack();
					foreach (NuGetRepository vRepository in vList)
					{
						UpdateTokenSet(vRepository);
						PrepareForCoreProcess(vRepository);
						WriteLine($"\nRepository: {vRepository.RepositoryName}\n");
						LaunchTheProcesses(vRepository);
					}
					break;
				}
				case DotNetFramework.Standard:
				{
					ProcessTheStandardProjectFileForPack();
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
					ProcessFullFrameworkNuSpecFileForPack();
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
