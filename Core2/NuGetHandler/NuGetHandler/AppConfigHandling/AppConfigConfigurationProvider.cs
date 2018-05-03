namespace NuGetHandler.AppConfigHandling
{
	using System.IO;
	using AppConfigConfigurationBuilder;
	using Microsoft.Extensions.Configuration;

	public class AppConfigConfigurationProvider: AppConfigConfigurationProviderBase
	{
		private const string _APP_SETTINGS = "appSettings";
		private const string _NUGET_NUSPEC_VALUES = "NuGetNuSpecSettings";

		public AppConfigConfigurationProvider
			(FileConfigurationSource aSource)
			: base(aSource)
		{
		}

		public override void Load(Stream aStream)
		{
			base.Load(aStream);
			ParseStreamForConnectionStrings();
			ParseStreamForKeyValuePairs(_APP_SETTINGS);
			ParseStreamForKeyValuePairs(_NUGET_NUSPEC_VALUES);
		}

	}
}