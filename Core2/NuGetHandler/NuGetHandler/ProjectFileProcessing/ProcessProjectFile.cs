namespace NuGetHandler.ProjectFileProcessing
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Linq;
	using Infrastructure;
	using static AppConfigHandling.CommandLineSettings;
	using static FrameworkInformation;

	public static class ProcessProjectFile
	{
		private const string _VERSION = "Version";
		private const string _LOOK_FOR_VERSION = _VERSION;
		private const string _LOOK_FOR_TARGET_FRAMEWORK = "TargetFramework";
		private const string _LOOK_FOR_TARGET_FRAMEWORK_VERSION =
			_LOOK_FOR_TARGET_FRAMEWORK + _VERSION;
		private const string _LOOK_FOR_GENERATE_PACKAGE_ON_BUILD =
			"GeneratePackageOnBuild";

		private const string _NAMESPACE = "t";
		private const string _NET_CORE = "netcoreapp";
		private const string _NET_CORE_2_0 = _NET_CORE + "2.0";
		private const string _NET_CORE_2_1 = _NET_CORE + "2.1";
		private const string _NET_STANDARD = "netstandard";

		private const StringComparison _COMPARISON =
			StringComparison.InvariantCultureIgnoreCase;

		private static void GetProjectVersionInfo()
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

		/// <summary>
		/// Load the file as a standard Xml document, search for a node whose node
		/// name is "TargetFrameworkVersion". If found, then one need look no
		/// further as the .csproj file represents a Full Framework project file.
		/// </summary>
		/// <param name="aFileName"></param>
		/// <returns></returns>
		private static (DotNetFramework, string) CheckForFullFramework(string aFileName)
		{
			(DotNetFramework, string) vResult;
			bool vIsFullFramework;
			string vFrameworkVersion;
			XmlDocument vXml = new XmlDocument();
			vXml.Load(aFileName);
			XmlNamespaceManager vManager = new XmlNamespaceManager(vXml.NameTable);
			vManager.AddNamespace
				(_NAMESPACE, "http://schemas.microsoft.com/developer/msbuild/2003");
			vIsFullFramework =
				vXml.SelectNodes
					($"//{_NAMESPACE}:" + _LOOK_FOR_TARGET_FRAMEWORK_VERSION, vManager).Count > 0;
			if (vIsFullFramework)
			{
				XmlNodeList vSelectedNodes =
					vXml.SelectNodes
						($"//{_NAMESPACE}:" + _LOOK_FOR_TARGET_FRAMEWORK_VERSION, vManager);
				vFrameworkVersion = vSelectedNodes.Item(0).InnerText;
				vResult = (DotNetFramework.Full, vFrameworkVersion);
			}
			else
			{
				vResult = (DotNetFramework.Unknown, string.Empty);
			}
			return vResult;
		}

		private static (DotNetFramework, string) ProcessFramework
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

		/// <remarks>
		/// Why separate methods to search out "Standard" and "Core"? Right now
		/// the process is identical and thus this process is a cpu cycle waster.
		/// Since the files aren't that big, no biggie, but it should be noted that
		/// the author engineer is aware of this and chose to do it anyway simply
		/// because "Things May Change" at MS and this gives the author the
		/// flexibility of adapting to change by applying SOLID techniques.
		/// </remarks>
		private static (DotNetFramework, string) CheckForStandard(string aFileName)
		{
			(XDocument Doc, XElement Node, string Value) vNode =
				aFileName.XDocDocumentAndElementAndValue(_LOOK_FOR_TARGET_FRAMEWORK);
			NodeDocument = vNode.Doc;
			NodeParent = vNode.Node.Parent;
			string vFramework = vNode.Value;
			(DotNetFramework, string) vResult =
				ProcessFramework(vFramework, _NET_STANDARD, DotNetFramework.Standard_2_0);
			return vResult;
		}

		private static (DotNetFramework, string) CheckForCore(string aFileName)
		{
			(XDocument Doc, XElement Node, string Value) vNode =
				aFileName.XDocDocumentAndElementAndValue(_LOOK_FOR_TARGET_FRAMEWORK);
			NodeDocument = vNode.Doc;
			NodeParent = vNode.Node.Parent;
			string vFramework = vNode.Value;
			(DotNetFramework, string) vResult =
				ProcessFramework(vFramework, _NET_CORE_2_0, DotNetFramework.Core_2_0);
			if (vResult.Item1 == DotNetFramework.Unknown)
			{
				vResult =
					ProcessFramework(vFramework, _NET_CORE_2_1, DotNetFramework.Core_2_1);
			}
			return vResult;
		}

		private static (DotNetFramework, string) ExtractTargetFramework()
		{
			string vPath = ProjectPath;
			(DotNetFramework, string) vResult = CheckForFullFramework(vPath);
			if (vResult.Item1 == DotNetFramework.Unknown)
			{
				vResult = CheckForStandard(vPath);
				if (vResult.Item1 == DotNetFramework.Unknown)
				{
					vResult = CheckForCore(vPath);
					if (vResult.Item1 == DotNetFramework.Unknown)
					{
						vResult = UnhandledFramework(vResult.Item2);
					}
				}
			}
			return vResult;
		}

		private static string ExtractPackageVersion()
		{
			string vPath = ProjectPath;
			string vVersion = vPath.ElementValue(_LOOK_FOR_VERSION);
			string vResult =
				!String.IsNullOrWhiteSpace(vVersion)
					? vVersion
					: "No Package Version for this project found.";
			return vResult;
		}

		private static void GeneratePackageOnBuild()
		{
			string vPath = ProjectPath;
			string vGeneratePackageOnBuild =
				vPath.ElementValue(_LOOK_FOR_GENERATE_PACKAGE_ON_BUILD);
			FrameworkInformation.GeneratePackageOnBuild =
				!String.IsNullOrWhiteSpace(vGeneratePackageOnBuild)
					&& vGeneratePackageOnBuild.Equals(bool.TrueString, _COMPARISON);
		}

		public static void ProcessProjectFileContent()
		{
			if (String.IsNullOrWhiteSpace(ProjectPath))
			{
				return;
			}
			GetProjectVersionInfo();
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
				PackageVersion = ExtractPackageVersion();
				GeneratePackageOnBuild();
			}
			else
			{
				PackageVersion = AssemblyVersion;
			}
		}

	}
}
