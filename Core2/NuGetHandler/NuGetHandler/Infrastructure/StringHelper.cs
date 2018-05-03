namespace NuGetHandler.Infrastructure
{
	using System;
	using System.IO;

	public static class StringHelper
	{
		public static string SetLastChar(this string aString, char aChar)
		{
			string vResult =
				aString
					+ (
							aString.EndsWith(aChar)
								? String.Empty
								: aChar.ToString()
						);
			return vResult;
		}

		public static string AsDir(this string aString)
		{
			string vResult = aString.SetLastChar(Path.DirectorySeparatorChar);
			return vResult;
		}

		public static string AsDirName(this string aString)
		{
			string vResult =
				aString.EndsWith(Path.DirectorySeparatorChar)
					? aString.Remove(aString.Length - 1)
					: aString;
			return vResult;
		}

		public static string AsSpacePrefixed(this string aString)
		{
			string vResult = " " + aString;
			return vResult;
		}

		public static string AsSpaceEncapsulated(this string aString)
		{
			string vResult = aString.AsSpacePrefixed() + " ";
			return vResult;
		}

		public static string AsToken(this string aTokenName)
		{
			string vResult = $"${aTokenName}$";
			return vResult;
		}
	}
}
