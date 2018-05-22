namespace NuGetHandler.Help
{
	using AppConfigHandling;
	using static Help;

	public static class HelpNuSpecNuGet
	{
		public static void OutputNuSpecNuGetValues()
		{
			Add("***** NuSpec values\n");
			Add($"{nameof(NuGetNuSpecValues.ForceAuthors)} = {HandleConfiguration.NuGetNuSpecSettings.ForceAuthors}");
			Add($"{nameof(NuGetNuSpecValues.Authors)} = {HandleConfiguration.NuGetNuSpecSettings.Authors}");
			Add($"{nameof(NuGetNuSpecValues.ForceCopyright)} = {HandleConfiguration.NuGetNuSpecSettings.ForceCopyright}");
			Add($"{nameof(NuGetNuSpecValues.Copyright)} = {HandleConfiguration.NuGetNuSpecSettings.Copyright}");
			Add($"{nameof(NuGetNuSpecValues.ForceDescription)} = {HandleConfiguration.NuGetNuSpecSettings.ForceDescription}");
			Add($"{nameof(NuGetNuSpecValues.Description)} = {HandleConfiguration.NuGetNuSpecSettings.Description}");
			Add($"{nameof(NuGetNuSpecValues.ForceIconUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceIconUrl}");
			Add($"{nameof(NuGetNuSpecValues.IconUrl)} = {HandleConfiguration.NuGetNuSpecSettings.IconUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceLicenseUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceLicenseUrl}");
			Add($"{nameof(NuGetNuSpecValues.LicenseUrl)} = {HandleConfiguration.NuGetNuSpecSettings.LicenseUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceOwners)} = {HandleConfiguration.NuGetNuSpecSettings.ForceOwners}");
			Add($"{nameof(NuGetNuSpecValues.Owners)} = {HandleConfiguration.NuGetNuSpecSettings.Owners}");
			Add($"{nameof(NuGetNuSpecValues.ForceProjectUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ForceProjectUrl}");
			Add($"{nameof(NuGetNuSpecValues.ProjectUrl)} = {HandleConfiguration.NuGetNuSpecSettings.ProjectUrl}");
			Add($"{nameof(NuGetNuSpecValues.ForceReleaseNotes)} = {HandleConfiguration.NuGetNuSpecSettings.ForceReleaseNotes}");
			Add($"{nameof(NuGetNuSpecValues.ReleaseNotes)} = {HandleConfiguration.NuGetNuSpecSettings.ReleaseNotes}");
			Add($"{nameof(NuGetNuSpecValues.ForceRequireLicenseAcceptance)} = {HandleConfiguration.NuGetNuSpecSettings.ForceRequireLicenseAcceptance}");
			Add($"{nameof(NuGetNuSpecValues.RequireLicenseAcceptance)} = {HandleConfiguration.NuGetNuSpecSettings.RequireLicenseAcceptance}");
			Add($"{nameof(NuGetNuSpecValues.ForceSummary)} = {HandleConfiguration.NuGetNuSpecSettings.ForceSummary}");
			Add($"{nameof(NuGetNuSpecValues.Summary)} = {HandleConfiguration.NuGetNuSpecSettings.Summary}");
			Add($"{nameof(NuGetNuSpecValues.ForceTags)} = {HandleConfiguration.NuGetNuSpecSettings.ForceTags}");
			Add($"{nameof(NuGetNuSpecValues.Tags)} = {HandleConfiguration.NuGetNuSpecSettings.Tags}");
			Add($"{nameof(NuGetNuSpecValues.ForceTitle)} = {HandleConfiguration.NuGetNuSpecSettings.ForceTitle}");
			Add($"{nameof(NuGetNuSpecValues.Title)} = {HandleConfiguration.NuGetNuSpecSettings.Title}");
			Add($"{nameof(NuGetNuSpecValues.ForceVersion)} = {HandleConfiguration.NuGetNuSpecSettings.ForceVersion}");
			Add($"{nameof(NuGetNuSpecValues.Version)} = {HandleConfiguration.NuGetNuSpecSettings.Version}");
			Add();
		}

		public static void OutputNuSpecNuGet()
		{
			Add(@"NuGet NuSpec settings");
			Add();
			Add(@"- If one examines the content of the generated .nuspec file by executing");
			Add(@"  NuGet spec in the same directory as the targeted project, one will discover");
			Add(@"  that a .nuspec file has been created. This file will contain replacable tokens");
			Add(@"  whose assigned values may be assigned here. This program uses this process");
			Add(@"  to create a .nuspec file and then modify it.");
			Add();
			Add(@"  Note that ""Id"" and ""Version"" are handled elsewhere. The ""Id"" value is left ");
			Add(@"  as-is, ""Version"" is handled by the ""ForceVersionOverride"" and ");
			Add(@"  ""VersionOverride"" values as found in the app settings section.");
			Add(@"  In addition, the ""Version"" may be overridden on the command line");
			Add();
			Add(@"  The following represents a typical .nuspec file just after creation:");
			Add();
			Add(@"<?xml version=""1.0""?>");
			Add(@"<package >");
			Add(@"  <metadata>");
			Add(@"    <id>$id$</id>");
			Add(@"    <version>$version$</version>");
			Add(@"    <title>$title$</title>");
			Add(@"    <authors>$author$</authors>");
			Add(@"    <owners>$author$</owners>");
			Add(@"    <licenseUrl>http://LICENSE_URL_HERE_OR_DELETE_THIS_LINE</licenseUrl>");
			Add(@"    <projectUrl>http://PROJECT_URL_HERE_OR_DELETE_THIS_LINE</projectUrl>");
			Add(@"    <iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>");
			Add(@"    <requireLicenseAcceptance>false</requireLicenseAcceptance>");
			Add(@"    <description>$description$</description>");
			Add(@"    <releaseNotes>Summary of changes made in this release of the package.</releaseNotes>");
			Add(@"    <copyright>Copyright 2018</copyright>");
			Add(@"    <tags>Tag1 Tag2</tags>");
			Add(@"  </metadata>");
			Add(@"</package>");
			Add();
			Add(@"- To assign a value to the .nuspec file, one need only set the appropriate");
			Add(@"  ForceXXX value to ""true"" and give an appropriate value to the associatedkey.");
			Add(@"  If the ForceXXX value is set to ""false"", the associated value will NOT be");
			Add(@"  used and the value, if any, that can be derived from the AssemblyInfo.cs");
			Add(@"  file will be inserted in its place when the package is created. This can");
			Add(@"  cause problems if required items such as ""Description"" are left blank.");
			Add();
			Add(@"<add key=""ForceAuthors"" value=""false""/>");
			Add(@"<add key=""Authors"" value=""Authored by Az Web Engineering""/>");
			Add(@"<add key=""ForceCopyright"" value=""false""/>");
			Add(@"<add key=""Copyright"" value=""Copyright © 2015 Az Web Engineering""/>");
			Add(@"<add key=""ForceDescription"" value=""false""/>");
			Add(@"<add key=""Description"" value=""App.config Description.""/>");
			Add(@"<add key=""ForceIconUrl"" value=""false""/>");
			Add(@"<add key=""IconUrl"" value=""""/>");
			Add(@"<add key=""ForceLicenseUrl"" value=""false""/>");
			Add(@"<add key=""LicenseUrl"" value=""""/>");
			Add(@"<add key=""ForceOwners"" value=""false""/>");
			Add(@"<add key=""Owners"" value=""Owned By Az Web Engineering""/>");
			Add(@"<add key=""ForceProjectUrl"" value=""false""/>");
			Add(@"<add key=""ProjectUrl"" value=""""/>");
			Add(@"<add key=""ForceReleaseNotes"" value=""false""/>");
			Add(@"<add key=""ReleaseNotes"" value=""App.config Release Notes.""/>");
			Add(@"<add key=""ForceRequireLicenseAcceptance"" value=""false""/>");
			Add(@"<add key=""RequireLicenseAcceptance"" value=""false""/>");
			Add(@"<add key=""ForceSummary"" value=""false""/>");
			Add(@"<add key=""Summary"" value=""App.config Summary""/>");
			Add(@"<add key=""ForceTags"" value=""false""/>");
			Add(@"<add key=""Tags"" value=""AWE""/>");
			Add(@"<add key=""ForceTitle"" value=""false""/>");
			Add(@"<add key=""Title"" value=""""/>");
			Add(@"<add key=""ForceVersion"" value=""false""/>");
			Add(@"<add key=""Version"" value=""""/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				Add();
				SectionBreak("NuSpec values for NuGet");
				OutputNuSpecNuGetValues();
			}
		}

	}
}
