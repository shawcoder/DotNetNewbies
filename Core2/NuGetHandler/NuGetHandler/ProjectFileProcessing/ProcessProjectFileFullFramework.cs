namespace NuGetHandler.ProjectFileProcessing
{
	using System;
	using System.Xml;

	public static partial class ProcessProjectFile
	{
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
			string vFrameworkVersion;
			XmlDocument vXml = new XmlDocument();
			vXml.Load(aFileName);
			XmlNamespaceManager vManager = new XmlNamespaceManager(vXml.NameTable);
			vManager.AddNamespace
				(_NAMESPACE, "http://schemas.microsoft.com/developer/msbuild/2003");
			bool vIsFullFramework =
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


		private static void ExtractFullFrameworkVersionInfo() { throw new NotImplementedException(); }

	}
}
