namespace NuGetHandler.ProjectFileProcessing
{
	using System;

	public static partial class ProcessProjectFile
	{
		private static (DotNetFramework, string) CheckForUniversalWindows
			(string aFileName)
		{
			(DotNetFramework, string) vResult =
				(DotNetFramework.Unknown, String.Empty);
			return vResult;
		}

		private static void ExtractUniversalWindowsVersionInfo() { throw new NotImplementedException(); }

	}
}
