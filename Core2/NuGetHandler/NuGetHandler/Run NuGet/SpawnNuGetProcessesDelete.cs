namespace NuGetHandler.Run_NuGet
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using static Consts;
	using static TokenSetContainer;

	public partial class SpawnNuGetProcesses
	{
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

	}
}
