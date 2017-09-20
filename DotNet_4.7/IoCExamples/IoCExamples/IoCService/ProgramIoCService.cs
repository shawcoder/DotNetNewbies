namespace IoCService
{
	using System.Diagnostics;
	using System.ServiceProcess;
	using AWEFramework.IoC;
	using AWEFramework.NinjectInitializer;

	static class ProgramIoCService
	{
		static void Main(string[] args)
		{
			// Kick off the IoC container.
			// Initialize the service locator (InstanceFactory)
			InitializeNinject.StartUp();

			// Instantiate the instance
			// ServiceBase[] ServicesToRun;
			// ServicesToRun = new ServiceBase[] { new IoCService() };
			IoCService vService = InstanceFactory.GetInstance<IoCService>();
#if DEBUG
			if (Debugger.IsAttached && (args != null) && (args.Length > 0) && (args[0] == "-Console"))
			{
				vService.Timer_Elapsed(vService, null);
				return;
			}
#endif
			ServiceBase.Run(vService);
		}

	}
}
