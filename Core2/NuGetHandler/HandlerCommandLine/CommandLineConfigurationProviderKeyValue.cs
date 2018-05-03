namespace HandlerCommandLine
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	/// Command line values come in three types:
	///		1.	Switches: Switches are defined as values which are either present or
	///				not present on the command line. If present, then the stronly-typed
	///				container value whose name matches the switch name will be set to
	///				true.
	///				Switches *MUST* be presented to this class at instantiation (at
	///				construction time) in the following form:
	/// 
	///					"switch name in strongly typed class|variant 1|variant 2|..."
	///	
	///					an example: "help", "help|?|h"
	///				
	///				would be the entry for help in that the property name in the
	///				strongly typed class is "help" with two variants: "?" and "h". The
	///				list should be constructed as a dictionary of string, string where
	///				the key is the full, proper name of the switch and the values are
	///				the various variants that are acceptable to the program as
	///				short-hand substitutes for the full name.
	/// 
	///				ALWAYS INCLUDE THE FULL NAME OF THE SWITCH AS ONE OF THE VARIANTS!
	///	
	/// 			SWITCHES ARE CASE-INSENSITIVE!
	/// 
	/// 			The property which represents the switch MUST be defined as a 
	///				boolean such that	the calling program may examine the class instance 
	///				to determine if	the switch is present (the property is true) or 
	///				absent (the switch is false). 
	///				Switches, by default, start with either a dash (-) or a double
	///				dash (--) or a slash (/).
	/// 
	///		2.	Named Values: Named values are switches which come in key-value
	///				form with a separator (the equals (=) character) as a separator.
	///				Given that spaces ARE significant in command line arguments, such
	///				values must be in "name=value" form.
	/// 
	///				One should note that it is IMPOSSIBLE to assign multiple values to
	///				a single switch because of the nature of the gadget itself.
	/// 
	///		3.	Values. Values are POSITION DEPENDENT arguments whose postion in
	///				the command line argument string determines their function within
	///				the application e.g. If the value "C:\Dir" is in position 1, it
	///				may tell the program where to look for a config file, if in
	///				position 2, it might tell where to find a source file.
	/// 
	///				One should note that it is IMPOSSIBLE to assign position-dependent
	///				values because of the nature of the gadget itself. It RELIES upon
	///				the assignment of values to switch names in order to perform the
	///				strongly-typed binding initiated with the Bind method.
	/// </summary>
	public class CommandLineConfigurationProviderKeyValue: ConfigurationProvider
	{
		private const string _SLASH = "/";
		private const string _DASH = "-";
		private const string _DASH_DASH = _DASH + _DASH;
		private const string _SEPARATOR = "=";
		private const string _ALT_SEPARATOR = "~";
		private const string _PIPE = "|";

		private readonly Dictionary<string, string> _Mappings;

		protected IEnumerable<string> Args { get; }

		private Dictionary<string, string> GetValidatedMappingsCopy
			(IDictionary<string, string> aMappings)
		{
			Dictionary<string, string> vResult =
				new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, string> vMapping in aMappings)
			{
				if (aMappings.ContainsKey(vMapping.Key))
				{
					// It's a duplicate but instead of throwing an exception, just apply
					// the lastest value.
					vResult[vMapping.Key] = vMapping.Value;
				}
			}
			return vResult;
		}

		private string ExtractFullNameOfSwitch(string aSwitch)
		{
			string[] vPieces;
			string vResult = String.Empty;
			foreach (KeyValuePair<string, string> vEntry in _Mappings)
			{
				vPieces =
					vEntry.Value.Split
						(new[] { _PIPE }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string vPiece in vPieces)
				{
					if (vPiece.Equals(aSwitch, StringComparison.OrdinalIgnoreCase))
					{
						vResult = vEntry.Key;
						break;
					}
				}
			}
			if (string.IsNullOrWhiteSpace(vResult))
			{
				throw new ArgumentOutOfRangeException
					(nameof(aSwitch), $"Invalid switch: {aSwitch}");
			}
			return vResult;
		}

		/// <summary>
		/// If this is coming just from the command line, then processing of the
		/// Equals symbol (=) works just fine.
		/// HOWEVER, if the call is made from within Visual Studio as of version
		/// 15.6.4, the processing of internal Post Build events REALLY SCREWS UP
		/// handling of parameters with values - to wit, following the recommended
		/// process of setting the flag, then setting the value of the switch via
		/// an attached equals symbol, REALLY HOSES THINGS UP! The processing is
		/// totally off. 
		/// BUT, this situation can be remedied by using an alternate separator, in
		/// this case the tilde (~) symbol as a separator, hence this little method
		/// to get the positon of the separator.
		/// </summary>
		/// <param name="aCurrentArg"></param>
		/// <returns></returns>
		private int FindSeparator(string aCurrentArg)
		{
			int vResult =
				aCurrentArg.IndexOf(_ALT_SEPARATOR, StringComparison.Ordinal);
			if (vResult < 0)
			{
				vResult = aCurrentArg.IndexOf(_SEPARATOR, StringComparison.Ordinal);
			}
			return vResult;
		}

		public CommandLineConfigurationProviderKeyValue
			(IEnumerable<string> aArgs, IDictionary<string, string> aMappings = null)
		{
			Args = aArgs ?? throw new ArgumentNullException(nameof(aArgs));
			_Mappings =
				aMappings != null
					? GetValidatedMappingsCopy(aMappings)
					: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		public override void Load()
		{
			Dictionary<string, string> vData =
				new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			string vKey;
			string vValue;
			int vSeparatorPos = 0;
			int vKeyStartIndex = 0;
			bool vIsASwitch;
			bool vIsANamedValue;
			using (IEnumerator<string> vEnumerator = Args.GetEnumerator())
			{
				while (vEnumerator.MoveNext())
				{
					string vCurrentArg = vEnumerator.Current ?? String.Empty;
					if (vCurrentArg.Equals(_DASH_DASH))
					{
						continue;
					}
					vSeparatorPos = FindSeparator(vCurrentArg);
					if (vCurrentArg.StartsWith(_DASH_DASH))
					{
						vKeyStartIndex = 2;
					}
					else
					{
						if (vCurrentArg.StartsWith(_DASH) || vCurrentArg.StartsWith(_SLASH))
						{
							vKeyStartIndex = 1;
						}
					}
					vIsASwitch = vKeyStartIndex >= 0;
					vIsANamedValue = vSeparatorPos >= 0;
					if (vIsASwitch || vIsANamedValue)
					{
						if (vIsANamedValue)
						{
							vKey =
								vCurrentArg.Substring
									(vKeyStartIndex, vSeparatorPos - vKeyStartIndex);
							vValue = vCurrentArg.Substring(++vSeparatorPos);
						}
						else
						{
							vKey = vCurrentArg.Substring(vKeyStartIndex);
							vValue = bool.TrueString;
						}
						vKey = ExtractFullNameOfSwitch(vKey);
						if (vData.ContainsKey(vKey))
						{
							vData[vKey] = vValue;
						}
						else
						{
							vData.Add(vKey, vValue);
						}
					}
				}
			}
			Data = vData;
		}

	}
}