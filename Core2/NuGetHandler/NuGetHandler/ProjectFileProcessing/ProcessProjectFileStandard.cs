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
				ProcessFrameworkTag
					(vFramework, _NET_STANDARD, DotNetFramework.Standard_2_0);
			return vResult;
		}

		private static string ExtractStandardPackageVersion()
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
