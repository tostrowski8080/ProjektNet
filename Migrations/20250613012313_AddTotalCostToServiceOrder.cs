using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCostToServiceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalCost",
                table: "ServiceOrders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "ServiceOrders");
        }
    }
}
