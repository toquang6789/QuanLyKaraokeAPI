using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKaraokeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TotalProduct",
                table: "HoaDons",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalService",
                table: "HoaDons",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalProduct",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "TotalService",
                table: "HoaDons");
        }
    }
}
