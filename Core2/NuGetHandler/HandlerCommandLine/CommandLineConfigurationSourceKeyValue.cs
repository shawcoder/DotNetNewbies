namespace HandlerCommandLine
{
	using System.Collections.Generic;
	using Microsoft.Extensions.Configuration;

	public class CommandLineConfigurationSourceKeyValue: IConfigurationSource
	{
		public IDictionary<string, string> Mappings { get; set; }
		public IEnumerable<string> Args { get; set; }

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new CommandLineConfigurationProviderKeyValue(Args, Mappings);
		}

	}
}
