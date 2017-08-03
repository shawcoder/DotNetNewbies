namespace SpyStoreDal.SpyStore.Models.Entities
{
	using System.ComponentModel.DataAnnotations.Schema;
	using Base;
	using Microsoft.EntityFrameworkCore;

	public class Category : EntityBase
	{
		public const string TABLE_NAME = "Categories";
		public const string SCHEMA_NAME = "Store";

		public static void OnModelCreating(ModelBuilder aModelBuilder)
		{
			const string NVARCHAR50 = "nvarchar(50)";
			// Setup the Table
			aModelBuilder
				.Entity<Category>()
				.ToTable(TABLE_NAME, SCHEMA_NAME);
			// Setup the inherited properties
			aModelBuilder
				.Entity<Category>()
				.Property(p => p.TimeStamp)
					.IsRowVersion()
					.IsConcurrencyToken();
			aModelBuilder
				.Entity<Category>()
				.Property(p => p.Id)
					.IsRequired()
					.ValueGeneratedOnAdd()
					.HasAnnotation("Key", DatabaseGeneratedOption.Identity);
			// Setup the rest of the entity
			aModelBuilder
				.Entity<Category>()
				.Property(p => p.CategoryName)
				//.HasColumnType(DataType.Text.ToString()) // Can't sort on text
				.HasColumnType(NVARCHAR50)
				.IsRequired()
				.HasMaxLength(50);
		}

		public string CategoryName { get; set; }
	}
}
