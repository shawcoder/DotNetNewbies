using System.Reflection;
using System.Runtime.InteropServices;
using static AssemblyInfoConsts;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SpyStore.DAL.Contracts")]
[assembly: AssemblyDescription("You didn't update the AssemblyDescription!")]
[assembly: AssemblyCompany(COMPANY)]
[assembly: AssemblyProduct("You didn't update the AssemblyProduct!")]
[assembly: AssemblyCopyright("Copyright © 2017 " + COMPANY)]
[assembly: AssemblyTrademark("Bang the Button - It Just Works!")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fdd01a9c-1e56-430f-8120-fa160776094f")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and
// Revision Numbers by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyVersion(VERSION + BUILD_V)]
[assembly: AssemblyFileVersion(VERSION + BUILD_FV)]
[assembly: AssemblyInformationalVersion(VERSION + BUILD_V + BUILD_I)] // NuGet $version$
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyVersion(VERSION + BUILD_V)]
[assembly: AssemblyFileVersion(VERSION + BUILD_FV)]
[assembly: AssemblyInformationalVersion(VERSION + BUILD_V)] // NuGet $version$
#endif
// ReSharper disable once CheckNamespace
internal struct AssemblyInfoConsts
{
	internal const string VERSION = "1.0.0.";
	internal const string BUILD_FV = "1"; // This value needs to be AT LEAST 1, otherwise NuGet has "trouble" with it (it ignores it entirely).
	internal const string BUILD_V = BUILD_FV;
	internal const string BUILD_I = "-Debug"; // Set this to an empty string if a debug version is needed in NuGet
	internal const string COMPANY = "Az Web Engineering";
}
