namespace AdamFreemansDualWebSite.Web.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http.Dependencies;
	using Ninject;
	using Ninject.Web.Common;
	using Repository;

	public class NinjectResolver
		: IDependencyResolver, System.Web.Mvc.IDependencyResolver
	{
		private readonly IKernel _Kernel;

		public NinjectResolver() : this(new StandardKernel())
		{
			// Nothind else needed			
		}

		public NinjectResolver(IKernel aKernel)
		{
			_Kernel = aKernel;
			AddBindings(aKernel);
		}

		public IDependencyScope BeginScope() => this;

		public object GetService(Type aServiceType)
		{
			return _Kernel.TryGet(aServiceType);
		}

		public IEnumerable<object> GetServices(Type aServiceType)
		{
			return _Kernel.GetAll(aServiceType);
		}

		public void Dispose()
		{
			// Do nothing
			// Is there really nothing that needs doing here?
		}

		/// <summary>
		/// This seems to be the only place that bindings are necessary and those
		/// bindings are only for things that are passed into the controllers, 
		/// there seems to be no need to prepare bindings of any sort for either
		/// type of Controller - Mvc or WebApi. I have tried both leaving in and 
		/// removing the default parameterless constructors for the Controllers,
		/// the gadget seems to not care in the slightest - before, I was getting 
		/// a "Missing Parameter-less constructor" exception. Now, it Just Works!
		/// </summary>
		/// <param name="aKernel"></param>
		private void AddBindings(IKernel aKernel)
		{
			aKernel.Bind<IRepository>().To<Repository>().InRequestScope();
		}

	}
}
