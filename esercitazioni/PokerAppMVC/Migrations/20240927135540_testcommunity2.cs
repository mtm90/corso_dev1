using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class testcommunity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stack",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ComputerBet",
                table: "Hands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerBet",
                table: "Hands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pot",
                table: "Hands",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stack",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ComputerBet",
                table: "Hands");

            migrationBuilder.DropColumn(
                name: "PlayerBet",
                table: "Hands");

            migrationBuilder.DropColumn(
                name: "Pot",
                table: "Hands");
        }
    }
}
