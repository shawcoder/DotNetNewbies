namespace HandlerCommandLine
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Configuration;

	public static class CommandLineConfigurationIsolatedHelper
	{
		public static IConfigurationBuilder AddCommandLineIsolated
		(
		this IConfigurationBuilder aBuilder
		, string[] aArgs
		)
		{
			return aBuilder.AddCommandLineIsolated(aArgs, null);
		}

		public static IConfigurationBuilder AddCommandLineIsolated
		(
		this IConfigurationBuilder aBuilder
		, string[] aArgs
		, IDictionary<string, string> aMappings
		)
		{
			CommandLineConfigurationSourceIsolated vSource =
				new CommandLineConfigurationSourceIsolated
				{
					Args = aArgs
					, Mappings = aMappings
				};
			aBuilder.Add(vSource);
			return aBuilder;
		}

		public static IConfigurationBuilder AddCommandLineIsolated
		(
		this IConfigurationBuilder aBuilder
		, Action<CommandLineConfigurationSourceIsolated> aConfig
		) => aBuilder.Add(aConfig);

	}
}
