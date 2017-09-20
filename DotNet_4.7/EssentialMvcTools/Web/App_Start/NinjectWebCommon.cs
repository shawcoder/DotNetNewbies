using Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Web
{
	using System;
	using System.Web;
	using System.Web.Http;
	using Infrastructure;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Web.Common;
	using Ninject.Web.WebApi;

	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper _Bootstrapper = new Bootstrapper();

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			_Bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop()
		{
			_Bootstrapper.ShutDown();
		}

		/// <summary>
		/// Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel()
		{
			StandardKernel vKernel = new StandardKernel();
			try
			{
				vKernel
					.Bind<Func<IKernel>>()
					.ToMethod(ctx => () => new Bootstrapper().Kernel);
				vKernel
					.Bind<IHttpModule>()
					.To<HttpApplicationInitializationHttpModule>();

				RegisterServices(vKernel);
				GlobalConfiguration.Configuration.DependencyResolver =
					new NinjectDependencyResolver(vKernel);
				return vKernel;
			}
			catch
			{
				vKernel.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Load your modules or register your services here!
		/// </summary>
		/// <param name="aKernel">The kernel.</param>
		private static void RegisterServices(IKernel aKernel)
		{
			System.Web.Mvc.DependencyResolver.SetResolver
				(new MvcNinjectDependencyResolver(aKernel));
		}
		
	}
}
