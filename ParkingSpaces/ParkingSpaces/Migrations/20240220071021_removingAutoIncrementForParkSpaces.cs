using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingSpaces.Migrations
{
    public partial class removingAutoIncrementForParkSpaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ParkSpaces_ParkSpaceId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkSpaces",
                table: "ParkSpaces");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ParkSpaceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ParkSpaces");

            migrationBuilder.AddColumn<string>(
                name: "Plate",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plate",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ParkSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkSpaces",
                table: "ParkSpaces",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ParkSpaceId",
                table: "Bookings",
                column: "ParkSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ParkSpaces_ParkSpaceId",
                table: "Bookings",
                column: "ParkSpaceId",
                principalTable: "ParkSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
