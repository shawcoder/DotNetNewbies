#define IoC
namespace IoCWinForm
{
	using System;
	using System.Windows.Forms;
	using IoCExampleSet.InitializeIoCNinject;
	using IoCExampleSet.IoC;
	using IoCExampleSet.IoCExampleSetSupport;

	// Done
	static class ProgramIoCWinForm
	{
		private static mfIoCWinForm _MainForm;

		//--------------------------------------------------------------------------
		// Begin IoC methodology
		//--------------------------------------------------------------------------

		private static void StartUpIoC()
		{
			// Use this instead because of (bad design) the need to tell ninject
			// which constructor to use for IoC/DI. Found at:
			// http://stackoverflow.com/questions/8777475/whats-the-difference-between-toconstructor-and-tomethod-in-ninject-3

			// Kick off the IoC container.
			// Initialize the service locator (InstanceFactory)
			InitializeNinject.StartUp();

			// Instantiate the instance
			// Once again, we are forced to use a Bad Design (the "Service Locator"
			// pattern, but, also again, there is no other way to invoke IoC.
			_MainForm = InstanceFactory.GetInstance<mfIoCWinForm>();
		}

		//--------------------------------------------------------------------------
		// End IoC methodology
		//--------------------------------------------------------------------------

		//--------------------------------------------------------------------------
		// Begin Standard methodology
		//--------------------------------------------------------------------------

		private static void StartUpStandard()
		{
			_MainForm =
				new mfIoCWinForm(new IoCExampleClass(new IoCDependedOnClass()));
		}

		//--------------------------------------------------------------------------
		// End Standard methodology
		//--------------------------------------------------------------------------

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if IoC
			// The IoC way
			StartUpIoC();
#else
			// The standardway
			StartUpStandard();
#endif
			Application.Run(_MainForm);
		}

	}
}