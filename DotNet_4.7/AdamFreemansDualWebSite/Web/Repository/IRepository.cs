namespace AdamFreemansDualWebSite.Web.Repository
{
	using System.Collections.Generic;
	using Models;

	public interface IRepository
	{
		IEnumerable<Product> Products { get; }
		Product GetProduct(int aId);
		Product SaveProduct(Product aNewProduct);
		Product DeleteProduct(int aId);
	}
}