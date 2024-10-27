using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APISalledeSport.Migrations
{
    public partial class Abbonement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abonnements_Utilisateurs_UtilisateurId",
                table: "Abonnements");

            migrationBuilder.DropIndex(
                name: "IX_Abonnements_UtilisateurId",
                table: "Abonnements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Abonnements_UtilisateurId",
                table: "Abonnements",
                column: "UtilisateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Abonnements_Utilisateurs_UtilisateurId",
                table: "Abonnements",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
