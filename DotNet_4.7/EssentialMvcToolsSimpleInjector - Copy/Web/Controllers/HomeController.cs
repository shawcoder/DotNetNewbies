namespace Web.Controllers
{
	using System;
	using System.Web.Mvc;
	using Models;

	public class HomeController : Controller
	{
		private readonly IShoppingCart _ShoppingCart;
		private readonly Product[] _Products =
			{
				new Product
					{
						Name = "Kayak"
						, Description = "Kayak"
						, Category = "Watersports"
						, Price = 275M
					}
				, new Product
					{
						Name = "Lifejacket"
						, Description = "Lifejacket"
						, Category = "Watersports"
						, Price = 48.95M
					}
				, new Product
					{
						Name = "Soccer ball"
						, Description = "Soccer ball"
						, Category = "Soccer"
						, Price = 19.5M
					}
				, new Product
					{
						Name = "Corner flag"
						, Description = "Corner flag"
						, Category = "Soccer"
						, Price = 34.95M
					}
			};

		public HomeController(IShoppingCart aShoppingCart)
		{
			_ShoppingCart = aShoppingCart ?? throw new ArgumentNullException();
			_ShoppingCart.Products = _Products;
		}

		/// <summary>
		/// Why is this here? Because if it isn't you get a whiny complaint from the
		/// ASP.NET gadget complaining that it can't find a parameterless 
		/// constructor for this object (necessary to make IDisposable happy).
		/// </summary>
		//public HomeController() 
		//	: this(new ShoppingCart(new LinqValueCalculator(new DiscountHelper())))
		//{
		//	// Nothing else needed here.	
		//}

		public ActionResult Index()
		{
			// Original without DI
			//ILinqValueCalculator vCalc = new LinqValueCalculator();
			//ShoppingCart vCart = 
			//	new ShoppingCart(vCalc)
			//	{
			//		Products = _Products
			//	};
			//decimal vTotalValue = vCart.CalculateProductTotal();

			// With single layer DI
			//decimal vTotalValue = _ShoppingCart.CalculateProductTotal();

			// With chained DI
			//decimal vTotalValue = _ShoppingCart.CalculateDiscountedProductTotal();
			IndexViewDTO vIndexViewDTO = 
				new IndexViewDTO
				{
					FullPrice = _ShoppingCart.CalculateProductTotal()
					, DiscountedPrice = _ShoppingCart.CalculateDiscountedProductTotal()
				};
			return View(vIndexViewDTO);
		}

	}
}