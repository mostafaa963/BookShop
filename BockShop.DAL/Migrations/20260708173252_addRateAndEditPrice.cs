using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BockShop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRateAndEditPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "Books",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Books");
        }
    }
}
