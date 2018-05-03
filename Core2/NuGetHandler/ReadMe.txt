Things to do:
- Check for the existence of the appropriate nuget gadget after determining
the necessary framework e.g. for Core, look for dotnet.exe, for full framework
look for nuget.exe

- Build gadgets that will substitute the replaceable tokens with their correct
values.

- Add functionality such that when a package is successfully pushed, the package 
name and version are written to a "Delete Later" (*.del) file that can be fed 
back to NuGetHandler in order to delete the selected files from the package 
store.
The user should be able to specify a flag e.g. --D=<Some Delete List>.del to
invoke the deletion process.

//------------------------------------------------------------------------------

It should be noted that the various command declarations DO NOT INCLUDE THE
CALLED PROGRAM e.g. "dotnet" or "nuget". The value of the entry consists
ENTIRELY OF THE TOKENIZED PARAMETERS AS THEY ARE TO BE PASSED TO THE DESIGNATED 
COMMAND!
See below for definitions of the various parameters and the tokenized keyword
that can be used to build the commands in the app.config file.

//------------------------------------------------------------------------------

The following commands are based on NuGet.exe version 4.5.1.4079

Replaceable tokens for the nuget spec command follow:
Run this command in the same directory as the .csproj file and nuget will automatically also create a tokenized .nuspec file

usage:	NuGet spec [package id]

-AssemblyPath					$AssemblyPath$					Path to the assmebly for which a .nuspec file will be created
-Force																				Overwrite existing .nuspec file.
-Verbosity						$NuGetVerbosity$				Display the amount of desired details. values are normal, quiet and detailed.
-NonInteractive																Do not prompt for user input or confirmations.
-ForceEnglishOutput														Forces the application to run using an invariant, English-based culture.

Replaceable tokens for the nuget pack command follow:
usage:	NuGet pack <.nuspec file | .csproj file> [options]

-OutputDirectory			$OutputTo$							Specifies the directory for the created NuGet package file. If not specified, uses the current directory.
-BasePath							$BasePath$							The base path of the files defined in the nuspec file.
-Version							$Version$								Overrides the version number from the nuspec file.
-Suffix								$NuGetVersionSuffix$		Appends a pre-release suffix to the internally generated version number.
-Exclude +						$Exclude$								Specifies one or more wildcard patterns to exclude when creating a package. 
-Symbols																			Determines if a package containing sources and symbols should be created. When specified with a nuspec, creates a regular NuGet package file and the corresponding symbols package.
-Tool																					Determines if the output files of the project should be in the tool folder. 
-Build																				Determines if the project should be built before building the package.
-NoDefaultExcludes														Prevent default exclusion of NuGet package files and files and folders starting with a dot e.g. .svn.
-NoPackageAnalysis														Specify if the command should not run package analysis after building the package.
-ExcludeEmptyDirectories											Prevent inclusion of empty directories when building the package.
-IncludeReferencedProjects										Include referenced projects either as dependencies or as part of the package.
-Properties +					$Properties$						Provides the ability to specify a semicolon ";" delimited list of properties when creating a package.
-MinClientVersion			$MinClientVersion$			Set the minClientVersion attribute for the created package.
-MSBuildVersion				$MinBuildVersion$				Specifies the version of MSBuild to be used with this command. Supported values are 4, 12, 14. By default the MSBuild in your path is picked, otherwise it defaults to the highest installed version of MSBuild.
-MSBuildPath					$MSBuildPath$						Specifies the path of MSBuild to be used with this command. This command will takes precedence over MSbuildVersion, nuget will always pick MSbuild from this specified path.
-Verbosity						$NuGetVerbosity$				Display this amount of details in the output: normal, quiet, detailed.
-NonInteractive																Do not prompt for user input or confirmations.
-ForceEnglishOutput														Forces the application to run using an invariant, English-based culture.

Replaceable tokens for the nuget Push command follow:
usage:	NuGet push <package path> [API Key when not specified as an option] [options]

-Source								$Source$								The url for the NuGet server. If not specified, NuGet.org will be used
-ApiKey								$ApiKey$								The Api key for the NuGet server.
-SymbolSource					$SymbolSource$					Specifies the symbol server URL. If not specified, nuget.smbsrc.net is used when pushing to nuget.org
-SymbolApiKey					$SymbolApiKey$					he API key for the symbol server
-Timeout							$Timeout$								Specifies the timeout for pushin gto a server in seconds Defaults to 300 seconds.
-DisableBuffering															Disable buffering when pushing to a server to decrease memory usage.  *** Dont' use this
-NoSymbols																		If a symbols package exists, it will not be pushed to a symbol server.
-ConfigFile						$ConfigFile$						The NuGet configuration file. If not specifed, %APPDATA%\NuGet\NuGet.config is used.
-Verbosity						$NuGetVerbosity$				Display the amount of desired details. values are normal, quiet and detailed.
-NonInteractive																Do not prompt for user input or confirmations.
-ForceEnglishOutput														Forces the application to run using an invariant, English-based culture.

