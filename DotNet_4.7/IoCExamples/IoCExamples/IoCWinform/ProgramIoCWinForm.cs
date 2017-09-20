namespace IoCWinform
{
	using System;
	using System.Windows.Forms;
	using AWEFramework.IoC;
	using AWEFramework.NinjectInitializer;

	// Done
	static class ProgramIoCWinForm
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// Kick off the IoC container.
			// Initialize the service locator (InstanceFactory)
			InitializeNinject.StartUp();

			// Use this instead because of (bad design) the need to tell ninject
			// which constructor to use for IoC/DI. Found at:
			// http://stackoverflow.com/questions/8777475/whats-the-difference-between-toconstructor-and-tomethod-in-ninject-3
			// The following example was pulled from the DapperPlayground project.
			// It worked as needed until a much-needed refactoring took place which
			// removed the necessity for the following code:
			//IKernel vKernel = InitializeNinject.StartupAndReturnKernel();
			//vKernel
			//	.Rebind<ISqlServerConnectionFactory>()
			//	.ToConstructor<SqlServerConnectionFactory>
			//	(
			//		ctorArg =>
			//			new SqlServerConnectionFactoryNamedConnection(ctorArg.Inject<IAWESettings>())
			//	);

			// Instantiate the instance
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			mfIoCWinForm vForm = InstanceFactory.GetInstance<mfIoCWinForm>();
			Application.Run(vForm);
		}

	}
}
