using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SOSTransito.Migrations
{
    public partial class firtMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Localidade",
                columns: table => new
                {
                    LocalidadeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Regiao = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidade", x => x.LocalidadeID);
                    table.ForeignKey(
                        name: "FK_Localidade_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalidadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Cliente_Localidade_LocalidadeId",
                        column: x => x.LocalidadeId,
                        principalTable: "Localidade",
                        principalColumn: "LocalidadeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CNH",
                columns: table => new
                {
                    CNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroCNH = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    ValidadeCNH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusCNH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CNH", x => x.CNHId);
                    table.ForeignKey(
                        name: "FK_CNH_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veiculo",
                columns: table => new
                {
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    RENAVAN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.VeiculoId);
                    table.ForeignKey(
                        name: "FK_Veiculo_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PAT",
                columns: table => new
                {
                    PATId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrgAtuador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Veiculo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pontuacao = table.Column<int>(type: "int", nullable: false),
                    Processo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNHId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PAT", x => x.PATId);
                    table.ForeignKey(
                        name: "FK_PAT_CNH_CNHId",
                        column: x => x.CNHId,
                        principalTable: "CNH",
                        principalColumn: "CNHId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_LocalidadeId",
                table: "Cliente",
                column: "LocalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_CNH_ClienteId",
                table: "CNH",
                column: "ClienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Localidade_UsuarioId",
                table: "Localidade",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PAT_CNHId",
                table: "PAT",
                column: "CNHId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_ClienteId",
                table: "Veiculo",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PAT");

            migrationBuilder.DropTable(
                name: "Veiculo");

            migrationBuilder.DropTable(
                name: "CNH");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Localidade");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
