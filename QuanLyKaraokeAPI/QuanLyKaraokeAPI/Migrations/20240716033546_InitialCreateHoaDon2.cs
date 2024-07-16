using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKaraokeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateHoaDon2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailOderProductDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OderID = table.Column<int>(type: "int", nullable: false),
                    //odersName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    productsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TimeOder = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    hoaDonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailOderProductDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailOderProductDTO_HoaDons_hoaDonID",
                        column: x => x.hoaDonID,
                        principalTable: "HoaDons",
                        principalColumn: "hoaDonID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailOderProductDTO_hoaDonID",
                table: "DetailOderProductDTO",
                column: "hoaDonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailOderProductDTO");
        }
    }
}
