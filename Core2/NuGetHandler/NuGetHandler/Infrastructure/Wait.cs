namespace NuGetHandler.Infrastructure
{
	using System.Diagnostics;
	using AppConfigHandling;
	using static System.Console;

	public static class Wait
	{
		public static void Pause()
		{
			bool vTest = Debugger.IsAttached || CommandLineSettings.Wait;
			if (vTest)
			{
				WriteLine("\nPress a key to conmplete processing...");
				ReadKey();
			}
		}

	}
}
