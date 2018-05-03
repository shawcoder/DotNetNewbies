namespace NuGetHandler.AppConfigHandling
{
	/// <summary>
	/// Even though there is no such thing as a "DotNetNuSpec" file, this is the
	/// equivalent of the nuget-generated .nuspec file. The values are actually
	/// found in a sub-section of the originating .csproj file. 
	///	IT IS ASSUMED (YES, "ASSUMED" WITH ALL THAT MEANS) THAT THE CONTAINING
	/// SECTION ALREADY EXISTS IN THE .CSPROJ FILE AND THAT THIS PROGRAM WON'T
	/// HAVE A PROBLEM FINDING AND MODIFYING IT.
	/// </summary>
	public class DotNetNuSpecValues
	{
		public bool UseNuGetNuSpecValues { get; set; }
		public bool ForceAssemblyVersion { get; set; }
		public string AssemblyVersion { get; set; }
		public bool ForceAuthors { get; set; }
		public string Authors { get; set; }
		public bool ForceCompany { get; set; }
		public string Company { get; set; }
		public bool ForceCopyright { get; set; }
		public string Copyright { get; set; }
		public bool ForceDescription { get; set; }
		public string Description { get; set; }
		public bool ForceFileVersion { get; set; }
		public string FileVersion { get; set; }
		public bool ForcePackageIconUrl { get; set; }
		public string PackageIconUrl { get; set; }
		public bool ForcePackageLicenseUrl { get; set; }
		public string PackageLicenseUrl { get; set; }
		public bool ForcePackageProjectUrl { get; set; }
		public string PackageProjectUrl { get; set; }
		public bool ForcePackageReleaseNotes { get; set; }
		public string PackageReleaseNotes { get; set; }
		public bool ForcePackageRequireLicenseAcceptance { get; set; }
		public string PackageRequireLicenseAcceptance { get; set; }
		public bool ForcePackageTags { get; set; }
		public string PackageTags { get; set; }
		public bool ForceProduct { get; set; }
		public string Product { get; set; }
		public bool ForceVersion { get; set; }
		public string Version { get; set; }

	}

	public static class DotNetNuSpecValuesHelper
	{

		private static void FromTo(DotNetNuSpecValues aFrom, DotNetNuSpecValues aTo)
		{
			aTo.UseNuGetNuSpecValues = aFrom.UseNuGetNuSpecValues;
			aTo.ForceAssemblyVersion = aFrom.ForceAssemblyVersion;
			aTo.AssemblyVersion = aFrom.AssemblyVersion;
			aTo.ForceAuthors = aFrom.ForceAuthors;
			aTo.Authors = aFrom.Authors;
			aTo.ForceCompany = aFrom.ForceCompany;
			aTo.Company = aFrom.Company;
			aTo.ForceCopyright = aFrom.ForceCopyright;
			aTo.Copyright = aFrom.Copyright;
			aTo.ForceDescription = aFrom.ForceDescription;
			aTo.Description = aFrom.Description;
			aTo.ForceFileVersion = aFrom.ForceFileVersion;
			aTo.FileVersion = aFrom.FileVersion;
			aTo.ForcePackageIconUrl = aFrom.ForcePackageIconUrl;
			aTo.PackageIconUrl = aFrom.PackageIconUrl;
			aTo.ForcePackageLicenseUrl = aFrom.ForcePackageLicenseUrl;
			aTo.PackageLicenseUrl = aFrom.PackageLicenseUrl;
			aTo.ForcePackageProjectUrl = aFrom.ForcePackageProjectUrl;
			aTo.PackageProjectUrl = aFrom.PackageProjectUrl;
			aTo.ForcePackageReleaseNotes = aFrom.ForcePackageReleaseNotes;
			aTo.PackageReleaseNotes = aFrom.PackageReleaseNotes;
			aTo.ForcePackageRequireLicenseAcceptance =
				aFrom.ForcePackageRequireLicenseAcceptance;
			aTo.PackageRequireLicenseAcceptance =
				aFrom.PackageRequireLicenseAcceptance;
			aTo.ForcePackageTags = aFrom.ForcePackageTags;
			aTo.PackageTags = aFrom.PackageTags;
			aTo.ForceProduct = aFrom.ForceProduct;
			aTo.Product = aFrom.Product;
			aTo.ForceVersion = aFrom.ForceVersion;
			aTo.Version = aFrom.Version;
		}

		public static void AssignFrom
			(this DotNetNuSpecValues aDotNetNuSpecValues, DotNetNuSpecValues aFrom)
		{
			FromTo(aFrom, aDotNetNuSpecValues);
		}

		public static void AssignTo
			(this DotNetNuSpecValues aDotNetNuSpecValues, DotNetNuSpecValues aTo)
		{
			FromTo(aTo, aDotNetNuSpecValues);
		}

	}
}