Replaceable tokens for the nuget Add command follow:
usage:	NuGet add <packagePath> -Source <FolderBasedPackageSource> [options]

-Source								$Source$								Specifis the package source, which is a folder or UNC share, to which the nupkg will be added. Http sources NOT supported.
-Expand																				If provided, a package added to offline feed is also expanded.
-ConfigFile						$ConfigFile$						The NuGet configuration file. If not specifed, %APPDATA%\NuGet\NuGet.config is used.
-Verbosity						$NuGetVerbosity$				Display the amount of desired details. values are normal, quiet and detailed.
-NonInteractive																Do not prompt for user input or confirmations.
-ForceEnglishOutput														Forces the application to run using an invariant, English-based culture.

Replaceable tokens for the nuget delete command follow:
usage:	NuGet delete <package id> <package version> [API Key if not specified as an option] [options]

-Source								$Source$								The URL for the NuGet server. UNC shares and folders are not supported.
-NoPrompt																			Do not promopt when deleting
-ApiKey								$ApiKey$								ApiKey for the server
-ConfigFile						$ConfigFile$						The NuGet configuration file. If not specifed, %APPDATA%\NuGet\NuGet.config is used.
-Verbosity						$NuGetVerbosity$				Display the amount of desired details. values are normal, quiet and detailed.
-NonInteractive																Do not prompt for user input or confirmations.
-ForceEnglishOutput														Forces the application to run using an invariant, English-based culture.

//------------------------------------------------------------------------------

The following commands are based on dotnet versoin 2.1.103

Replaceable tokens for the dotnet nuget pack command follow:
usage:	dotnet pack [<PROJECT>] [-c|--configuration] [--force] [--include-source] [--include-symbols] [--no-build] [--no-dependencies] [--no-restore] [-o|--output] [--runtime] [-s|--serviceable] [-v|--verbosity] [--version-suffix]

Project																				The project to pack. It's either a path to a csproj file or to a directory. If omitted, it defaults to the current directory.

-c|--configuration														Defines the configuration the package was built with e.g. {Debug|Release}
--force 
--include-source															Include the source files in the NuGet package. THe source fiels are included in the src folder with the nupkg.
--include-symbols															Generates the symbols nupkg
--no-build																		Does NOT build the project before packing.
--no-dependencies															Ignores project-to-project references and only restores the root project.
--no-restore																	Doesn't perform an implicit restore when running the command.
-o|--output						$OutputTo$							Places the built packages in the directory specified. 
--runtime							$RuntimeIdentifier$			Specifies the target runtime to restore packages for. For a list of Runtime Identifiers (RIDs), see the RID catalog.
-s|--serviceable															Sets the serviceable flag in the package. For more information, see .NET Blog: .NET 4.5.1 Supports Microsoft Security Updates for .NET NuGet Libraries.
-v|--verbosity				$DotNetVerbosity$				Sets the verbosity level of the command. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
--version-suffix			$DotNetVersionSuffix$		Defines the value for the $(VersionSuffix) MSBuild property in the project.

Replaceable tokens for the dotnet nuget push command follow:
usage:	dotnet nuget push [<ROOT>] [-s|--source] [-ss|--symbol-source] [-t|--timeout] [-k|--api-key] [-sk|--symbol-api-key] [-d|--disable-buffering] [-n|--no-symbols] [--force-english-output] [-h|--help]

Root									$Root$									Specifies the file path to the package to be pushed
		
-s|--source						$Source$								The NuGet server from which the package is to be deleted.
-ss|--symbol-source		$SymbolSource$					The NuGet server from which the package is to be deleted.
-k|--api-key					$ApiKey$								The Api key that grants access to the NuGet server.
-sk|--symbol-api-key	$SymbolApiKey$					The Api key that grans access to the NuGet Symbol server.
-t|--timeout					$Timeout$								Specifies the timeout for pushing to a server in seconds. Defaults to 300 seconds (5 minutes). Specifying 0 (zero seconds) applies the default value.
-d|--disable-buffering												Disables buffering when pushing to an HTTP(S) server to decrease memory usage.
-n|--no-symbols																Don't push symbols even if they are available.
--force-english-output												Forces the application to run using an invariant, English-based culture.

Replaceable tokens for the dotnet nuget delete command follow:
usage:	dotnet nuget delete [<PACKAGE_NAME> <PACKAGE_VERSION>] [-s|--source] [--non-interactive] [-k|--api-key] [--force-english-output] [-h|--help]

Package Name					$PackageName$						The name of the package to delete from the NuGet server.
Package Version				$PackageVersion$				The full version of the package to delete from the NuGet server.

-s|--source						$Source$								The NuGet server from which the package is to be deleted.
-k|--api-key					$ApiKey$								The Api key that grants access to the NuGet server.
--non-interactive															Don't prompt for user input or confirmations.
--force-english-output												Forces the application to run using an invariant, English-based culture.