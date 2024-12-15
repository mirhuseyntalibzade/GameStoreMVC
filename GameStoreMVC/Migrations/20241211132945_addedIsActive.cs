using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class addedIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Games");
        }
    }
}
