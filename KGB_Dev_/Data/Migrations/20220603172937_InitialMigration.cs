using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KGB_Dev_.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KGB_Knowledge",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sifra_Oj = table.Column<int>(type: "int", nullable: false),
                    Fk_Category = table.Column<int>(type: "int", nullable: false),
                    Fk_Subcategory = table.Column<int>(type: "int", nullable: false),
                    Sifra_Prijave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naziv_Prijave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opis_Prijave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Putanja_Fajl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    d_upd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    k_ins = table.Column<int>(type: "int", nullable: false),
                    d_ins = table.Column<DateTime>(type: "datetime2", nullable: false),
                    k_upd = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KGB_Knowledge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KGB_Oj",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SifraOj = table.Column<int>(type: "int", nullable: false),
                    NazivOj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DUpd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KIns = table.Column<int>(type: "int", nullable: false),
                    DIns = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KUpd = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KGB_Oj", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KGB_UsersDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Rola = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sifra_Oj = table.Column<int>(type: "int", nullable: false),
                    Naziv_Oj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lozinka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    K_Ins = table.Column<int>(type: "int", nullable: false),
                    D_Ins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    K_Upd = table.Column<int>(type: "int", nullable: false),
                    D_Upd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KGB_UsersDb", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KGB_Knowledge");

            migrationBuilder.DropTable(
                name: "KGB_Oj");

            migrationBuilder.DropTable(
                name: "KGB_UsersDb");
        }
    }
}
