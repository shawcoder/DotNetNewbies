namespace AppConfigConfigurationBuilder
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;
	using Microsoft.Extensions.Configuration;
	using static AppConfigConfigurationProviderBaseHelper;

	public class AppConfigConfigurationProviderBase: FileConfigurationProvider
	{
		private const string _NAME = "name";
		private const string _CONNECTION_STRING = "connectionString";
		private const string _CONNECTION_STRINGS = "connectionStrings";
		protected XDocument XDoc;

		protected Dictionary<string, string> ParseSectionToDictionary
			(string aSectionName)
		{
			Dictionary<string, string> vResult =
				ParseFromXDocument(XDoc, aSectionName);
			return vResult;
		}

		protected void ParseToNamedSection(string aSectionName)
		{
			Dictionary<string, string> vSectionContent =
				ParseSectionToDictionary(aSectionName);
			foreach (KeyValuePair<string, string> vPair in vSectionContent)
			{
				Data.Add($"{aSectionName}:{vPair.Key}", vPair.Value);
			}
		}

		protected void ParseStreamForKeyValuePairs(string aSectionName)
		{
			Dictionary<string, string> vContent =
				ParseSectionToDictionary(aSectionName);
			foreach (KeyValuePair<string, string> vItem in vContent)
			{
				Data.Add(vItem.Key, vItem.Value);
			}
		}

		protected void ParseStreamForConnectionStrings()
		{
			XElement vAppSettings =
				XDoc.Root?.Elements(_CONNECTION_STRINGS).FirstOrDefault();
			if (vAppSettings == null)
			{
				return;
			}
			Dictionary<string, string> vContent =
			(
				vAppSettings.DescendantNodes()
					.Select
					(
						vNode =>
							new
							{
								vNode
								, vElement = vNode as XElement
							}
					)
						.Where
						(
							vNode =>
								(vNode.vElement != null) && vNode.vElement.HasAttributes
						)
							.Select
							(
								vItem =>
									new
									{
										Key = vItem.vElement.Attribute(_NAME)?.Value
										, Value = vItem.vElement.Attribute(_CONNECTION_STRING)?.Value
									}
							)
			).ToDictionary(e => e.Key, e => e.Value);
			foreach (KeyValuePair<string, string> vItem in vContent)
			{
				Data.Add(vItem.Key, vItem.Value);
			}
		}

		public AppConfigConfigurationProviderBase(FileConfigurationSource aSource)
			: base(aSource)
		{
		}

		public override void Load(Stream aStream)
		{
			XDoc = XDocument.Load(aStream);
		}

	}
}
