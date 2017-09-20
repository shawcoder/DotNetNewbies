using Web;
using WebActivator;

[assembly:PostApplicationStartMethod(typeof(SimpleInjectorWebApiInitializer), "Initialize")]

namespace Web
{
	using System.Reflection;
	using System.Web.Http;
	using Infrastructure;
	using Models;
	using SimpleInjector;
	using SimpleInjector.Integration.WebApi;

	public static class SimpleInjectorWebApiInitializer
	{
		/// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
		public static void Initialize()
		{
			var vContainer = new Container();
			// To use the "greediest constructor" paradigm, add the following line:
			vContainer.Options.ConstructorResolutionBehavior = 
				new MostResolvableParametersConstructorResolutionBehavior(vContainer);
			
			vContainer.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			InitializeContainer(vContainer);

			vContainer.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			// From the docs, these next two lines need to be added for MVC
			vContainer.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			vContainer.RegisterMvcIntegratedFilterProvider();
			
			vContainer.Verify();

			GlobalConfiguration.Configuration.DependencyResolver =
				new SimpleInjectorWebApiDependencyResolver(vContainer);
		}

		private static void InitializeContainer(Container aContainer)
		{
			aContainer.Register<IDiscountHelper, DiscountHelper>();
			aContainer.RegisterInitializer<DiscountHelper>
			(
				i =>
					{
						i.DiscountSize = 15M;
					}
			);
			aContainer.Register<ILinqValueCalculator, LinqValueCalculator>();
			aContainer.Register<IShoppingCart, ShoppingCart>();
		}
		
	}
}