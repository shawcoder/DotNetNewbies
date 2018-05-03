namespace NuGetHandler.Infrastructure
{
	using AppConfigHandling;

	public static class NuSpecHelper
	{
		private static void FromTo(NuGetNuSpecValues aFrom, DotNetNuSpecValues aTo)
		{
			aTo.ForceAuthors = aFrom.ForceAuthors;
			aTo.Authors = aFrom.Authors;
			aTo.ForceCompany = aFrom.ForceOwners;
			aTo.Company = aFrom.Owners;
			aTo.ForceCopyright = aFrom.ForceCopyright;
			aTo.Copyright = aFrom.Copyright;
			aTo.ForceDescription = aFrom.ForceDescription;
			aTo.Description = aFrom.Description;
			aTo.ForcePackageIconUrl = aFrom.ForceIconUrl;
			aTo.PackageIconUrl = aFrom.IconUrl;
			aTo.ForcePackageLicenseUrl = aFrom.ForceLicenseUrl;
			aTo.PackageLicenseUrl = aFrom.LicenseUrl;
			aTo.ForcePackageProjectUrl = aFrom.ForceProjectUrl;
			aTo.PackageProjectUrl = aFrom.ProjectUrl;
			aTo.ForcePackageReleaseNotes = aFrom.ForceReleaseNotes;
			aTo.PackageReleaseNotes = aFrom.ReleaseNotes;
			aTo.ForcePackageTags = aFrom.ForceTags;
			aTo.PackageTags = aFrom.Tags;
			aTo.ForcePackageRequireLicenseAcceptance = aFrom.ForceRequireLicenseAcceptance;
			aTo.PackageRequireLicenseAcceptance = aFrom.RequireLicenseAcceptance;
		}

		private static void FromTo(DotNetNuSpecValues aFrom, NuGetNuSpecValues aTo)
		{
			aTo.ForceAuthors = aFrom.ForceAuthors;
			aTo.Authors = aFrom.Authors;
			aTo.ForceOwners = aFrom.ForceCompany;
			aTo.Owners = aFrom.Company;
			aTo.ForceCopyright = aFrom.ForceCopyright;
			aTo.Copyright = aFrom.Copyright;
			aTo.ForceDescription = aFrom.ForceDescription;
			aTo.Description = aFrom.Description;
			aTo.ForceIconUrl = aFrom.ForcePackageIconUrl;
			aTo.IconUrl = aFrom.PackageIconUrl;
			aTo.ForceLicenseUrl = aFrom.ForcePackageLicenseUrl;
			aTo.LicenseUrl = aFrom.PackageLicenseUrl;
			aTo.ForceProjectUrl = aFrom.ForcePackageProjectUrl;
			aTo.ProjectUrl = aFrom.PackageProjectUrl;
			aTo.ForceReleaseNotes = aFrom.ForcePackageReleaseNotes;
			aTo.ReleaseNotes = aFrom.PackageReleaseNotes;
			aTo.ForceTags = aFrom.ForcePackageTags;
			aTo.Tags = aFrom.PackageTags;
			aTo.ForceRequireLicenseAcceptance = aFrom.ForcePackageRequireLicenseAcceptance;
			aTo.RequireLicenseAcceptance = aFrom.PackageRequireLicenseAcceptance;
		}

		public static void AssignFrom
			(this NuGetNuSpecValues aTo, DotNetNuSpecValues aFrom)
		{
			FromTo(aFrom, aTo);
		}

		public static void AssignTo
			(this NuGetNuSpecValues aFrom, DotNetNuSpecValues aTo)
		{
			FromTo(aFrom, aTo);
		}

		public static void AssignFrom
			(this DotNetNuSpecValues aTo, NuGetNuSpecValues aFrom)
		{
			FromTo(aFrom, aTo);
		}

		public static void AssignTo
			(this DotNetNuSpecValues aFrom, NuGetNuSpecValues aTo)
		{
			FromTo(aFrom, aTo);
		}

	}
}
