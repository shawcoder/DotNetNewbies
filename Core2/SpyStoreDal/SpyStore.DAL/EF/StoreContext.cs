namespace SpyStoreDal.SpyStore.DAL.EF
{
	using Microsoft.EntityFrameworkCore;
	using Models.Entities;

	public class StoreContext : DbContext
	{
		private const string _CONNECTION_STRING =
			@"Server=.\SQLExpress;Database=SpyStore;Trusted_Connection=True;MultipleActiveResultSets=true";

		public StoreContext()
		{

		}

		public StoreContext(DbContextOptions aOptions) : base(aOptions)
		{

		}

		protected override void OnConfiguring
			(DbContextOptionsBuilder aOptionsBuilder)
		{
			if (!aOptionsBuilder.IsConfigured)
			{
				aOptionsBuilder.UseSqlServer
				(
					_CONNECTION_STRING
					//, options => options.EnableRetryOnFailure() // Original code
					, options =>
							options.ExecutionStrategy
								(aContext => new CustomExecutionStrategy(aContext))
				);
			}
		}

		protected override void OnModelCreating(ModelBuilder aModelBuilder)
		{
			base.OnModelCreating(aModelBuilder);
			Category.OnModelCreating(aModelBuilder);
		}

		public DbSet<Category> Categories { get; set; }
	}
}
