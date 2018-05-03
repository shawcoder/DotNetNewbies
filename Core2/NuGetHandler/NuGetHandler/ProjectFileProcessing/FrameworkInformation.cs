namespace NuGetHandler.ProjectFileProcessing
{
	using System.Xml.Linq;

	public static class FrameworkInformation
	{
		public static DotNetFramework Framework { get; set; }
		public static string FrameworkVersion { get; set; }
		public static bool GeneratePackageOnBuild { get; set; }
		public static string ProjectName { get; set; }
		public static string PackageVersion { get; set; }
		public static string AssemblyVersion { get; set; }
		public static string AssemblyFileVersion { get; set; }
		public static XDocument NodeDocument { get; set; }
		public static XElement NodeParent { get; set; }

	}
}
