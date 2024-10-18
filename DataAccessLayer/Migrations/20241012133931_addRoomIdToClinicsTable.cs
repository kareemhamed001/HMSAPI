using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addRoomIdToClinicsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Clinics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_RoomId",
                table: "Clinics",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Rooms_RoomId",
                table: "Clinics",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Rooms_RoomId",
                table: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_RoomId",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Clinics");
        }
    }
}
