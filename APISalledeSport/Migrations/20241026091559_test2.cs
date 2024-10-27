using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APISalledeSport.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeAbonnement",
                table: "Paiements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeAbonnement",
                table: "Paiements");
        }
    }
}
