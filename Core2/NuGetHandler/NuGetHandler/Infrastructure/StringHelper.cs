namespace NuGetHandler.Infrastructure
{
	using System;
	using System.IO;
	using static Consts;

	public static class StringHelper
	{
		private const int _PAD_INTO = 30;

		public static int IndexOf
		(
			this string aString
			, string aLookFor
			, int aStartIndex
			, StringComparison aStringComparison
		)
		{
			bool vTest =
				String.IsNullOrEmpty(aString)
					|| String.IsNullOrEmpty(aLookFor)
					|| (aStartIndex >= aString.Length);
			if (vTest)
			{
				return -1;
			}
			string vSearchString = aString.Substring(aStartIndex);
			int vResult = vSearchString.IndexOf(aLookFor, COMPARISON);
			return vResult;
		}

		public static string AsConditionallyQuoted(this string aString)
		{
			return aString.Contains(" ") ? $@"""{aString}""" : aString;
		}

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

		public static string PadItRight(this string aWord)
		{
			if (String.IsNullOrEmpty(aWord))
			{
				aWord = String.Empty;
			}
			string vResult = aWord + ":".PadRight(_PAD_INTO - aWord.Length);
			return vResult;
		}

		public static string PadItLeft
			(this string aWord, bool aInsertNewLine = false)
		{
			if (String.IsNullOrEmpty(aWord))
			{
				aWord = String.Empty;
			}
			string vResult =
				String.Empty.PadLeft(_PAD_INTO)
					+ aWord
					+ (
							aInsertNewLine
								? "\n"
								: String.Empty
						);
			return vResult;
		}

	}
}
