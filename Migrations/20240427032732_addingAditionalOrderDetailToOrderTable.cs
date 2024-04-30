using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksSpring2024_sec02.Migrations
{
    /// <inheritdoc />
    public partial class addingAditionalOrderDetailToOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ShippingDate",
                table: "Orders",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "Orders");
        }
    }
}
