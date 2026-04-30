using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaInterface.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelaRelatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoRelatorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataGeracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalItens = table.Column<int>(type: "INTEGER", nullable: false),
                    ValorTotalEstoque = table.Column<decimal>(type: "TEXT", nullable: false),
                    Resumo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoRelatorios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoRelatorios");
        }
    }
}
