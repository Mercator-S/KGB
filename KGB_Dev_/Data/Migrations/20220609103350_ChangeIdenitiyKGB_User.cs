using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KGB_Dev_.Data.Migrations
{
    public partial class ChangeIdenitiyKGB_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KGB_UsersDb");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "D_Ins",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "D_Upd",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Fk_Rola",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "K_Ins",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "K_Upd",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Lozinka",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Naziv_Oj",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sifra_Oj",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "D_Ins",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "D_Upd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fk_Rola",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "K_Ins",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "K_Upd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Lozinka",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Naziv_Oj",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Prezime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sifra_Oj",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "KGB_UsersDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    D_Ins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    D_Upd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Rola = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    K_Ins = table.Column<int>(type: "int", nullable: false),
                    K_Upd = table.Column<int>(type: "int", nullable: false),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naziv_Oj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sifra_Oj = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KGB_UsersDb", x => x.Id);
                });
        }
    }
}
