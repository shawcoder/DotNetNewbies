namespace NuGetHandler
{
	using System;
	using System.Diagnostics;
	using AppConfigHandling;
	using ConfigurationHandler;
	using Infrastructure;
	using Microsoft.Extensions.DependencyInjection;
	using Run_NuGet;
	using static System.Console;

	public class Program
	{
		private static ILaunchPoint _Launch;

		/// <remarks>
		/// Thanks again, Stack Overflow Contributor!
		/// http://stackoverflow.com/questions/3133199/net-global-exception-handler-in-console-application
		/// </remarks>
		/// <param name="aSender"></param>
		/// <param name="aExceptionArgs"></param>
		static void UnhandledExceptionTrapper
			(object aSender, UnhandledExceptionEventArgs aExceptionArgs)
		{
			const int UNIVERSAL_ANSWER = 42;
			foreach (string vLine in ErrorContainer.Errors)
			{
				WriteLine(vLine);
			}
			WriteLine(aExceptionArgs.ExceptionObject.ToString());
			bool vTest =
				Debugger.IsAttached
					|| CommandLineSettings.Wait;
			if (vTest)
			{
				WriteLine("Please press a key to conmplete processing...");
				ReadKey();
			}
			Environment.Exit(UNIVERSAL_ANSWER);
		}

		private static void ConfigureDIServices
		(
			IServiceCollection aCollection
			, ConfigureDIService aConfigureDIService = null
		)
		{
			aCollection.AddSingleton<IHandleConfiguration, HandleConfiguration>();
			aCollection.AddSingleton<ILaunchPoint, LaunchPoint>();
			aCollection.AddSingleton<ISpawnNugetProcesses, SpawnNuGetProcesses>();
			aConfigureDIService?.Invoke(aCollection);
		}

		private static void ConfigureDI()
		{
			ServiceCollection vServiceCollection = new ServiceCollection();
			ConfigureDIServices(vServiceCollection);
			ServiceProvider vServiceProvider =
				vServiceCollection.BuildServiceProvider();
			_Launch = vServiceProvider.GetService<ILaunchPoint>();
		}

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
			ConfigureDI();
			_Launch.Execute(args);
		}

	}
}