namespace Web.Models
{
	using Ninject;

	public interface IDiscountHelper
	{
		decimal ApplyDiscount(decimal aTotal);
		[Inject]
		decimal DiscountSize { get; set; }
	}

	public class DiscountHelper: IDiscountHelper
	{
		public decimal ApplyDiscount(decimal aTotal)
		{
			decimal vResult = (aTotal - ((DiscountSize / 100M) * aTotal));
			return vResult;
		}

		[Inject]
		public decimal DiscountSize { get; set; }
	}
}