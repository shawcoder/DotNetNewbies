namespace NuGetHandler.Infrastructure
{
	using System;
	using System.Linq;
	using System.Xml.Linq;

	public static class XmlFileLoader
	{
		public static XElement FindElement(this XDocument aDoc, string aLookFor)
		{
			const StringComparison COMPARISON =
				StringComparison.InvariantCultureIgnoreCase;
			XElement vResult =
				aDoc
					.Descendants()
					.FirstOrDefault
					(
						vRec =>
							vRec.Name.ToString().Equals(aLookFor, COMPARISON)
					);
			return vResult;
		}

		private static XElement FindElement(this string aFileName, string aLookFor)
		{
			XDocument vDoc = aFileName.LoadAsXDocument();
			XElement vResult = vDoc.FindElement(aLookFor);
			return vResult;
		}

		public static XDocument LoadAsXDocument(this string aFileName)
		{
			XDocument vResult = XDocument.Load(aFileName);
			return vResult;
		}

		public static (XElement, string) ElementParentAndValue
			(this string aFileName, string aLookFor)
		{
			XElement vNode = aFileName.FindElement(aLookFor);
			(XElement, string) vResult = (vNode?.Parent, vNode?.Value);
			return vResult;
		}

		public static (XDocument, XElement, string) XDocDocumentAndElementAndValue
			(this string aFileName, string aLookFor)
		{
			XDocument vDoc = aFileName.LoadAsXDocument();
			XElement vElement = vDoc.FindElement(aLookFor);
			(XDocument, XElement, string) vResult = (vDoc, vElement, vElement?.Value);
			return vResult;
		}

		public static string ElementValue(this string aFileName, string aLookFor)
		{
			string vResult = aFileName.FindElement(aLookFor)?.Value;
			return vResult;
		}

	}
}
