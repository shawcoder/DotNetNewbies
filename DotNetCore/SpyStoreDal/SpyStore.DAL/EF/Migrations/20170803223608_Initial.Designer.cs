using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStoreDal.SpyStore.DAL.EF.Migrations
{
	[DbContext(typeof(StoreContext))]
	[Migration("20170803223608_Initial")]
	partial class Initial
	{
		protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
