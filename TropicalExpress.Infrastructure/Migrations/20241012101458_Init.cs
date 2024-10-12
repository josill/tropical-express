using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicalExpress.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fruits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NetWeight_Weight_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NetWeight_Weight_Value = table.Column<decimal>(type: "numeric", nullable: false),
                    NetWeight_Weight_WeightUnit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fruits", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fruits");
        }
    }
}
