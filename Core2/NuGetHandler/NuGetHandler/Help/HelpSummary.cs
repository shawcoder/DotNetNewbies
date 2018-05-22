namespace NuGetHandler.Help
{
	using static Help;

	public static class HelpSummary
	{
		public static void OutputSummary()
		{
			Add("Summary:");
			Add();
			Add("NuGetHandler is a program designed to make the developers job easier when ");
			Add("dealing with NuGet. The idea is to provide a mechanism that can be both");
			Add("standardized and automated with regards to publishing NuGet packages.");
			Add();
			Add("Packages, as created for Visual Studio, are handled by two different commands:");
			Add();
			Add("	- NugGet.exe for Full Framework packages");
			Add("	- DotNet nuget for Standard/Core packages");
			Add();
			Add("Within Visual Studio there are some provisions for handling package");
			Add("publication when developing with Standard and Core, there is no equivalent");
			Add("for Full Framework and even those that are for Standard/Core are not well");
			Add("documented.");
			Add();
			Add("This program solves that issue by supplying a standard methodology for");
			Add("publishing NuGet packages regardless of the environment that is targeted by the");
			Add("package itself e.g. Full Framework, by simply calling on the mechanism provided");
			Add("by Microsoft to perform the actual work. This program simply facilitates the");
			Add("activity by leveraging the resources already provided by Visual Studio and the");
			Add("availability of the Post-Build event handler for each project.");
			Add();
			Add("In other words, for Full Framework, this program simply calls the nuget.exe");
			Add("program and for Standard/Core, this program calls the dotnet.exe program.");
			Add();
			Add("NuGetHandler can cause the generation and publication of a package and cause");
			Add("that package to be published to multiple targets with a single call.");
		}

	}
}
