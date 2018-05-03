namespace NuGetHandler.AppConfigHandling
{
	using System;
	using System.IO;
	using AppConfigConfigurationBuilder;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.FileProviders;

	public class AppConfigConfigurationSource: FileConfigurationSource
	{
		public override IConfigurationProvider Build(IConfigurationBuilder aBuilder)
		{
			if (FileProvider == null)
			{
				FileProvider = aBuilder.GetFileProvider();
			}
			return new AppConfigConfigurationProvider(this);
		}

	}

	public static class AppConfigConfigurationHelper
	{
		public static IConfigurationBuilder AddAppConfig
		(
			this IConfigurationBuilder aBuilder
			, string aPath
			, bool aOptional = false
			, bool aReloadOnChange = false
			, IFileProvider aProvider = null
		)
		{
			if (aProvider == null && Path.IsPathRooted(aPath))
			{
				AppConfigConfigurationProviderBaseHelper.ConfigFileName = aPath;
				aProvider = new PhysicalFileProvider(Path.GetDirectoryName(aPath));
				aPath = Path.GetFileName(aPath);
			}
			else
			{
				AppConfigConfigurationProviderBaseHelper.ConfigFileName =
					Path.Combine(Directory.GetCurrentDirectory(), aPath);
				throw new Exception("We should'nt be here!");
			}
			AppConfigConfigurationSource vSource =
				new AppConfigConfigurationSource
				{
					FileProvider = aProvider,
					Path = aPath,
					Optional = aOptional,
					ReloadOnChange = aReloadOnChange
				};
			aBuilder.Add(vSource);
			return aBuilder;
		}

	}
}
