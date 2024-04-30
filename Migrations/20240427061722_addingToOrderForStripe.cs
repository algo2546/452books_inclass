using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksSpring2024_sec02.Migrations
{
    /// <inheritdoc />
    public partial class addingToOrderForStripe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentID",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionID",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SessionID",
                table: "Orders");
        }
    }
}
