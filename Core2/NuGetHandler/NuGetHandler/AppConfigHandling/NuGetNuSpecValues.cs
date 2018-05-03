namespace NuGetHandler.AppConfigHandling
{
	public class NuGetNuSpecValues
	{
		/* From a generated .nuspec file generated with NuGet.exe version 4.5.1.4879

			<id>$id$</id>
			<version>$version$</version>
			<title>$title$</title>
			<authors>$author$</authors>
			<owners>$author$</owners>
			<licenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</licenseUrl>
			<projectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</projectUrl>
			<iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>
			<requireLicenseAcceptance>false</requireLicenseAcceptance>
			<description>$description$</description>
			<releaseNotes>Summary of changes made in this release of the package.</releaseNotes>
			<copyright>Copyright 2018</copyright>
			<tags>Tag1 Tag2</tags>

		 */

		public bool ForceAuthors { get; set; }
		public string Authors { get; set; }
		public bool ForceCopyright { get; set; }
		public string Copyright { get; set; }
		public bool ForceDescription { get; set; }
		public string Description { get; set; }
		public bool ForceIconUrl { get; set; }
		public string IconUrl { get; set; }
		public bool ForceLicenseUrl { get; set; }
		public string LicenseUrl { get; set; }
		public bool ForceOwners { get; set; }
		public string Owners { get; set; }
		public bool ForceProjectUrl { get; set; }
		public string ProjectUrl { get; set; }
		public bool ForceReleaseNotes { get; set; }
		public string ReleaseNotes { get; set; }
		public bool ForceRequireLicenseAcceptance { get; set; }
		public string RequireLicenseAcceptance { get; set; }
		public bool ForceSummary { get; set; }
		public string Summary { get; set; }
		public bool ForceTags { get; set; }
		public string Tags { get; set; }
		public bool ForceTitle { get; set; }
		public string Title { get; set; }
		//"Id" and "Version" are handled elsewhere. The "Id" value is left as-is,
		//"Version" is handled by the "ForceVersionOverride" and "VersionOverride"
		//values as found in the app settings section.

		//public string Id { get; set; }
		//public string Version { get; set; }
	}

	public static class NuSpecValuesHelper
	{
		private static void FromTo(NuGetNuSpecValues aFrom, NuGetNuSpecValues aTo)
		{
			aTo.ForceAuthors = aFrom.ForceAuthors;
			aTo.Authors = aFrom.Authors;
			aTo.ForceCopyright = aFrom.ForceCopyright;
			aTo.Copyright = aFrom.Copyright;
			aTo.ForceDescription = aFrom.ForceDescription;
			aTo.Description = aFrom.Description;
			aTo.ForceIconUrl = aFrom.ForceIconUrl;
			aTo.IconUrl = aFrom.IconUrl;
			aTo.ForceLicenseUrl = aFrom.ForceLicenseUrl;
			aTo.LicenseUrl = aFrom.LicenseUrl;
			aTo.ForceOwners = aFrom.ForceOwners;
			aTo.Owners = aFrom.Owners;
			aTo.ForceProjectUrl = aFrom.ForceProjectUrl;
			aTo.ProjectUrl = aFrom.ProjectUrl;
			aTo.ForceReleaseNotes = aFrom.ForceReleaseNotes;
			aTo.ReleaseNotes = aFrom.ReleaseNotes;
			aTo.ForceRequireLicenseAcceptance = aFrom.ForceRequireLicenseAcceptance;
			aTo.RequireLicenseAcceptance = aFrom.RequireLicenseAcceptance;
			aTo.ForceSummary = aFrom.ForceSummary;
			aTo.Summary = aFrom.Summary;
			aTo.ForceTags = aFrom.ForceTags;
			aTo.Tags = aFrom.Tags;
			aTo.ForceTitle = aFrom.ForceTitle;
			aTo.Title = aFrom.Title;
			//aTo.Id = aFrom.Id;
			//aTo.Version = aFrom.Version;
		}

		public static void AssignFrom
			(this NuGetNuSpecValues aNuGetNuSpecValues, NuGetNuSpecValues aFrom)
		{
			FromTo(aFrom, aNuGetNuSpecValues);
		}

		public static void AssignTo
			(this NuGetNuSpecValues aNuGetNuSpecValues, NuGetNuSpecValues aTo)
		{
			FromTo(aTo, aNuGetNuSpecValues);
		}

	}
}
