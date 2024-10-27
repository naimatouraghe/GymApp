using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APISalledeSport.Migrations
{
    public partial class Paiement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Montant",
                table: "Paiements");

            migrationBuilder.AddColumn<decimal>(
                name: "Prix",
                table: "Paiements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
         name: "Prix",
         table: "Abonnements",
         type: "decimal(18, 2)",
         nullable: false,
         oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prix",
                table: "Paiements");

            migrationBuilder.AddColumn<decimal>(
                name: "Montant",
                table: "Paiements",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
