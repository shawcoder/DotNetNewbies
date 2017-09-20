namespace Web
{
	using Models;
	using SimpleInjector;

	public static class InitializeContainerBindings
	{
		public static void InitializeBindings(Container aContainer)
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