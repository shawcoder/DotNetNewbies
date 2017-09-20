namespace SpyStore.DAL2.SpyStore.DAL.Models.Entities
{
	using System.ComponentModel.DataAnnotations;
	using Base;
	using System.Data.Entity;

	public class Category : EntitiesBase
	{
		public const string TABLE_NAME = "Categories";
		public const string SCHEMA_NAME = "Store";

		public static void OnModelCreating(ModelBuilder aModelBuilder)
		{
			aModelBuilder
				.Entity<Category>()
				.ToTable(TABLE_NAME, SCHEMA_NAME)
				.Property(p => p.CategoryName)
				.HasColumnType(DataType.Text.ToString())
				.IsRequired()
				.HasMaxLength(50);
			aModelBuilder
				.Entity<Category>()
				.Property(p => p.TimeStamp)
				.IsRowVersion()
				.IsConcurrencyToken();
		}

		public string CategoryName { get; set; }
	}
}