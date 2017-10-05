namespace AdamFreemansDualWebSite.Web.Repository
{
	using System.Collections.Generic;
	using Models;

	public class Repository: IRepository
	{
		private Dictionary<int, Product> _Data;

		static Repository() { Current = new Repository(); }

		public static Repository Current { get; }

		public Repository()
		{
			Product[] vProducts =
			{
				new Product
				{
					ProductId = 1
					, Name = "Kayak"
					, Price = 275M
				}
				, new Product
				{
					ProductId = 2
					, Name = "Lifejacket"
					, Price = 48.95M
				}
				, new Product
				{
					ProductId = 3
					, Name = "Soccer ball"
					, Price = 19.5M
				}
				, new Product
				{
					ProductId = 4
					, Name = "Corner flag"
					, Price = 34.95M
				}
			};
			_Data = new Dictionary<int, Product>();
			foreach (Product vProduct in vProducts)
			{
				_Data.Add(vProduct.ProductId, vProduct);
			}
		}

		public IEnumerable<Product> Products => _Data.Values;

		public Product GetProduct(int aId) => _Data[aId];

		public Product SaveProduct(Product aNewProduct)
		{
			aNewProduct.ProductId = _Data.Keys.Count + 1;
			return _Data[aNewProduct.ProductId] = aNewProduct;
		}

		public Product DeleteProduct(int aId)
		{
			Product vProduct = _Data[aId];
			if (vProduct != null)
			{
				_Data.Remove(aId);
			}
			return vProduct;
		}

	}
}