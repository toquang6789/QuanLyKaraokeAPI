using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKaraokeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateHoaDon3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "odersName",
                table: "DetailOderProductDTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "odersName",
                table: "DetailOderProductDTO",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
