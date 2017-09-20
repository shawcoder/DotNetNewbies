namespace IocConsole
{
	using System;
	using AWEFramework.IoC;
	using AWEFramework.NinjectInitializer;
	using IoCExampleLibrary;

	// Done
	public class ProgramIoCConsole
	{
		private readonly IIoCExampleClass _IoCExampleClass;

		public static int Main(string[] args)
		{
			// Kick off the IoC container.
			// Initialize the service locator (InstanceFactory)
			InitializeNinject.StartUp();

			// Instantiate the instance
			ProgramIoCConsole vProgram = InstanceFactory.GetInstance<ProgramIoCConsole>();
			int vResult = vProgram.DoTheProgram(args);
			return vResult;
		}

		public ProgramIoCConsole(IIoCExampleClass aIoCExampleClass)
		{
			if (aIoCExampleClass == null)
			{
				throw new ArgumentNullException("aIoCExampleClass");
			}
			_IoCExampleClass = aIoCExampleClass;
		}

		public int DoTheProgram(string[] args)
		{
			_IoCExampleClass.DoTheStuff();
			return 0;
		}

	}
}
