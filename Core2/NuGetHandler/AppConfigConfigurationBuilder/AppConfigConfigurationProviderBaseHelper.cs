namespace AppConfigConfigurationBuilder
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;

	public static class AppConfigConfigurationProviderBaseHelper
	{
		private const string _KEY = "key";
		private const string _VALUE = "value";

		public static Dictionary<string, string> ParseFromFileName
			(string aSectionName)
		{
			XDocument vDoc = XDocument.Load(ConfigFileName);
			Dictionary<string, string> vResult =
				ParseFromXDocument(vDoc, aSectionName);
			return vResult;
		}

		public static Dictionary<string, string> ParseFromStream
			(Stream aStream, string aSectionName)
		{
			XDocument vDoc = XDocument.Load(aStream);
			Dictionary<string, string> vResult =
				ParseFromXDocument(vDoc, aSectionName);
			return vResult;
		}

		public static Dictionary<string, string> ParseFromXDocument
			(XDocument aDoc, string aSectionName)
		{
			XElement vAppSettings =
				aDoc.Root?.Elements(aSectionName).FirstOrDefault();
			if (vAppSettings == null)
			{
				return new Dictionary<string, string>();
			}
			Dictionary<string, string> vResult =
			(
				from vNode in vAppSettings.DescendantNodes()
				let vElement = vNode as XElement
				where (vElement != null) && vElement.HasAttributes
				select new
				{
					Key = vElement.Attribute(_KEY)?.Value
					, Value = vElement.Attribute(_VALUE)?.Value
				}
			).ToDictionary
			(
				vKey => vKey.Key
				, vValue => vValue.Value
			);
			return vResult;
		}

		public static string ConfigFileName { get; set; }
	}
}
