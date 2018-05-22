namespace NuGetHandler.Help
{
	using static Help;

	public static class HelpSetup
	{
		public static void OutputSetup()
		{
			Add("Setup:");
			Add();
			Add("Setting up the NuGetHandler is simplicity itself if the program is generated");
			Add("from the original source using Visual Studio. The Post-Build event contans");
			Add("instructions to copy the generated program and appropriate ancillary files to ");
			Add("the desired location (the same location is the NuGet.exe program) upon ");
			Add("successful compilation of the source in Release mode.");
			Add();
			Add("However, there are a few items that need to be taken care of prior to compiling");
			Add("this program. They are:");
			Add();
			Add("- Fetch the NuGet.exe program from Microsoft. As of this writing, the executable");
			Add("  can be downloaded from the following link:  ");
			Add();
			Add("    https://www.nuget.org/downloads");
			Add();
			Add("Now, create a directory in the %APPDATA% directory for the NuGet.exe program ");
			Add(@"called ""NuGet"" (no quotes, of course). This is the default location presumed by ");
			Add("the NuGetHandler program for the location of the NuGet.exe program.");
			Add();
			Add("Copy the downloaded version of NuGet.exe to the aforementioned directory, open");
			Add("a command prompt and execute the NuGet.exe program with no parameters. This has");
			Add("the effect of creating the default NuGet.config file which makes NuGet happy.");
			Add();
			Add("The NuGetHandler program does not use the NuGet.config file but the authors of");
			Add("the NuGet.exe program recomend that it exist for NuGet.exe to use.");
			Add();
			Add(@"- In the %APPDATA%\NuGet directory, create an additional directory, ""win10 - x64"".");
			Add("  This is the default installation directory for the NuGetHandler program ");
			Add("  itself.");
			Add("  If allowed to execute, the Post-Build event of the NuGetHandler program, upon");
			Add("  successful compilation in Release mode of the program, will copy the result");
			Add("  to this location. One need not add the path to the Environment as the call to");
			Add("  the NuGetHandler program itself is fully specified in the call sequence for ");
			Add("  each project. The full specification for the command sequence for a ");
			Add("  NuGet-eligible project follows:");
			Add();
			Add("@echo off");
			Add("IF NOT $(ConfigurationName) == Release GOTO NOT_RELEASE");
			//Add(@"IF EXIST ""$(SolutionDir)CopyToT4Support.bat"" CALL ""$(SolutionDir)CopyToT4Support"" ""$(TargetPath)"" ""$(SolutionDir)""");
			Add(@"IF NOT EXIST ""%VSProjectsDir%NuGetHandler.bat"" GOTO DIRECT");
			Add(@"CALL ""%VSProjectsDir%NuGetHandler.bat""  ""$(TargetPath)"" ""$(SolutionPath)"" ""$(ProjectPath)"" $(ConfigurationName)");
			Add("GOTO END");
			Add(":DIRECT");
			Add(@"IF NOT EXIST ""%APPDATA%\NuGet\win10-x64\NuGetHandler.dll"" GOTO END");
			Add(@"dotnet "" % APPDATA %\NuGet\win10 - x64\NuGetHandler.dll"" -T ""$(TargetPath)"" -S ""$(SolutionPath)"" -P ""$(ProjectPath)"" -C $(ConfigurationName) -V quiet");
			Add("GOTO END");
			Add(":NOT_RELEASE");
			Add("GOTO END");
			Add(":END");
			Add();
			Add("Place the above script, verbatim, into the Post-Build event of each project to ");
			Add("be made NuGetHandler eligible and the command will take care of the rest. And");
			Add("yes, the quotes in the above script ARE required.");
			Add();
			Add("As can be seen by the call itself, there is no need to clutter up the ");
			Add("Environment path with extraneous information. The .NET Core framework is");
			Add(@"presumed to have already been installed thus the call to ""dotnet.exe"" will be");
			Add("valid.");
			Add();
			Add("Obviously, other switches can be added as desired, but the above is the minimum");
			Add("necessary to make a project NuGetHandler-eligible.");
			Add();
			Add(@"Finally, create an environment variable named""VSProjectsDir"" and set it to");
			Add("the root project directory of Visual Studio (usually something like");
			Add(@"C:\Users\<userName>\Documents\Visual Studio 2017\Projects\ <-- Note the trailing");
			Add("Path delimiter character.).");
			Add("This allows each project to use either the batch-file launched version of");
			Add("NuGetHandler (easier for mass debugging of this program) or the direct call");
			Add("variant (which just launches the dotnet .dll directly, hence the name).");
		}

	}
}
