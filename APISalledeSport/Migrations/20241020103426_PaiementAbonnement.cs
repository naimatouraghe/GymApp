using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APISalledeSport.Migrations
{
    public partial class PaiementAbonnement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paiements_Abonnements_AbonnementId",
                table: "Paiements");

            migrationBuilder.DropForeignKey(
                name: "FK_Paiements_Utilisateurs_UtilisateurId",
                table: "Paiements");

            migrationBuilder.DropIndex(
                name: "IX_Paiements_AbonnementId",
                table: "Paiements");

            migrationBuilder.DropIndex(
                name: "IX_Paiements_UtilisateurId",
                table: "Paiements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Paiements_AbonnementId",
                table: "Paiements",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_UtilisateurId",
                table: "Paiements",
                column: "UtilisateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_Abonnements_AbonnementId",
                table: "Paiements",
                column: "AbonnementId",
                principalTable: "Abonnements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_Utilisateurs_UtilisateurId",
                table: "Paiements",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
