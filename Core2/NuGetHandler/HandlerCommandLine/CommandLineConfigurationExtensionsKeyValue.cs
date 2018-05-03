namespace HandlerCommandLine
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Extensions.Configuration;

	public static class CommandLineConfigurationKeyValueHelper
	{
		public static IConfigurationBuilder AddCommandLineKeyValue
		(
			this IConfigurationBuilder aBuilder
			, string[] aArgs
		)
		{
			return aBuilder.AddCommandLineKeyValue(aArgs, null);
		}

		public static IConfigurationBuilder AddCommandLineKeyValue
		(
			this IConfigurationBuilder aBuilder
			, string[] aArgs
			, IDictionary<string, string> aMappings
		)
		{
			CommandLineConfigurationSourceKeyValue vSource =
				new CommandLineConfigurationSourceKeyValue
				{
					Args = aArgs
					, Mappings = aMappings
				};
			aBuilder.Add(vSource);
			return aBuilder;
		}

		public static IConfigurationBuilder AddCommandLineKeyValue
		(
			this IConfigurationBuilder aBuilder
			, Action<CommandLineConfigurationSourceKeyValue> aConfig
		) => aBuilder.Add(aConfig);

	}
}
