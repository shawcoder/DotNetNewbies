namespace AdamFreemansDualWebSite.Web
{
	using System;
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			// The next line is necessary to make the Mvc injector happy...Not real
			// sure if it should come before or after the "RegisterRoutes" method, 
			// havent' tried moving it as I was so happy when it Just Worked that I
			// didn't want to fool with it!
			DependencyResolver.SetResolver
			(
				(IDependencyResolver)GlobalConfiguration
					.Configuration.DependencyResolver
			);
		}

	}
}