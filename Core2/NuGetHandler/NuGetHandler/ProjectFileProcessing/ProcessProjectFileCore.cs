namespace NuGetHandler.ProjectFileProcessing
{
	using System;
	using System.Xml.Linq;
	using Infrastructure;
	using static AppConfigHandling.CommandLineSettings;
	using static Consts;
	using static FrameworkInformation;

	public static partial class ProcessProjectFile
	{
		private static (DotNetFramework, string) CheckForCore(string aFileName)
		{
			(XDocument Doc, XElement Node, string Value) vNode =
				aFileName.XDocDocumentAndElementAndValue(_LOOK_FOR_TARGET_FRAMEWORK);
			NodeDocument = vNode.Doc;
			NodeParent = vNode.Node.Parent;
			string vFramework = vNode.Value;
			(DotNetFramework, string) vResult =
				ProcessFrameworkTag(vFramework, _NET_CORE_2_0, DotNetFramework.Core_2_0);
			if (vResult.Item1 == DotNetFramework.Unknown)
			{
				vResult =
					ProcessFrameworkTag(vFramework, _NET_CORE_2_1, DotNetFramework.Core_2_1);
			}
			return vResult;
		}

		private static string ExtractCorePackageVersion()
		{
			string vPath = ProjectPath;
			string vVersion = vPath.ElementValue(_VERSION);
			string vResult =
				!String.IsNullOrWhiteSpace(vVersion)
					? vVersion
					: DEFAULT_VERSION;
			return vResult;
		}

	}
}
