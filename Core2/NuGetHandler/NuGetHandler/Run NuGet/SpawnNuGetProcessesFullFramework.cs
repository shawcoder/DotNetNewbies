namespace NuGetHandler.Run_NuGet
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using AppConfigHandling;
	using Infrastructure;
	using static AppConfigHandling.CommandLineSettings;
	using static ApplyTokenValuesToCommandLine;
	using static NuGetNuSpecKeys;
	using static TokenSetContainer;

	// Full Framework- specific code
	public partial class SpawnNuGetProcesses
	{
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
			int vChangeCount = 0;
			string vLookFor;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceAuthors)
			{
				vLookFor =
					AUTHOR_KEY + AUTHORS.AsToken();
				vContent.Replace
				(
					vLookFor
					, AUTHOR_KEY
							+ _HandleConfiguration.NuGetNuSpecSettings.Authors
				);
				vChangeCount++;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceCopyright)
			{
				vLookFor = $"{COPYRIGHT} {DateTime.Now.Year}";
				vContent.Replace
				(
					vLookFor.AsToken()
					, _HandleConfiguration.NuGetNuSpecSettings.Copyright
				);
				vChangeCount++;
			}
			vLookFor = DESCRIPTION.AsToken();
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceDescription)
			{
				vContent.Replace
				(
					vLookFor.AsToken()
					, _HandleConfiguration.NuGetNuSpecSettings.Description
				);
				vChangeCount++;
			}
			else
			{
				string vFind =
					DESCRIPTION_KEY_START
					+ DEFAULT_DESCRIPTION
					+ DESCRIPTION_KEY_END
					+ "\r\n";
				string vReplaceWith =
					DESCRIPTION_KEY_START
					+ DEFAULT_DESCRIPTION
					+ DateTime.Now.ToLongDateString()
					+ DESCRIPTION_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, vReplaceWith);
				vChangeCount++;
			}
			vLookFor =
				ICON_URL_KEY + ICON_URL_OR_DELETE_THIS;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceIconUrl)
			{
				vContent.Replace
					(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.IconUrl);
				vChangeCount++;
			}
			else
			{
				string vFind =
					ICON_URL_KEY
						+ ICON_URL_OR_DELETE_THIS
						+ ICON_URL_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, String.Empty);
				vChangeCount++;
			}
			vLookFor = LICENSE_URL;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceLicenseUrl)
			{
				vContent.Replace
				(
					vLookFor.AsToken(),
					_HandleConfiguration.NuGetNuSpecSettings.LicenseUrl
				);
				vChangeCount++;
			}
			else
			{
				string vFind =
					LICENSE_URL_KEY
					+ LICENSE_URL_OR_DELETE_THIS
					+ LICENSE_URL_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, String.Empty);
				vChangeCount++;
			}
			vLookFor = PROJECT_URL;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceProjectUrl)
			{
				vContent.Replace
				(
					vLookFor.AsToken(),
					_HandleConfiguration.NuGetNuSpecSettings.ProjectUrl
				);
				vChangeCount++;
			}
			else
			{
				string vFind =
					PROJECT_URL_KEY
					+ PROJECT_URL_OR_DELETE_THIS
					+ PROJECT_URL_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, String.Empty);
				vChangeCount++;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceOwners)
			{
				vLookFor =
					OWNER_KEY + AUTHORS.AsToken();
				vContent.Replace
				(
					vLookFor
					, OWNER_KEY
							+ _HandleConfiguration.NuGetNuSpecSettings.Owners
				);
				vChangeCount++;
			}
			vLookFor = DEFAULT_RELEASE_NOTES;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceReleaseNotes)
			{
				if (_HandleConfiguration.AppSettingsValues.RequireReleaseNotesFile)
				{
					vContent.Replace(vLookFor, ExtractReleaseNotes());
					vChangeCount++;
				}
			}
			else
			{
				string vFind =
					RELEASE_NOTES_KEY
					+ DEFAULT_RELEASE_NOTES
					+ RELEASE_NOTES_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, String.Empty);
				vChangeCount++;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceRequireLicenseAcceptance)
			{
				vLookFor =
					REQUIRE_LICENSE_ACCEPTANCE_KEY
						+ DEFAULT_REQUIRE_LICENSE_ACCEPTANCE;
				vContent.Replace
				(
					vLookFor,
					_HandleConfiguration.NuGetNuSpecSettings.RequireLicenseAcceptance
				);
				vChangeCount++;
			}
			if (_HandleConfiguration.AppSettingsValues.RequireSummaryFile)
			{
				vLookFor = SUMMARY_KEY;
				string vContentAsString = vContent.ToString();
				int vIndex = vContentAsString.IndexOf(vLookFor, COMPARISON);
				if (vIndex < 0)
				{
					vIndex = vContentAsString.IndexOf
						(METADATA_END, COMPARISON);
					string vNewKey =
						SUMMARY_KEY
							+ ExtractNuGetSummary()
							+ SUMMARY_KEY_END;
					vContent.Insert(vIndex, vNewKey);
					vChangeCount++;
				}
				else
				{
					// This shouldn't ever have to be executed, but just in case MS 
					// changes how nuget.exe functions...
					int vEndIndex =
						vContentAsString.IndexOf
							(SUMMARY_KEY_END, COMPARISON);
					vIndex = vIndex + SUMMARY_KEY.Length;
					int vHowMuch = vEndIndex - vIndex + 1;
					vContentAsString = vContentAsString.Remove(vIndex, vHowMuch);
					vContentAsString = vContentAsString.Insert(vIndex, ExtractNuGetSummary());
					vContent.Clear();
					vContent.Append(vContentAsString);
					vChangeCount++;
				}
			}
			vLookFor = DEFAULT_TAGS;
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceTags)
			{
				vContent.Replace(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.Tags);
				vChangeCount++;
			}
			else
			{
				string vFind =
					TAGS_KEY_START
					+ DEFAULT_TAGS
					+ TAGS_KEY_END
					+ "\r\n";
				vContent.Replace(vFind, String.Empty);
				vChangeCount++;
			}
			if (_HandleConfiguration.NuGetNuSpecSettings.ForceTitle)
			{
				vLookFor = TITLE.AsToken();
				vContent.Replace
					(vLookFor, _HandleConfiguration.NuGetNuSpecSettings.Title);
				vChangeCount++;
			}
			if (_HandleConfiguration.AppSettingsValues.ForceVersionOverride)
			{
				vLookFor = DotNetCSProjKeys.VERSION;
				vContent.Replace
					(vLookFor, _HandleConfiguration.AppSettingsValues.VersionOverride);
				vChangeCount++;
			}
			if (vChangeCount > 0)
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
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.LICENSE_URL_OR_DELETE_THIS, vComparison);
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
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.LICENSE_URL_OR_DELETE_THIS, vComparison);
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
		//				&& vElement.Value.Equals(NuGetNuSpecKeys.LICENSE_URL_OR_DELETE_THIS, vComparison);
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
		//			_HandleConfiguration.NuGetNuSpecSettings.Summary = ExtractNuGetSummary();
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
		//	if (_HandleConfiguration.AppSettingsValues.ForceVersionOverride)
		//	{
		//		vLookFor = DotNetCSProjKeys.VERSION;
		//		vElement = _XDoc.FindElement(vLookFor);
		//		ProcessElement
		//		(
		//			vParent
		//			, vElement
		//			, DotNetCSProjKeys.VERSION
		//			,	_HandleConfiguration.AppSettingsValues.VersionOverride
		//		);
		//	}
		//	if (vChanged)
		//	{
		//		vDoc.Save(vPath);
		//	}
		//}

	}
}
