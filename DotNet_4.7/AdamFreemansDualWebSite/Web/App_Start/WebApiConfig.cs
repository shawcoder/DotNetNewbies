namespace AdamFreemansDualWebSite.Web
{
	using System.Web.Http;
	using Infrastructure;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			// The next line is needed to make the WebAPI injector happy...
			config.DependencyResolver = new NinjectResolver();

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute
			(
				name: "DefaultApi"
				, routeTemplate: "api/{controller}/{id}"
				, defaults: new
				{
					id = RouteParameter.Optional
				}
			);
		}

	}
}