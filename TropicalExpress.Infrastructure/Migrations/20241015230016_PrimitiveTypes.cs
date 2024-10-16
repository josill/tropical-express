using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicalExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrimitiveTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FruitType = table.Column<string>(type: "varchar(50)", nullable: false),
                    WeightData = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
