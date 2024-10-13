using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TropicalExpress.Infrastructure.Migrations
{
    public partial class ComplexTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "public");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS public.""Orders"" (
                    ""Id"" uuid NOT NULL,
                    ""Fruit_NetWeight_Weight_Unit"" integer NOT NULL,
                    ""Fruit_NetWeight_Weight_Value"" numeric NOT NULL,
                    ""Fruit_FruitType"" text NOT NULL,
                    CONSTRAINT ""PK_Orders"" PRIMARY KEY (""Id"")
                );
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS public.""Orders"";");
        }
    }
}