namespace NuGetHandler.Help
{
	using AppConfigHandling;
	using static Help;

	public static class HelpNuSpecDotNet
	{
		public static void OutputNuSpecDotNetValues()
		{
			Add($"{nameof(DotNetNuSpecValues.ForceAssemblyVersion)} = {HandleConfiguration.DotNetNuSpecSettings.ForceAssemblyVersion}");
			Add($"{nameof(DotNetNuSpecValues.AssemblyVersion)} = {HandleConfiguration.DotNetNuSpecSettings.AssemblyVersion}");
			Add($"{nameof(DotNetNuSpecValues.ForceAuthors)} = {HandleConfiguration.DotNetNuSpecSettings.ForceAuthors}");
			Add($"{nameof(DotNetNuSpecValues.Authors)} = {HandleConfiguration.DotNetNuSpecSettings.Authors}");
			Add($"{nameof(DotNetNuSpecValues.ForceCompany)} = {HandleConfiguration.DotNetNuSpecSettings.ForceCompany}");
			Add($"{nameof(DotNetNuSpecValues.Company)} = {HandleConfiguration.DotNetNuSpecSettings.Company}");
			Add($"{nameof(DotNetNuSpecValues.ForceCopyright)} = {HandleConfiguration.DotNetNuSpecSettings.ForceCopyright}");
			Add($"{nameof(DotNetNuSpecValues.Copyright)} = {HandleConfiguration.DotNetNuSpecSettings.Copyright}");
			Add($"{nameof(DotNetNuSpecValues.ForceDescription)} = {HandleConfiguration.DotNetNuSpecSettings.ForceDescription}");
			Add($"{nameof(DotNetNuSpecValues.Description)} = {HandleConfiguration.DotNetNuSpecSettings.Description}");
			Add($"{nameof(DotNetNuSpecValues.ForceFileVersion)} = {HandleConfiguration.DotNetNuSpecSettings.ForceFileVersion}");
			Add($"{nameof(DotNetNuSpecValues.FileVersion)} = {HandleConfiguration.DotNetNuSpecSettings.FileVersion}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageIconUrl)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageIconUrl}");
			Add($"{nameof(DotNetNuSpecValues.PackageIconUrl)} = {HandleConfiguration.DotNetNuSpecSettings.PackageIconUrl}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageLicenseUrl)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageLicenseUrl}");
			Add($"{nameof(DotNetNuSpecValues.PackageLicenseUrl)} = {HandleConfiguration.DotNetNuSpecSettings.PackageLicenseUrl}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageProjectUrl)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageProjectUrl}");
			Add($"{nameof(DotNetNuSpecValues.PackageProjectUrl)} = {HandleConfiguration.DotNetNuSpecSettings.PackageProjectUrl}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageReleaseNotes)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageReleaseNotes}");
			Add($"{nameof(DotNetNuSpecValues.PackageReleaseNotes)} = {HandleConfiguration.DotNetNuSpecSettings.PackageReleaseNotes}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageRequireLicenseAcceptance)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageRequireLicenseAcceptance}");
			Add($"{nameof(DotNetNuSpecValues.PackageRequireLicenseAcceptance)} = {HandleConfiguration.DotNetNuSpecSettings.PackageRequireLicenseAcceptance}");
			Add($"{nameof(DotNetNuSpecValues.ForcePackageTags)} = {HandleConfiguration.DotNetNuSpecSettings.ForcePackageTags}");
			Add($"{nameof(DotNetNuSpecValues.PackageTags)} = {HandleConfiguration.DotNetNuSpecSettings.PackageTags}");
			Add($"{nameof(DotNetNuSpecValues.ForceVersion)} = {HandleConfiguration.DotNetNuSpecSettings.ForceVersion}");
			Add($"{nameof(DotNetNuSpecValues.Version)} = {HandleConfiguration.DotNetNuSpecSettings.Version}");
			Add();
		}

