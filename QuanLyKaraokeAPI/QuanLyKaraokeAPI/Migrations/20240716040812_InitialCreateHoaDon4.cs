using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyKaraokeAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateHoaDon4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetaiOderServiceDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OderID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    serviceTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false),
                    OpenPrice = table.Column<float>(type: "real", nullable: false),
                    PricePerHour = table.Column<float>(type: "real", nullable: false),
                    hoaDonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetaiOderServiceDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetaiOderServiceDTO_HoaDons_hoaDonID",
                        column: x => x.hoaDonID,
                        principalTable: "HoaDons",
                        principalColumn: "hoaDonID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetaiOderServiceDTO_hoaDonID",
                table: "DetaiOderServiceDTO",
                column: "hoaDonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetaiOderServiceDTO");
        }
    }
}
