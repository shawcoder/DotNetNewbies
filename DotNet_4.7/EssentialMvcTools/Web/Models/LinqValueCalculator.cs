namespace Web.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public interface ILinqValueCalculator
	{
		decimal ValueProducts(IEnumerable<Product> aProductList);
		decimal DiscountedValue(IEnumerable<Product> aProductList);
	}

	public class LinqValueCalculator: ILinqValueCalculator
	{
		private readonly IDiscountHelper _DiscountHelper;

		public LinqValueCalculator(IDiscountHelper aDiscountHelper)
		{
			_DiscountHelper = aDiscountHelper ?? throw new ArgumentNullException();
		}

		private decimal ProductSum(IEnumerable<Product> aProductList)
		{
			decimal vResult = aProductList.Sum(p => p.Price);
			return vResult;
		}

		public decimal ValueProducts(IEnumerable<Product> aProductList)
		{
			decimal vResult = ProductSum(aProductList);
			return vResult;
		}

		public decimal DiscountedValue(IEnumerable<Product> aProductList)
		{
			decimal vResult = _DiscountHelper.ApplyDiscount(ProductSum(aProductList));
			return vResult;
		}

	}
}