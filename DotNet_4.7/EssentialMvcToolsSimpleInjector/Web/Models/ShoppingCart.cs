namespace Web.Models
{
	using System;
	using System.Collections.Generic;

	public interface IShoppingCart
	{
		decimal CalculateProductTotal();
		decimal CalculateDiscountedProductTotal();
		IEnumerable<Product> Products { get; set; }
	}

	public class ShoppingCart: IShoppingCart
	{
		private readonly ILinqValueCalculator _LinqValueCalculator;

		public ShoppingCart(ILinqValueCalculator aLinqValueCalculator)
		{
			_LinqValueCalculator =
				aLinqValueCalculator ?? throw new ArgumentNullException();
		}

		public decimal CalculateProductTotal()
		{
			return _LinqValueCalculator.ValueProducts(Products);
		}

		public decimal CalculateDiscountedProductTotal()
		{
			return _LinqValueCalculator.DiscountedValue(Products);
		}

		public IEnumerable<Product> Products { get; set; }
	}
}