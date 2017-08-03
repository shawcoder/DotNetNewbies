using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpyStoreDal.SpyStore.DAL.EF.Migrations
{
	[DbContext(typeof(StoreContext))]
	partial class StoreContextModelSnapshot : ModelSnapshot
	{
		protected override void BuildModel(ModelBuilder modelBuilder)
		{
			modelBuilder
					.HasAnnotation("ProductVersion", "1.1.2")
					.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

			modelBuilder.Entity("SpyStoreDal.SpyStore.Models.Entities.Category", b =>
					{
						b.Property<int>("Id")
											.ValueGeneratedOnAdd()
											.HasAnnotation("Key", DatabaseGeneratedOption.Identity);

						b.Property<string>("CategoryName")
											.IsRequired()
											.HasColumnType("nvarchar(50)")
											.HasMaxLength(50);

						b.Property<byte[]>("TimeStamp")
											.IsConcurrencyToken()
											.ValueGeneratedOnAddOrUpdate();

						b.HasKey("Id");

						b.ToTable("Categories", "Store");
					});
		}
	}
}
