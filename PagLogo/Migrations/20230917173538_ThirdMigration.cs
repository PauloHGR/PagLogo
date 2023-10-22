using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagLogo.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Genrics",
                table: "Genrics");

            migrationBuilder.RenameTable(
                name: "Genrics",
                newName: "Generics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Generics",
                table: "Generics",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Generics",
                table: "Generics");

            migrationBuilder.RenameTable(
                name: "Generics",
                newName: "Genrics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genrics",
                table: "Genrics",
                column: "Id");
        }
    }
}
