namespace NuGetHandler.Run_NuGet
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;
	using AppConfigHandling;
	using Infrastructure;
	using static AppConfigHandling.CommandLineSettings;
	using static ApplyTokenValuesToCommandLine;
	using static ProjectFileProcessing.FrameworkInformation;

	public partial class SpawnNuGetProcesses
	{
		private void PrepareForStandardProcess(NuGetRepository aNuGetRepository)
		{
			_CommandsToExecute.Clear();
			if (!aNuGetRepository.IsNuGetServer)
			{
				return;
			}
			_Command = SetupNuGetCommand();
			Dictionary<string, string> vCommands;
			switch (Framework)
			{
				case DotNetFramework.Standard_2_0:
				{
					vCommands = _HandleConfiguration.Standard_2_0_Commands.Commands;
					break;
				}
				default:
				{
					throw new UnhandledSwitchCaseException
						($"Invalid command set: {Framework}");
				}
			}
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
			if (_HandleConfiguration.AppSettingsValues.RequireSummaryFile)
			{
				vLookFor = DotNetCSProjKeys.PRODUCT;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.PRODUCT
					, ExtractDotNetSummary()
				);
			}
			if (_HandleConfiguration.AppSettingsValues.ForceVersionOverride)
			{
				vLookFor = DotNetCSProjKeys.VERSION;
				vElement = _XDoc.FindElement(vLookFor);
				ProcessElement
				(
					vParent
					, vElement
					, DotNetCSProjKeys.VERSION
					, _HandleConfiguration.AppSettingsValues.VersionOverride
				);
			}
			else
			{
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
			}
			//if (_HandleConfiguration.DotNetNuSpecSettings.ForceVersion)
			//{
			//	vLookFor = DotNetCSProjKeys.VERSION;
			//	vElement = _XDoc.FindElement(vLookFor);
			//	ProcessElement
			//	(
			//		vParent
			//		, vElement
			//		, DotNetCSProjKeys.VERSION
			//		, _HandleConfiguration.DotNetNuSpecSettings.Version
			//	);
			//}
			if (vChanged)
			{
				_XDoc.Save(vPath);
			}
		}

	}
}
