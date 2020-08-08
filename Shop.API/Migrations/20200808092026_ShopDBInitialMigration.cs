using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.API.Migrations
{
    public partial class ShopDBInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    InStock = table.Column<bool>(nullable: false),
                    Favourite = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Favourite", "InStock", "Name", "Price" },
                values: new object[] { 1, "Our famous apple!", false, true, "Apple", 12.95m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Favourite", "InStock", "Name", "Price" },
                values: new object[] { 2, "Our famous pear!", true, false, "Pear", 9.95m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Favourite", "InStock", "Name", "Price" },
                values: new object[] { 3, "Our famous cheese!", false, true, "Cheese", 15.95m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