		public static void OutputNuSpecDotNet()
		{
			Add("- The dotnet.exe equivalent of a NuGet .nuspec file (if one were possible) is");
			Add("  here. These values actually represent the values found in one section of the ");
			Add("  .csproj file as created for .NET Standard/Core projects. Given that ");
			Add("  Standard/Core projects do not have a .nuspec file that can be created for ");
			Add("  them, it is necessary to modify the .csproj file directly and apply the values ");
			Add("  as necessary.");
			Add();
			Add(@"  *** Looks like ""Company"" equals ""Owners"" above.");
			Add(@"  *** The value of the NuGet ""Summary"" equals the dotnet ""Product"".");
			Add();
			Add("- The following is an extraction from a .NET Standard project .csproj file with");
			Add("  the equivalent values expressed.");
			Add();
			Add("  <AssemblyVersion>5.0.0.1</AssemblyVersion>");
			Add("  <Authors>Authored by Az Web Engineering</Authors>");
			Add("  <Company>Company is Az Web Engineering</Company>");
			Add("  <Copyright>Copyright © Az Web Engineering 2018</Copyright>");
			Add("  <Description>External handler for NuGet and dotnet nuget.</Description>");
			Add("  <FileVersion>2.0.0.0</FileVersion>");
			Add("  <PackageIconUrl>PackageUrl</PackageIconUrl>");
			Add("  <PackageLicenseUrl>LicenseUrl</PackageLicenseUrl>");
			Add("  <PackageProjectUrl>ProjectUrl</PackageProjectUrl>");
			Add("  <PackageReleaseNotes>Relase Notes</PackageReleaseNotes>");
			Add("  <PackageRequireLicenseAcceptance>false</PackageRequireLicenseAcceptance>");
			Add("  <PackageTags>AWE</PackageTags>");
			Add("  <Version>4.0.0.1</Version>");
			Add("  <Product>Some Product Summary</Product>");
			Add();
			Add("DotNet NuSpec settings (if DotNet nuget where to have such a thing as a .nuspec");
			Add("file)");
			Add();
			Add(@"<add key=""UseNuGetNuSpecValues"" value=""false""/>");
			Add();
			Add(@"- If this value is set to ""true"", then the assigned values from the following");
			Add(@"  will be applied to the .csproj file and then those values will be applied");
			Add(@"  when the assembly is ""packed"" in preparation for creating the package.");
			Add(@"  ");
			Add(@"  If the value is set to ""false"", then the values as found in the .csproj file");
			Add(@"  will be used.");
			Add();
			Add(@"<add key=""ForceAssemblyVersion"" value=""false""/>");
			Add(@"<add key=""AssemblyVersion"" value=""/>");
			Add(@"<add key=""ForceAuthors"" value=""true""/>");
			Add(@"<add key=""Authors"" value=""Az Web Engineering senior development team""/>");
			Add(@"<add key=""ForceCompany"" value=""false""/>");
			Add(@"<add key=""Company"" value=""Az Web Engineering""/>");
			Add(@"<add key=""ForceCopyright"" value=""false""/>");
			Add(@"<add key=""Copyright"" value=""/>");
			Add(@"<add key=""ForceDescription"" value=""false""/>");
			Add(@"<add key=""Description"" value=""/>");
			Add(@"<add key=""ForceFileVersion"" value=""false""/>");
			Add(@"<add key=""FileVersion"" value=""/>");
			Add(@"<add key=""ForcePackageIconUrl"" value=""false""/>");
			Add(@"<add key=""PackageIconUrl"" value=""/>");
			Add(@"<add key=""ForcePackageLicenseUrl"" value=""false""/>");
			Add(@"<add key=""PackageLicenseUrl"" value=""/>");
			Add(@"<add key=""ForcePackageProjectUrl"" value=""false""/>");
			Add(@"<add key=""PackageProjectUrl"" value=""/>");
			Add(@"<add key=""ForcePackageReleaseNotes"" value=""false""/>");
			Add(@"<add key=""PackageReleaseNotes"" value=""/>");
			Add(@"<add key=""ForcePackageRequireLicenseAcceptance"" value=""false""/>");
			Add(@"<add key=""PackageRequireLicenseAcceptance"" value=""/>");
			Add(@"<add key=""ForcePackageTags"" value=""false""/>");
			Add(@"<add key=""PackageTags"" value=""/>");
			Add(@"<add key=""ForceVersion"" value=""false""/>");
			Add(@"<add key=""Version"" value=""/>");
			if (CommandLineSettings.VerbosityLevel == VerbosityE.Detailed)
			{
				Add();
				SectionBreak("NuSpec values for DotNet");
				OutputNuSpecDotNetValues();
			}
		}

	}
}
