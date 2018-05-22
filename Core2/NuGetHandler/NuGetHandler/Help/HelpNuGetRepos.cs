namespace NuGetHandler.Help
{
	using System;
	using System.Collections.Generic;
	using AppConfigHandling;
	using static Help;

	public static class HelpNuGetRepos
	{
		private static void OutputRepoInfo
			(NuGetRepository aRepository, int aIndent = 0)
		{
			string vIndent =
				(aIndent > 0)
					? "\n" + new String('\t', aIndent)
					: "\n";
			string vHasSource =
				aRepository.HasSource
					? "Yep, it has a source."
					: "Nope, no source here.";
			string vIsNuGetServer =
				aRepository.IsNuGetServer
					? "Yep, it's a NuGetServer"
					: "Nope, it's a share";
			string vSource = $"Source: {aRepository.Source}";
			string vSourceApiKey =
				aRepository.IsNuGetServer
					? $", Source API Key: {aRepository.SourceApiKey}.{vIndent}"
					: $", Source is not a NuGet Server, just a share.{vIndent}";
			bool vTest =
				aRepository.HasSymbolSource
				&& !String.IsNullOrWhiteSpace(aRepository.SymbolSource);
			string vHasSymbolsSource =
				aRepository.HasSymbolSource
					? $"Yep, has a symbol source.{vIndent}"
					: $"Nope, no symbols source here.{vIndent}";
			string vSymbolSource =
				vTest
					? $"Symbol Source: {aRepository.SymbolSource}, "
					: "No Symbol Source. ";
			vTest =
				aRepository.HasSymbolSource
				&& !String.IsNullOrWhiteSpace(aRepository.SymbolSourceApiKey);
			string vSymbolSourceApiKey =
				vTest
					? $"Symbols Source API Key: {aRepository.SymbolSourceApiKey}."
					: "No Symbols Source API Key.";
			Add($"{vIndent}{aRepository.RepositoryName}{vIndent}{vHasSource}{vIndent}{vIsNuGetServer}{vIndent}{vSource}{vSourceApiKey}{vHasSymbolsSource}{vSymbolSource}{vSymbolSourceApiKey}");
			Add();
		}

		public static void OutputNuGetRepositoryInfo()
		{
			Add("***** Processed NuGet Repository Information");
			foreach (NuGetRepository vItem in HandleConfiguration.Repositories)
			{
				OutputRepoInfo(vItem);
			}
		}

		public static void OutputPushDestinationInfo()
		{
			Add("***** Processed NuGet Push Destination Information\n");
			string vLine = new String('*', 80);
			foreach (KeyValuePair<string, List<NuGetRepository>> vItem in HandleConfiguration.Destinations)
			{
				Add($"{vItem.Key}");
				foreach (NuGetRepository vInnerItem in vItem.Value)
				{
					OutputRepoInfo(vInnerItem, 1);
				}
				Add($"{vLine}\n");
			}
			Add();
		}

		public static void OutputNuGetRepositories()
		{
			Add("***** Repositories\n");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.NuGetRepos.Repositories)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Add();
		}

		public static void OutputNuGetPushDestinations()
		{
			Add("***** Push Destinations\n");
			foreach (KeyValuePair<string, string> vPair in HandleConfiguration.NuGetDestinations.PushDestinations)
			{
				Add($"Key: {vPair.Key}, Value: {vPair.Value}");
			}
			Add();
		}

		public static void OutputNuGetRepository()
		{
			Add("NuGetRepos");
			Add();
			Add("- This section is where the various destinations are defined. Each destination");
			Add("  represents a diffrerent NuGet server or share (only in the case of NuGet.exe).");
			Add();
			Add("  The following entries *MUST* come in pairs or quads unless the entry is a");
			Add("  local share. ");
			Add("  ");
			Add("  Each entry translates as follows:");
			Add(@"    key=Arbritrary name given to the NuGet server e.g. ""LocalVM""");
			Add();
			Add("    value=Path to the NuGet Server (or local share)");
			Add("          API Key to the NuGet Server (blank for a local share)");
			Add("          Path to the NuGet Symbols Server");
			Add("          API Key to the NuGet Symbols Server");
			Add("  ");
			Add("  The entries for the symbol server are optional. ");
			Add("  Each entry *MUST* be delimited by a semi-colon (;), even if the API Key is");
			Add("  blank, it's position *MUST* be delimited by a semi-colon in order for the");
			Add("  symbol server values to match up correctly.");
			Add();
			Add(@"<add key=""LocalVM"" value=""http://localhost:2017/api/v2/package;{B598D282-1039-427A-A09B-73CEFC95986E}""/>");
			Add(@"<add key=""Home"" value=""http://10.20.30.40/api/v2/package;{0BD38176-3B76-4BFC-8E11-D6DE2ED0081C}""/>");
			Add(@"<add key=""HomeNew"" value=""http://www.awenugetserver3.com/nuget;{589B0E44-2820-4F77-8AF4-1BB0D21B177D}""/>");
			Add(@"<add key=""localShare"" value=""c:\Packages2017\LocalRepository""/>");
			Add();
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				SectionBreak("NuGet Server Repository Info");
				OutputNuGetRepositoryInfo();
			}
		}

		public static void OutputNuGetDestinations()
		{
			Add("NuGetDestinations (examples)");
			Add();
			Add(@"- These values represent the ""PushDestination"" value as defined elsewhere.");
			Add();
			Add("  Each PushDestination is a unique name that represents one or more NuGet");
			Add("  servers (repos) that a newly-created package will be pushed to.");
			Add();
			Add("  One constructs a PushDestination by giving it a unique name and setting its");
			Add("  value to one or more NuGetRepos entries e.g. if a Repo (NuGet server) has a");
			Add(@"  name of ""LocalVM"" and another has a name of ""Home"", then a default");
			Add(@"  PushDestination might be given a name of ""Default"" and its value set to");
			Add(@"  ""LocalVM;Home"", the result being that a newly created package will be pushed");
			Add("  to both the LocalVM NuGet server and the Home NuGet server.");
			Add("  ");
			Add(@"<add key=""Default"" value=""LocalVM;HomeNew""/>");
			Add(@"<add key=""DefaultTest"" value=""LocalVM;HomeNew;Home;LocalShare""/>");
			Add(@"<add key=""LocalOnly"" value=""LocalVM""/>");
			Add(@"<add key=""HomeHome"" value=""Home""/>");
			Add(@"<add key=""HomeNewHome"" value=""HomeNew""/>");
			Add(@"<add key=""LocalShareLocalShare"" value=""localShare""/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				SectionBreak("NuGet Server Destination Info");
				OutputPushDestinationInfo();
			}
		}

		public static void OutputNuGetPush()
		{
			OutputNuGetRepository();
			SectionBreak("NuGet Destinations");
			OutputNuGetDestinations();
		}

	}
}
