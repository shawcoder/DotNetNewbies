namespace NuGetSimulator
{
	using static System.Console;

	/// <summary>
	/// This is the Standard/.NET Core variant of the NuGet Simulator
	/// </summary>
	public class Program
	{
		private static int _EXIT_CODE = 0;

		static int Main(string[] args)
		{
			WriteLine("\nBegin .NET Standard/.NET Core Simulator\n");
			foreach (string vArg in args)
			{
				WriteLine($"Argument: {vArg}");
			}
			WriteLine("\nPress a key to continue...");
			ReadKey();
			WriteLine("\nEnd Simulator\n");
			return _EXIT_CODE;
		}


	}
}