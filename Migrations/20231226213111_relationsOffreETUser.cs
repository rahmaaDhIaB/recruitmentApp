using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    public partial class relationsOffreETUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Offres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offres_UserId",
                table: "Offres",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offres_Users_UserId",
                table: "Offres",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offres_Users_UserId",
                table: "Offres");

            migrationBuilder.DropIndex(
                name: "IX_Offres_UserId",
                table: "Offres");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offres");
        }
    }
}
