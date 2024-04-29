using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAPI.Migrations
{
    /// <inheritdoc />
    public partial class CriandoColunaDeLido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Readed",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Readed",
                table: "Books");
        }
    }
}
