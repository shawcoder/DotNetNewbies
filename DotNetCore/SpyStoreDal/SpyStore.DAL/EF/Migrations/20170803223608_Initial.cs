using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStoreDal.SpyStore.DAL.EF.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
					name: "Store");

			migrationBuilder.CreateTable(
					name: "Categories",
					schema: "Store",
					columns: table => new
					{
						Id = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
						TimeStamp = table.Column<byte[]>(rowVersion: true, nullable: true)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Categories", x => x.Id);
					});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Categories",
					schema: "Store");
		}
	}
}
