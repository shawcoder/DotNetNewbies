namespace SingletonClient
{
	using Singleton;
	using static System.Console;

	class Program
	{
		static void Main()
		{
			int vRunningCount = SerialNumberGenerator.Instance.NextSerial;
			WriteLine(vRunningCount);
			vRunningCount = SerialNumberGenerator.Instance.NextSerial;
			WriteLine(vRunningCount);
			vRunningCount = SerialNumberGenerator.Instance.NextSerial;
			WriteLine(vRunningCount);
			vRunningCount = SerialNumberGenerator.Instance.NextSerial;
			WriteLine(vRunningCount);
			vRunningCount = SerialNumberGenerator.Instance.NextSerial;
			WriteLine(vRunningCount);

			ReadKey();
		}
	}
}
