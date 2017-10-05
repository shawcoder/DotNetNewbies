namespace AdamFreemansDualWebSite.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using Models;
	using Repository;

	public class ProductsController : ApiController
	{
		private readonly IRepository _Repository;

		public ProductsController() { _Repository = Repository.Current; }

		/// <remark>
		/// My interest was truly piqued when I disovered that the injection 
		/// process works just fine all by itself without having to register the
		/// Controller classes in any. It Just Works!
		/// </remark>
		/// <param name="aRepository"></param>
		public ProductsController(IRepository aRepository)
		{
			_Repository =
				aRepository ?? throw new ArgumentNullException(nameof(aRepository));
		}

		public IEnumerable<Product> GetAll() { return _Repository.Products; }

		/// <remark>
		/// Why am I doing a Post here? In the read-world project I'm working on, I
		/// need to send a rather large object containing selection criteria, etc.
		/// to perform the necessary action and it would be ugly indeed trying to
		/// fit the entire thing in a query string (it's a Json object), hence the
		/// post.
		/// </remark>
		/// <returns></returns>
		[HttpPost]
		public IEnumerable<Product> PostAllProducts()
		{
			return _Repository.Products;
		}

	}
}