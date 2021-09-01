using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SOSTransito.Migrations
{
    public partial class @new : Migration
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
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "Localidade",
                columns: table => new
                {
                    LocalidadeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Regiao = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localidade", x => x.LocalidadeId);
                    table.ForeignKey(
                        name: "FK_Localidade_Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atribuicao_Localidade",
                columns: table => new
                {
                    ATRLOCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalidadeId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atribuicao_Localidade", x => x.ATRLOCId);
                    table.ForeignKey(
                        name: "FK_Atribuicao_Localidade_Localidade_LocalidadeId",
                        column: x => x.LocalidadeId,
                        principalTable: "Localidade",
                        principalColumn: "LocalidadeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atribuicao_Localidade_Usuario_UsuarioId",
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
                    CPF = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Endereco = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalidadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Cliente_Localidade_LocalidadeId",
                        column: x => x.LocalidadeId,
                        principalTable: "Localidade",
                        principalColumn: "LocalidadeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CNH",
                columns: table => new
                {
                    CNHId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistroCNH = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidadeCNH = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusCNH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusSistema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalizadorHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Multa",
                columns: table => new
                {
                    MultaId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_Multa", x => x.MultaId);
                    table.ForeignKey(
                        name: "FK_Multa_CNH_CNHId",
                        column: x => x.CNHId,
                        principalTable: "CNH",
                        principalColumn: "CNHId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atribuicao_Localidade_LocalidadeId",
                table: "Atribuicao_Localidade",
                column: "LocalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Atribuicao_Localidade_UsuarioId",
                table: "Atribuicao_Localidade",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_LocalidadeId",
                table: "Cliente",
                column: "LocalidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_CNH_ClienteId",
                table: "CNH",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Localidade_UsuarioID",
                table: "Localidade",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Multa_CNHId",
                table: "Multa",
                column: "CNHId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_ClienteId",
                table: "Veiculo",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atribuicao_Localidade");

            migrationBuilder.DropTable(
                name: "Multa");

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
