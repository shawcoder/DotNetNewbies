namespace HandlerCommandLine
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	/// Command line values come in three types:
	///		1.	Switches: Switches are command line values which are preceded by
	///									either a double dash (--) or a slash (/).
	/// 
	///				Switches are defined as values which are either present or
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
	/// 			SWITCHES ARE CASE-**IN**-SENSITIVE!
	/// 
	/// 			The property which represents the switch MUST be defined as a 
	///				boolean such that	the calling program may examine the class instance 
	///				to determine if	the switch is present (the property is true) or 
	///				absent (the switch is false). 
	/// 
	///		2.	Named Values: Named values are command line values which come in
	///											pairs. First the "owning" name of the named value,
	///											which is preceded by a SINGLE dash (-), followed by
	///											the "value" portion fo the named value pair.
	/// 
	///				Named values are switches which come in key-value
	///				form such that the value is the next argument in the list of
	///				arguments as presented to the command line e.g.
	///
	///					-ProjectPath "C:\Some\Project\Path\File"
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
	///
	///				In other words, all "Values" must be of the Named Value variant.
	/// </summary>
	public class CommandLineConfigurationProviderIsolated: ConfigurationProvider
	{
		private const string _SLASH = "/";
		private const string _DASH = "-";
		private const string _DASH_DASH = _DASH + _DASH;
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

		public CommandLineConfigurationProviderIsolated
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
			string vCurrentArg;
			string vLastNamedValueName = null;
			string vKey;
			string vValue;
			int vKeyStartIndex;
			bool vIsASwitch;
			bool vIsANamedValue;
			using (IEnumerator<string> vEnumerator = Args.GetEnumerator())
			{
				while (vEnumerator.MoveNext())
				{
					vCurrentArg = vEnumerator.Current ?? String.Empty;
					if (vCurrentArg.Equals(_DASH) || vCurrentArg.Equals(_DASH_DASH))
					{
						continue;
					}
					vIsASwitch =
						vCurrentArg.StartsWith(_DASH_DASH)
							|| vCurrentArg.StartsWith(_SLASH);
					vIsANamedValue = vCurrentArg.StartsWith(_DASH) && !vIsASwitch;
					vKeyStartIndex =
						vIsASwitch
							? 2
							: vIsANamedValue
									? 1
									: 0;
					if (vKeyStartIndex > 0)
					{
						vKey = vCurrentArg.Substring(vKeyStartIndex);
						vKey = ExtractFullNameOfSwitch(vKey);
					}
					else
					{
						vKey = String.Empty;
					}
					// If this is a named value, just move to the next entry to fetch
					// the value of the named value pair. If there isn't one, no biggie
					// as the routine will just quit without adding the final value.
					if (vIsANamedValue)
					{
						vLastNamedValueName = vKey;
						continue;
					}
					if (vIsASwitch)
					{
						vValue = bool.TrueString;
					}
					else
					{
						vKey =
							!String.IsNullOrWhiteSpace(vLastNamedValueName)
								? vLastNamedValueName
								: "SomeNewRandomKey" + Guid.NewGuid().ToString("N");
						vValue = vCurrentArg;
					}
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
			Data = vData;
		}

	}
}
