namespace Web.Models
{
	public interface IDiscountHelper
	{
		decimal ApplyDiscount(decimal aTotal);
		decimal DiscountSize { get; set; }
	}

	public class DiscountHelper: IDiscountHelper
	{
		public decimal ApplyDiscount(decimal aTotal)
		{
			decimal vResult = (aTotal - ((DiscountSize / 100M) * aTotal));
			return vResult;
		}

		public decimal DiscountSize { get; set; }
	}
}