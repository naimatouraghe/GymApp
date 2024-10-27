using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APISalledeSport.Migrations
{
    public partial class AddAbonnementModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Paiements_AbonnementId",
                table: "Paiements",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_UtilisateurId",
                table: "Paiements",
                column: "UtilisateurId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Paiements_Abonnements_AbonnementId",
                table: "Paiements",
                column: "AbonnementId",
                principalTable: "Abonnements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            /*  migrationBuilder.AddForeignKey(
                  name: "FK_Paiements_Utilisateurs_UtilisateurId",
                  table: "Paiements",
                  column: "UtilisateurId",
                  principalTable: "Utilisateurs",
                  principalColumn: "Id",
                  onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Abonnements_Utilisateurs_UtilisateurId",
                table: "Abonnements");

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

            migrationBuilder.DropIndex(
                name: "IX_Abonnements_UtilisateurId",
                table: "Abonnements");
        }
    }
}
