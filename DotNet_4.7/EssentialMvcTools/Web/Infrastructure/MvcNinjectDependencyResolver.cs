namespace Web.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using Models;
	using Ninject;
	using System.Web.Mvc;

	public class MvcNinjectDependencyResolver: IDependencyResolver
	{
		private readonly IKernel _Kernel;

		public MvcNinjectDependencyResolver(IKernel aKernel)
		{
			_Kernel = aKernel;
			AddBindings();
		}

		private void AddBindings()
		{
			_Kernel.Bind<IDiscountHelper>()
						.To<DiscountHelper>()
							.WithPropertyValue(nameof(DiscountHelper.DiscountSize), 15.0M);
			_Kernel.Bind<ILinqValueCalculator>().To<LinqValueCalculator>();
			_Kernel.Bind<IShoppingCart>().To<ShoppingCart>();
		}
		
		public object GetService(Type aServiceType) =>
			_Kernel.TryGet(aServiceType);

		public IEnumerable<object> GetServices(Type aServiceType) =>
			_Kernel.GetAll(aServiceType);

	}
}