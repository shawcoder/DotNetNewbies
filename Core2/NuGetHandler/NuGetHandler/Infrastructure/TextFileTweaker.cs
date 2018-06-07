namespace NuGetHandler.Infrastructure
{
	using System;
	using System.IO;
	using System.Linq;
	using static Consts;

	/// <summary>
	/// This class is used to read in an xml-based csproj file for any framework
	/// and treat it as a simple text file. It will also load and fiddle a
	/// corresponding AssemblyInfo file and tweak it the same way.
	/// The methods herein are designed to
	/// find a particular key in the xml file, extract its content, allow
	/// modification of said content, then write said content back into the file.
	/// This will work regardless of whatever xml variant Microsoft settles on
	/// (except some binary variant) because all standard xml can be treated as a
	/// simple string. If the key being sought is unique to the file, then this
	/// class can help with manipulating the file.
	/// It should be noted that since everything herein is declared as local
	/// variables, it all goes on the stack so really big project files may
	/// kill this thing. But for what the author is currently working on, the
	/// project files will be a few Kilobytes in size, nothing more and nothing to
	/// worry about.
	/// </summary>
	public static class TextFileTweaker
	{
#if DEBUG
		private const bool _MAKE_BACKUP = true;
		private const bool _WRITE_TO_BACKUP = true;
#else
		private const bool _MAKE_BACKUP = false;
		private const bool _WRITE_TO_BACKUP = false;
#endif

		private static string _FilePath;
		private static string _FileBackupPath;
		private static string _FileContent = String.Empty;
		private static string _EndKey;

		/// <summary>
		/// Make sure that the _FilePath actually points to something before we
		/// try manipulating the content of said file.
		/// </summary>
		private static void CheckFilePath()
		{
			if (String.IsNullOrWhiteSpace(_FilePath))
			{
				throw new Exception("You forgot to call SetTextFilePath dummy!");
			}
			if (!File.Exists(_FilePath))
			{
				throw new Exception($"File not found: {_FilePath}");
			}
		}

		private static void LoadFileContent()
		{
			// If the file has already been loaded, don't load it again.
			if (!String.IsNullOrEmpty(_FileContent))
			{
				return;
			}
			CheckFilePath();
			_FileContent = File.ReadAllText(_FilePath);
			// Make a backup if desired
			if (_MAKE_BACKUP)
			{
				_FileBackupPath = _FilePath + ".bak";
				File.WriteAllText(_FileBackupPath, _FileContent);
			}
		}

		/// <summary>
		/// This method ensures that the key is unique in the file and, by doing so,
		/// loads the file content after validating that the file even exists. The
		/// appropriate exception is thrown if the file is empty, does not exist or
		/// the key is not unique.
		/// If the key is indeed unique and it starts with the traditional content
		/// key marker (a less-than sign), then automatically create the end key
		/// for use in other methods.
		/// </summary>
		/// <param name="aKey"></param>
		private static void CheckKey(this string aKey)
		{
			if (!aKey.IsKeyUniqueInFile())
			{
				throw new Exception
					($"Key is not unique: {aKey}\n in file: {_FilePath}");
			}
			_EndKey =
				aKey.StartsWith(KEY_START)
					? aKey.Replace(KEY_START, KEY_END_START)
					: RIGHT_PAREN + RIGHT_BRACKET;
		}

		/// <summary>
		/// Returns the starting index of the key and corresponding end key.
		/// </summary>
		/// <param name="aKey"></param>
		/// <returns></returns>
		private static (int vKeyIndex, int vEndKeyIndex) GetKeyIndexes
			(this string aKey)
		{
			LoadFileContent();
			(int vKeyIndex, int vEndKeyIndex) vResult;
			vResult.vKeyIndex = _FileContent.IndexOf(aKey, COMPARISON);
			vResult.vEndKeyIndex =
				_FileContent.IndexOf(_EndKey, vResult.vKeyIndex, COMPARISON);
			return vResult;
		}

		private static bool KeyHasContent(this string aKey)
		{
			(int vKeyIndex, int vEndKeyIndex) vIndexes = aKey.GetKeyIndexes();
			bool vResult =
				vIndexes.vKeyIndex + aKey.Length + 1 == vIndexes.vEndKeyIndex;
			return vResult;
		}

		public static void SetTextFilePath
			(this string aFilePath, bool aAutoLoadFile = true)
		{
			ClearFileContent();
			_FilePath =
				File.Exists(aFilePath)
					? aFilePath
					: throw new Exception($"File does not exist: {aFilePath}");
			if (aAutoLoadFile)
			{
				LoadFileContent();
			}
		}

		public static void ClearFileContent() { _FileContent = String.Empty; }

		/// <summary>
		/// Search the entire file looking for instances of aKey. If more than one
		/// instance is found, this method will return false. If zero instances of
		/// the key are found, this method will return false. If a single instance
		/// of the key is found, this method will return true.
		/// </summary>
		/// <param name="aKey"></param>
		/// <returns></returns>
		public static bool IsKeyUniqueInFile(this string aKey)
		{
			LoadFileContent();
			bool vResult =
				_FileContent
					.Where
					(
						(aItem, aIndex) =>
							_FileContent.Substring(aIndex).StartsWith(aKey)
					)
					.Count()
					.Equals(1);
			return vResult;
		}

		public static string ExtractKeyContents(this string aKey)
		{
			LoadFileContent();
			aKey.CheckKey();
			string vResult = String.Empty;
			if (aKey.KeyHasContent())
			{
				(int vKeyIndex, int vEndKeyIndex) vIndexes = aKey.GetKeyIndexes();
				int vStartAt = vIndexes.vKeyIndex + aKey.Length + 1;
				int vHowMany = vIndexes.vEndKeyIndex - vStartAt;
				vResult = _FileContent.Substring(vStartAt, vHowMany);
			}
			return vResult;
		}

		/// <summary>
		/// Variant of the ExtractKeyContent method in that this method not only
		/// returns the value of the associated key, but the index in the file
		/// that represents the beginning index of the key start.
		/// </summary>
		/// <param name="aKey"></param>
		/// <returns></returns>
		public static (int vKeyIndex, string vKeyValue) ExtractKeyContent
			(this string aKey)
		{
			LoadFileContent();
			aKey.CheckKey();
			(int vKeyIndex, string vKeyValue) vResult;
			vResult.vKeyValue = aKey.ExtractKeyContents();
			vResult.vKeyIndex = aKey.GetKeyIndexes().vKeyIndex;
			return vResult;
		}

		public static void UpdateKeyValue
			(this string aKey, string aNewValue, int aPreviousIndex = -1)
		{
			LoadFileContent();
			aKey.CheckKey();
			(int vKeyIndex, string vKeyValue) vCurrentValue =
				aKey.ExtractKeyContent();
			bool vTest =
				(aPreviousIndex > -1)
					&& (aPreviousIndex != vCurrentValue.vKeyIndex);
			if (vTest)
			{
				throw new Exception
				(
					$"Key index has changed from {aPreviousIndex} to {vCurrentValue.vKeyIndex}.");
			}
		}

		public static void EraseBackup()
		{
			if (_MAKE_BACKUP)
			{
				File.Delete(_FileBackupPath);
			}
		}

	}
}
