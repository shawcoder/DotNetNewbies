namespace NuGetHandler.ProjectFileProcessing
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Xml.Linq;
	using Infrastructure;
	using static AppConfigHandling.CommandLineSettings;
	using static FrameworkInformation;

	public static partial class ProcessProjectFile
	{
		private const string _TARGET = "Target";
		private const string _VERSION = "Version";
		private const string _PLATFORM_IDENTIFIER = "PlatformIdentifier";

		private const string _LOOK_FOR_TARGET_FRAMEWORK = _TARGET + "Framework";
		private const string _LOOK_FOR_TARGET_FRAMEWORK_VERSION =
			_LOOK_FOR_TARGET_FRAMEWORK + _VERSION;
		private const string _LOOK_FOR_TARGET_PLATFORM_IDENTIFIER =
			_TARGET + _PLATFORM_IDENTIFIER;

		private const string _LOOK_FOR_GENERATE_PACKAGE_ON_BUILD =
			"GetValueForGeneratePackageOnBuild";

		private const string _NAMESPACE = "t";
		private const string _NET_CORE = "netcoreapp";
		private const string _NET_CORE_2_0 = _NET_CORE + "2.0";
		private const string _NET_CORE_2_1 = _NET_CORE + "2.1";
		private const string _NET_STANDARD = "netstandard";
		private const string _NET_UAP = "UAP";

		private const StringComparison _COMPARISON =
			StringComparison.InvariantCultureIgnoreCase;

		// If one wishes to load the actual assembly to obtain version information,
		// here is how to do it. This method is deprecated for this program at this
		// time.
		private static void GetAssemblyVersionInfoFromAssembly()
		{
			FileVersionInfo vVersionInfo =
				FileVersionInfo.GetVersionInfo(TargetPath);
			FrameworkInformation.ProjectName = vVersionInfo.ProductName;
			AssemblyFileVersion =
				String.IsNullOrWhiteSpace(vVersionInfo.FileVersion)
					? "No Assembly File Version found."
					: vVersionInfo.FileVersion;
			AssemblyVersion =
				AssemblyName
					.GetAssemblyName(TargetPath)
					.Version.ToString();
		}

		private static (DotNetFramework, string) UnhandledFramework(string aFramework)
		{
			(DotNetFramework, string) vResult =
				(DotNetFramework.Unknown, $"Unhandled framework: {aFramework}");
			return vResult;
		}

		private static (DotNetFramework, string) ProcessFrameworkTag
			(string aFramework, string aLookFor, DotNetFramework aDesiredFramework)
		{
			(DotNetFramework, string) vResult =
				!String.IsNullOrWhiteSpace(aFramework)
					? (aFramework.StartsWith(aLookFor, _COMPARISON)
						? (aDesiredFramework, aFramework)
						: (DotNetFramework.Unknown, String.Empty))
					: (DotNetFramework.Unknown, String.Empty);
			return vResult;
		}

		private static (DotNetFramework, string) ExtractTargetFramework()
		{
			string vPath = ProjectPath;
			(DotNetFramework vDotNetFramework, string vFrameworkVersion) vResult =
				CheckForFullFramework(vPath);
			if (vResult.vDotNetFramework == DotNetFramework.Unknown)
			{
				CheckForUniversalWindows(vPath);
			}
			if (vResult.vDotNetFramework == DotNetFramework.Unknown)
			{
				vResult = CheckForStandard(vPath);
				if (vResult.vDotNetFramework == DotNetFramework.Unknown)
				{
					vResult = CheckForCore(vPath);
					if (vResult.vDotNetFramework == DotNetFramework.Unknown)
					{
						vResult = UnhandledFramework(vResult.Item2);
					}
				}
			}
			return vResult;
		}

		private static void GetValueForGeneratePackageOnBuild()
		{
			string vPath = ProjectPath;
			string vGeneratePackageOnBuild =
				vPath.ElementValue(_LOOK_FOR_GENERATE_PACKAGE_ON_BUILD);
			GeneratePackageOnBuild =
				!String.IsNullOrWhiteSpace(vGeneratePackageOnBuild)
					&& vGeneratePackageOnBuild.Equals(bool.TrueString, _COMPARISON);
		}

		public static void ProcessProjectFileContent()
		{
			if (String.IsNullOrWhiteSpace(ProjectPath))
			{
				return;
			}
			//GetAssemblyVersionInfoFromAssembly();
			(DotNetFramework, string) vTest = ExtractTargetFramework();
			Framework = vTest.Item1;
			FrameworkVersion = vTest.Item2;
			if (Framework == DotNetFramework.Unknown)
			{
				throw new Exception
					($"Cannot determine framework for project: \n{ProjectPath}");
			}
			if (Framework != DotNetFramework.Full)
			{
				PackageVersion = ExtractStandardPackageVersion();
				GetValueForGeneratePackageOnBuild();
			}
			else
			{
				ExtractFullFrameworkVersionInfo();
				PackageVersion = AssemblyVersion;
			}
		}

		public static void WritePackageVersion(string aVersion)
		{
			XElement vNode = NodeDocument?.FindElement(_VERSION);
			if (vNode == null)
			{
				return;
			}
			vNode.SetElementValue(_VERSION, aVersion);
			//NodeDocument.Save();
		}

	}
}
