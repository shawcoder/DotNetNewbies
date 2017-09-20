// ReSharper disable JoinDeclarationAndInitializer
#define IoC
namespace IoCExampleSet.IoCConsole
{
	using System;
	using InitializeIoCNinject;
	using IoC;
	using IoCExampleSetSupport;

	// Done
	public class ProgramIoCConsole
	{
		private static IIoCExampleClass _IoCExampleClass;
		private static string _Token;

		//--------------------------------------------------------------------------
		// Begin IoC methodology
		//--------------------------------------------------------------------------
		// Program constructor
		public ProgramIoCConsole(IIoCExampleClass aIoCExampleClass)
		{
			_IoCExampleClass =
				aIoCExampleClass
					?? throw new ArgumentNullException(nameof(aIoCExampleClass));
		}

		private int DoTheProgram(string[] args)
		{
			_Token = _IoCExampleClass.DoTheStuff();
			return 0;
		}

		private static int DoItTheIoCWay(string[] args)
		{
			// Kick off the IoC container.
			// Initialize the service locator (InstanceFactory)
			InitializeNinject.StartUp();

			// Instantiate the instance
			// Normally, this is a Bad Practice. However, since there isn't an easier
			// way to jump start the IoC, the program needs to go get the initial
			// instance.
			ProgramIoCConsole vProgram =
				InstanceFactory.GetInstance<ProgramIoCConsole>();
			int vResult = vProgram.DoTheProgram(args);
			return vResult;
		}

		//--------------------------------------------------------------------------
		// End IoC methodology
		//--------------------------------------------------------------------------

		//--------------------------------------------------------------------------
		// Begin standard methodology
		//--------------------------------------------------------------------------
		private static int DoItTheRegularWay(string[] args)
		{
			IIoCDependedOnClass vDependedOnClass = new IoCDependedOnClass();
			_IoCExampleClass = new IoCExampleClass(vDependedOnClass);
			_Token = _IoCExampleClass.DoTheStuff();
			return 0;
		}

		//--------------------------------------------------------------------------
		// End standard methodology
		//--------------------------------------------------------------------------

		//--------------------------------------------------------------------------
		// Main entry point for console application
		//--------------------------------------------------------------------------
		public static int Main(string[] args)
		{
			int vResult;

#if IoC
			// Via IoC
			vResult = DoItTheIoCWay(args);
#else
			// Via the "old fashioned" way
			vResult = DoItTheRegularWay(args);
#endif
			Console.WriteLine(_Token);
			Console.ReadKey();

			return vResult;
		}

	}
}
