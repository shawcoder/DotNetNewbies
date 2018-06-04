namespace NuGetHandler.Run_NuGet
{
	using System;
	using System.Collections.Generic;
	using AppConfigHandling;
	using Infrastructure;
	using static ApplyTokenValuesToCommandLine;
	using static ProjectFileProcessing.FrameworkInformation;

	public partial class SpawnNuGetProcesses
	{
		private void PrepareForCoreProcess(NuGetRepository aNuGetRepository)
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
				case DotNetFramework.Core_2_0:
				{
					vCommands = _HandleConfiguration.Core_2_0_Commands.Commands;
					break;
				}
				case DotNetFramework.Core_2_1:
				{
					vCommands = _HandleConfiguration.Core_2_1_Commands.Commands;
					break;
				}
				default:
				{
					throw new UnhandledSwitchCaseException
						($"Unhandled switch case: {Framework}");
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

		private void ProcessTheCoreProjectFileForPack()
		{
			// As of this writing, the two methods of handling the different core
			// and standard projects are identical. This method is provided only as a
			// place to start should the two ever diverge.
			ProcessTheStandardProjectFileForPack();
		}

	}
}
