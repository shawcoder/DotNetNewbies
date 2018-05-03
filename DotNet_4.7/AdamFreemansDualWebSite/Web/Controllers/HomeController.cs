namespace AdamFreemansDualWebSite.Web.Controllers
{
	using System;
	using System.Web.Mvc;
	using Repository;

	public class HomeController : Controller
	{
		private readonly IRepository _Repository;

		//public HomeController()
		//{
		//	_Repository = new Repository();
		//}

		public HomeController(IRepository aRepository)
		{
			_Repository =
				aRepository ?? throw new ArgumentNullException(nameof(aRepository));
		}

		public ActionResult Index() { return View(_Repository.Products); }
		
		
		
		
		
		

	}
}