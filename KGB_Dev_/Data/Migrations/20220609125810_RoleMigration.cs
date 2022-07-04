using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KGB_Dev_.Data.Migrations
{
    public partial class RoleMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Naziv_Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KGB_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sifra_Role = table.Column<int>(type: "int", nullable: false),
                    Naziv_Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KGB_Role", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KGB_Role");

            migrationBuilder.DropColumn(
                name: "Naziv_Role",
                table: "AspNetUsers");
        }
    }
}
